

using System.Text;

using K4os.Compression.LZ4.Internal;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using MigracaoTabelas.Source;
using MigracaoTabelas.Target;

using Mysqlx.Crud;

using Serilog;

using static System.Net.Mime.MediaTypeNames;


namespace MigracaoTabelas.Worker
{
    public class MigratorWorker
    {
        private IServiceScope _Scope;
        private TxDbContext _TContext;
        private SxDbContext _SContext;
        private List<SxAgencia> _AgenciaPA;
        private DataCache _DataCache;
        private DataBaseSess _SAgencia;
        public int TotalContratos;
        public int TotalMigrado;

        private class DataBaseSess
        {
            public string TABLE_SCHEMA
            {
                get; set;
            }
            public string Codigo
            {
                get; set;
            }
        }

        public MigratorWorker(IServiceScope pScope, TxDbContext pContext)
        {
            _Scope = pScope;
            _TContext = pContext;
        }

        public void Migrate()
        {
            List<DataBaseSess> databsess;
            SxDbContext.Schema = "unico";
            using var srcctx = _Scope.ServiceProvider.GetRequiredService<SxDbContext>();
            IDbContextTransaction tx = null;
            var tableOrder = new[] {"agencia","cooperado","seguradora","seguro_parametro","ponto_atendimento","seguradora_limite","proposta_seguradora",
                                    "condicao_seguradora","conta_corrente_seguradora","contabilizacao_seguradora","comissao_seguradora",
                                    "cooperado_agencia_conta","apolice_grupo_seguradora","seguro","parcela"};

            try
            {
                databsess = srcctx.Database.SqlQueryRaw<DataBaseSess>("SELECT CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) TABLE_SCHEMA,AG_CODIGO Codigo FROM unico.cd_agencia WHERE AG_ATIVA = 1").ToList();
                tx = _TContext.Database.BeginTransaction();
                var file = new FileStream(@$"D:\CrediSIS\DBs\inserts.sql", FileMode.Create, FileAccess.Write);
                var strWriter = new StreamWriter(file);
                strWriter.AutoFlush = true;
                strWriter.WriteLine("SET NAMES 'utf8mb4';");
                strWriter.WriteLine();
                strWriter.WriteLine("start transaction;");
                strWriter.WriteLine();
                var defaultData = File.ReadAllText(@$"Default Data.sql");
                strWriter.WriteLine(defaultData);
                Agencia ag = null;
                databsess.ForEach(d => Log.Information("Aência encontra para migração [" + d.TABLE_SCHEMA + "]"));
                MigraExistente();
                _DataCache = new DataCache(_TContext);


                foreach (var sagencia in databsess)
                {
                    _SAgencia = sagencia;
                    SxDbContext.Schema = sagencia.TABLE_SCHEMA;
                    Log.Information($"[{sagencia.TABLE_SCHEMA}]");
                    using (var sctx = _Scope.ServiceProvider.GetRequiredService<SxDbContext>())
                    {
                        ag = _DataCache.GetAgencia(sctx, sagencia.Codigo);
                        _DataCache.GetPontoAtendimento(sctx, sagencia.Codigo, "000");
                        lock (_TContext)
                        {
                            Log.Information($"Iniciando migração da Agencia [{ag.Nome}] Códiog [{ag.Codigo}] id[{ag.Id}]");
                            Console.WriteLine();
                        }
                        _SContext = sctx;
                        MigraDados(sctx);
                        sctx.Dispose();
                    }
                }

                foreach (var table in tableOrder)
                    foreach (var t in _TContext.CapturedInserts.Where(i => i.Table == table))
                        strWriter.WriteLine(t.Insert);

                var tables = _TContext.CapturedInserts.Select(i => i.Table).Distinct().ToList();
                foreach (var table in tables)
                    File.AppendAllText(@$"D:\CrediSIS\DBs\Tables.txt", table + Environment.NewLine);

                tx.Commit();
                strWriter.WriteLine();

                strWriter.WriteLine("INSERT INTO usuario(id, ponto_atendimento_id, perfil_id, login, nome, email, status, criado_em) " +
                    "VALUES(23, 1, 1, 'portal.seguradoras', 'Usuario de Sistema do ERP', 'mailto@etc.com', 'Ativo', SYSDATE());");

                strWriter.WriteLine("update seguro set usuario_id = 23 where usuario_id is null;");

                strWriter.WriteLine();
                AddicionaUsuarios(strWriter);
                strWriter.WriteLine();
                strWriter.WriteLine("commit;");
                file.Close();
                file.Dispose();
                Log.Information($"\r\nMigração da Agencia [{ag.Nome}] Código [{ag.Codigo}] id[{ag.Id}] concluída com sucesso!");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                Log.Fatal(ex, "Erro durante a migração [REVISE CUIDADOSAMENTO E LOG]: " + ex.Message);
            }
        }

