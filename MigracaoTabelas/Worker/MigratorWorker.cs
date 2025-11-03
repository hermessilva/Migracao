

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using MigracaoTabelas.Source;
using MigracaoTabelas.Target;

using Serilog;


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
            public string TABLE_SCHEMA { get; set; }
            public string Codigo { get; set; }
        }

        public MigratorWorker(IServiceScope pScope, TxDbContext pContext)
        {
            _Scope = pScope;
            _TContext = pContext;
            _DataCache = new DataCache(_TContext);
        }

        public void Migrate()
        {
            List<DataBaseSess> databsess;
            SxDbContext.Schema = "unico";
            using var srcctx = _Scope.ServiceProvider.GetRequiredService<SxDbContext>();
            databsess = srcctx.Database.SqlQueryRaw<DataBaseSess>("SELECT CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) TABLE_SCHEMA,AG_CODIGO Codigo FROM unico.cd_agencia WHERE AG_ATIVA = 1").ToList();
            IDbContextTransaction tx = null;
            try
            {
                tx = _TContext.Database.BeginTransaction();
                Agencia ag = null;
                databsess.ForEach(d => Log.Information("Aência encontra para migração [" + d.TABLE_SCHEMA + "]"));
                foreach (var sagencia in databsess)
                {
                    _SAgencia = sagencia;
                    SxDbContext.Schema = sagencia.TABLE_SCHEMA;
                    using (var sctx = _Scope.ServiceProvider.GetRequiredService<SxDbContext>())
                    {
                        ag = _DataCache.GetAgencia(sctx, sagencia.Codigo);
                        Console.WriteLine();
                        Log.Information($"Iniciando migração da Agencia [{ag.Nome}] Códiog [{ag.Codigo}] id[{ag.Id}]");
                        _SContext = sctx;
                        MigraDados(sctx);
                    }
                }
                tx.Commit();
                Log.Information($"Migração da Agencia [{ag.Nome}] Código [{ag.Codigo}] id[{ag.Id}] concluída com sucesso!");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                Log.Fatal(ex, "Erro durante a migração [REVISE CUIDADOSAMENTO E LOG]: " + ex.Message);
            }
        }

        private void MigraDados(SxDbContext sctx)
        {
            _AgenciaPA = null;
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
                .GroupBy(p => (p.CcoConta, p.SegContrato))
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
                    if (seguros.Count > 100)
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
                    Log.Error($"Erro ao migrar prestamista Conta [{prestamista.CcoConta}] Contrato [{prestamista.SegContrato}]: {ex.Message}");
                }
            }
            if (seguros.Count > 0)
            {
                _TContext.AddRange(seguros);
                _TContext.SaveChanges();
                TotalMigrado += seguros.Count;
            }
            stopwatch.Stop();
            Log.Information($"Migrados {processados} Prestamistas e {parcnt} Parcelas. Tempo Total: {stopwatch.Elapsed:hh\\:mm\\:ss} ");
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

        private int Migrate(Dictionary<(string CcoConta, string SegContrato), List<SxEpSegParcela>> pParcelas, List<Seguro> pSeguros, SxEpSegPrestamista pPrestamista)
        {
            pParcelas.TryGetValue((pPrestamista.CcoConta, pPrestamista.SegContrato), out var parcelasSrc);
            if (parcelasSrc == null || parcelasSrc.Count == 0)
                return 0;
            var tgt = new Seguro();
            tgt.Assign(pPrestamista);
            var (cooagct, cooperado, conta) = GetCooperadoId(pPrestamista);
            var agenciaId = GetAgenciaId();
            tgt.CooperadoAgenciaContaId = cooagct.Id;
            tgt.PontoAtendimentoId = GetPontoAtendimentoId(conta.PaCodigo);
            tgt.AgenciaSeguradoraId = GetAgenciaSeguradoraId(pPrestamista, agenciaId);
            tgt.CooperadoAgenciaContaId = cooagct.Id;

            foreach (var item in parcelasSrc)
            {
                var parcelaTgt = new Parcela();
                parcelaTgt.Assign(item);
                tgt.Parcelas.Add(parcelaTgt);
            }

            pSeguros.Add(tgt);

            return parcelasSrc?.Count ?? 0;
        }

        private ulong GetAgenciaSeguradoraId(SxEpSegPrestamista prestamista, ulong agenciaId)
        {
            var seguradoraId = _DataCache.GetSeguradora(_SContext, prestamista.PstCodigo).Id;
            return _DataCache.GetAgenciaSeguradora(agenciaId, seguradoraId);
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