        private void AddicionaUsuarios(StreamWriter strWriter)
        {
            var sql = "'tiberio.neto','Tiberio Cardoso de Oliveira Neto','tiberio.neto@credisis.com.b',(select id from perfil p where p.slug  ='cooperativa_singular' limit 1),'Ativo',SYSDATE() from agencia a join ponto_atendimento p on p.agencia_id =a.id where a.codigo ='0001' and p.codigo ='000' ";
            var lines = File.ReadAllLines(@"Usuários a serem cadastrados.csv", Encoding.Default);
            for (int i = 1; i < lines.Length; i++)
            {
                var ln = lines[i].Split(';', StringSplitOptions.RemoveEmptyEntries);
                //Agencia; Ponto Atendimento; Usuario; Nome; E-mail;  Perfil; perfil_id;
                //      0    1                 2        3     4        5       6

                strWriter.Write("INSERT INTO usuario (ponto_atendimento_id, perfil_id, login, nome, email, status, criado_em) (select p.id, ");
                strWriter.WriteLine($"{ln[6]}, '{ln[2]}', '{ln[3]}','{ln[4]}', 'Ativo', SYSDATE() from agencia a join ponto_atendimento p on p.agencia_id =a.id where a.codigo ='{ln[0]}' and p.codigo ='{ln[1]}' );");
            }
        }

        private void MigraExistente()
        {
            using var dbx = _Scope.ServiceProvider.GetRequiredService<TxDbContext>();
            dbx.Connection = "Server=172.30.98.12;Database=portal_seguros;User Id=portalseguros;Password=Qu2{Zug8+Vo9;Port=3306;Pooling=true";
            var agencias = dbx.Agencia.AsNoTracking().ToList();
            var pontos = dbx.PontoAtendimento.AsNoTracking().ToList();
            var segurados = dbx.Seguradora.AsNoTracking().ToList();
            var CondicaoSeguradora = dbx.CondicaoSeguradora.AsNoTracking().ToList();
            var GestaoDocumento = dbx.GestaoDocumento.AsNoTracking().ToList();
            var ContabilizacaoSeguradora = dbx.ContabilizacaoSeguradora.AsNoTracking().ToList();
            var ContaCorrenteSeguradora = dbx.ContaCorrenteSeguradora.AsNoTracking().ToList();
            var PropostaSeguradora = dbx.PropostaSeguradora.AsNoTracking().ToList();
            var ComissaoSeguradora = dbx.ComissaoSeguradora.AsNoTracking().ToList();
            var SeguradoraLimite = dbx.SeguradoraLimite.AsNoTracking().ToList();
            var ApoliceGrupoSeguradora = dbx.ApoliceGrupoSeguradora.AsNoTracking().ToList();

            agencias.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            pontos.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            segurados.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            CondicaoSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            GestaoDocumento.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            ContabilizacaoSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            ContaCorrenteSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            PropostaSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            ComissaoSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            SeguradoraLimite.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
            ApoliceGrupoSeguradora.ForEach(a => _TContext.Add(a));
            _TContext.SaveChanges();
        }

        private void MigraDados(SxDbContext sctx)
        {
            _AgenciaPA = null;
            GetPontoAtendimentoId("000");

            var segprestamistas = sctx.EpSegPrestamista.AsNoTracking().ToList();
            if (segprestamistas.Count == 0)
            {
                Log.Information($"Nenhum Contrato encontrado para migração da Base de Dados [{SxDbContext.Schema}].");
                return;
            }

            var segparcelas = sctx.EpSegParcelas.AsNoTracking().ToList();
            if (segparcelas.Count == 0)
            {
                Log.Information($"Nenhuma Parcela de Contrato encontrada para migração da Base de Dados [{SxDbContext.Schema}].");
                return;
            }

            var parcelas = segparcelas
                .GroupBy(p => (p.CcoConta, p.SegContrato, p.ConSeq))
                .ToDictionary(g => g.Key, g => g.ToList());

            var totalPrestamistas = segprestamistas.Count;
            TotalContratos += segprestamistas.Count;
            var processados = 0;
            var parcnt = 0;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var lastUpdate = DateTime.MinValue;
            var updateInterval = TimeSpan.FromSeconds(1);

            var cts = new CancellationTokenSource();

            var parallelOptions = new ParallelOptions
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            var seguros = new List<Seguro>();

            foreach (var prestamista in segprestamistas)
            {
                try
                {
                    processados++;
                    parcnt += Migrate(parcelas, seguros, prestamista);
                    if (seguros.Count > 50)
                    {
                        _TContext.AddRange(seguros);
                        _TContext.SaveChanges();
                        TotalMigrado += seguros.Count;
                        seguros.Clear();
                    }
                    lastUpdate = ETA(totalPrestamistas, processados, stopwatch, lastUpdate, updateInterval);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\r\nErro ao migrar prestamista Conta [{prestamista.CcoConta}] Contrato [{prestamista.SegContrato}]: {ex.Message}");
                }
            }
            if (seguros.Count > 0)
            {
                _TContext.AddRange(seguros);
                _TContext.SaveChanges();
                TotalMigrado += seguros.Count;
            }
            stopwatch.Stop();
            lock (_TContext)
                Console.WriteLine($"\r\nMigrados {processados} Prestamistas e {parcnt} Parcelas. Tempo Total: {stopwatch.Elapsed:hh\\:mm\\:ss} ");
        }

        private static DateTime ETA(int totalPrestamistas, int processados, System.Diagnostics.Stopwatch stopwatch, DateTime lastUpdate, TimeSpan updateInterval)
        {
            if (totalPrestamistas == 0)
                return lastUpdate;
            var now = DateTime.Now;
            if (now - lastUpdate > updateInterval || processados == totalPrestamistas)
            {
                lastUpdate = now;
                var elapsed = stopwatch.Elapsed;

                if (processados > 0 && elapsed.TotalMilliseconds > 0)
                {
                    var avgTimePerItem = elapsed.TotalMilliseconds / processados;
                    var itemsRemaining = totalPrestamistas - processados;
                    var remainingTime = TimeSpan.FromMilliseconds(avgTimePerItem * itemsRemaining);
                    var percentComplete = (double)processados / totalPrestamistas;

                    Console.Write($"\rProgresso: {processados}/{totalPrestamistas} ({percentComplete:P1}) - Tempo Restante Estimado: {remainingTime:hh\\:mm\\:ss}   ");
                }
            }

            return lastUpdate;
        }

        private int Migrate(Dictionary<(string CcoConta, string SegContrato, short ConSeq), List<SxEpSegParcela>> pParcelas, List<Seguro> pSeguros, SxEpSegPrestamista pPrestamista)
        {
            if (!pParcelas.TryGetValue((pPrestamista.CcoConta, pPrestamista.SegContrato, pPrestamista.ConSeq), out var parcelasSrc))
                parcelasSrc = new List<SxEpSegParcela>();
            var tgt = new Seguro();
            tgt.Assign(pPrestamista);
            var (cooagct, cooperado, conta) = GetCooperadoId(pPrestamista);
            var agenciaId = GetAgenciaId();
            var tipoCapital = pPrestamista.SegNome.Contains("VARIAVEL") ? TipoCapitalApolice.Variavel : TipoCapitalApolice.Fixo;
            tgt.CooperadoAgenciaContaId = cooagct.Id;
            tgt.PontoAtendimentoId = GetPontoAtendimentoId(conta.PaCodigo);
            var (apoliceGrupoSeguradoraId, seguradoraNome) = GetAgenciaSeguradoraId(pPrestamista, agenciaId, tipoCapital.AsString(), parcelasSrc.Count > 1);
            tgt.ApoliceGrupoSeguradoraId = apoliceGrupoSeguradoraId;
            tgt.CooperadoAgenciaContaId = cooagct.Id;
            tgt.TipoPagamento = parcelasSrc.Count > 1 ? TipoPagamentoSeguro.Parcelado : TipoPagamentoSeguro.AVista ;
            //tgt.UsuarioId = 18;

            // Obtém a seguradora para buscar as comissões
            var seguradora = _DataCache.GetSeguradora(_SContext, pPrestamista.PstCodigo);
            var comissao = seguradora.ComissoesSeguradoras.FirstOrDefault();

            var spar = new SeguroParametro();
            spar.TipoCapital = tipoCapital;

            if (tipoCapital == TipoCapitalApolice.Variavel)
                spar.Coeficiente = 0.0003511M;
            else
                spar.Coeficiente = 0.0005945M;

            spar.Periodicidade30Dias = true;
            spar.PorcentualIof = 0.0038M;
            spar.PorcentagemComissaoCorretora = comissao?.PorcentagemComissaoCorretora ?? 0.45M;
            spar.PorcentagemComissaoCooperativa = comissao?.PorcentagemComissaoCooperativa ?? 0.20M;
            tgt.SeguroParametro = spar;
            if (parcelasSrc.Count == 0)
            {
                var parcelaTgt = new Parcela();
                parcelaTgt.Status = StatusParcela.Pago;
                parcelaTgt.NumeroParcela = 1;
                parcelaTgt.ValorOriginal = parcelaTgt.ValorParcela = pPrestamista.SegPremio.HasValue ? pPrestamista.SegPremio.Value : 0;
                parcelaTgt.DataUltimoPagamento = parcelaTgt.Liquidacao = parcelaTgt.Vencimento = pPrestamista.SegInicio.Value;
                parcelaTgt.ValorPago = parcelaTgt.ValorOriginal;
                tgt.Parcelas.Add(parcelaTgt);
            }

            foreach (var item in parcelasSrc)
            {
                var parcelaTgt = new Parcela();
                parcelaTgt.Assign(item);
                tgt.Parcelas.Add(parcelaTgt);
            }

            pSeguros.Add(tgt);

            return parcelasSrc?.Count ?? 0;
        }

        private (ulong ID, string Nome) GetAgenciaSeguradoraId(SxEpSegPrestamista prestamista, ulong agenciaId, string pTipoCapital, bool pModalidadeParcelado)
        {
            var seguradoraId = _DataCache.GetSeguradora(_SContext, prestamista.PstCodigo).Id;
            return (_DataCache.GetAgenciaSeguradora(agenciaId, seguradoraId, prestamista.SegNome, pTipoCapital, pModalidadeParcelado), prestamista.SegNome);
        }

        private (CooperadoAgenciaConta, Cooperado, SxContas) GetCooperadoId(SxEpSegPrestamista prestamista)
        {
            return _DataCache.GetCooperado(_SContext, prestamista.SegCpf, prestamista.CcoConta, _SAgencia.Codigo);
        }

        private ulong GetPontoAtendimentoId(string pPonto)
        {
            var pv = _DataCache.GetPontoAtendimento(_SContext, _SAgencia.Codigo, pPonto);
            return pv.Id;

        }

        private ulong GetAgenciaId()
        {
            return _DataCache.GetAgencia(_SContext, _SAgencia.Codigo).Id;
        }
    }
}
