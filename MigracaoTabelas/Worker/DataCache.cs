using Microsoft.EntityFrameworkCore;

using MigracaoTabelas.Source;
using MigracaoTabelas.Target;

namespace MigracaoTabelas.Worker
{
    public class DataCache
    {
        private Dictionary<string, Agencia> Agencias = new Dictionary<string, Agencia>();
        private Dictionary<(string, string), PontoAtendimento> PontosAtendimento = new Dictionary<(string, string), PontoAtendimento>();
        private Dictionary<string, Cooperado> Cooperados = new Dictionary<string, Cooperado>();
        private Dictionary<string, Seguradora> Seguradoras = new Dictionary<string, Seguradora>();
        private Dictionary<(ulong, ulong), AgenciaSeguradora> AgenciasSeguradoras = new Dictionary<(ulong, ulong), AgenciaSeguradora>();

        private Object _ToLock = new Object();

        private TxDbContext _TContext;
        private List<SxClientes> _SCooperados;
        private string _LastSchema;
        private List<SxContas> _Contas;

        public DataCache(TxDbContext sctx)
        {
            _TContext = sctx;
            try
            {
                PrepareCache();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao preparar cache de dados: " + ex.Message, ex);
            }
        }

        private void PrepareCache()
        {
            _TContext.Seguradora.AsNoTracking().ToList().ForEach(a =>
            {
                Seguradoras.Add(a.Id.ToString("0000"), a);
            });
            _TContext.Agencia.AsNoTracking().ToList().ForEach(a =>
            {
                Agencias.Add(a.Codigo, a);
            });
            _TContext.PontoAtendimento.Include(p => p.Agencia).ToList().ForEach(p =>
            {
                PontosAtendimento.Add((p.Agencia.Codigo, p.Codigo), p);
            });
            _TContext.Cooperado.Include(c => c.CooperadosAgenciasContas).AsNoTracking().ToList().ForEach(c =>
            {
                Cooperados.Add(c.NumeroDocumento, c);
            });
            _TContext.AgenciaSeguradora.AsNoTracking().ToList().ForEach(a =>
            {
                AgenciasSeguradoras.Add((a.AgenciaId, a.SeguradoraId), a);
            });
        }

        public Agencia GetAgencia(SxDbContext pSContext, string pCodigo)
        {
            ArgumentNullException.ThrowIfNull(pCodigo, nameof(pCodigo));
            lock (_ToLock)
            {
                if (Agencias.ContainsKey(pCodigo))
                    return Agencias[pCodigo];

                var agencias = pSContext.Agencia.ToList();
                if (!agencias.Any())
                    throw new Exception($"Agência [{pCodigo}] não encontrada.");
                var ags = agencias.DistinctBy(a => a.Codigo);
                foreach (var agencia in ags)
                {
                    var ag = new Agencia
                    {
                        Codigo = agencia.Codigo,
                        Nome = agencia.Nome
                    };
                    _TContext.Agencia.Add(ag);
                }
                _TContext.SaveChanges();
                _TContext.Agencia.ToList().ForEach(a => Agencias.Add(a.Codigo, a));
                return Agencias[pCodigo];
            }
        }

        public PontoAtendimento GetPontoAtendimento(SxDbContext pSContext, string pAgenciaCodigo, string pPontoCodigo)
        {
            ArgumentNullException.ThrowIfNull(pAgenciaCodigo, nameof(pAgenciaCodigo));
            ArgumentNullException.ThrowIfNull(pPontoCodigo, nameof(pPontoCodigo));
            lock (_ToLock)
            {
                if (PontosAtendimento.ContainsKey((pAgenciaCodigo, pPontoCodigo)))
                    return PontosAtendimento[(pAgenciaCodigo, pPontoCodigo)];
                var agencia = GetAgencia(pSContext, pAgenciaCodigo);

                var pontos = pSContext.PontoAtendimento.Where(p => p.Agencia == agencia.Codigo).OrderBy(p => p.Codigo).ToList();
                if (!pontos.Any())
                    throw new Exception($"Ponto de Atendimento [{pPontoCodigo}] da Agência [{pAgenciaCodigo}] não encontrado.");
                foreach (var ponto in pontos)
                {
                    var pa = new PontoAtendimento
                    {
                        AgenciaId = agencia.Id,
                        Codigo = ponto.Codigo,
                        Nome = ponto.Codigo + " - " + ponto.Nome
                    };
                    if (!_TContext.PontoAtendimento.Any(p => p.AgenciaId == pa.AgenciaId && p.Codigo == pa.Codigo))
                        _TContext.PontoAtendimento.Add(pa);
                }
                _TContext.SaveChanges();
                _TContext.PontoAtendimento.Include(p => p.Agencia).ToList().ForEach(p =>
                {
                    if (!PontosAtendimento.ContainsKey((p.Agencia.Codigo, p.Codigo)))
                        PontosAtendimento.Add((p.Agencia.Codigo, p.Codigo), p);
                });
                return PontosAtendimento[(pAgenciaCodigo, pPontoCodigo)];
            }
        }

        public (CooperadoAgenciaConta, Cooperado, SxContas) GetCooperado(SxDbContext pSContext, string pNumeroDocumento, string pConta, string pAgencia)
        {
            ArgumentNullException.ThrowIfNull(pNumeroDocumento, nameof(pNumeroDocumento));
            ArgumentNullException.ThrowIfNull(pConta, nameof(pConta));
            ArgumentNullException.ThrowIfNull(pAgencia, nameof(pAgencia));
            lock (_ToLock)
            {
                if (SxDbContext.Schema != _LastSchema)
                {
                    _LastSchema = SxDbContext.Schema;
                    _Contas = pSContext.Contas.ToList();
                }
                var conta = _Contas.Where(c => c.Codigo == pConta).FirstOrDefault();
                if (conta == null)
                    throw new Exception($"Conta [{pConta}] do Cooperado [{pNumeroDocumento}] não encontrada.");

                var agencia = GetAgencia(pSContext, pAgencia);
                Cooperado coop = null;
                if (Cooperados.ContainsKey(pNumeroDocumento))
                {
                    coop = Cooperados[pNumeroDocumento];
                    var tcac = coop.CooperadosAgenciasContas.FirstOrDefault(c => c.CooperadoId == coop.Id && c.AgenciaId == agencia.Id && c.ContaCorrente == conta.Codigo);
                    if (tcac != null)
                        return (tcac, coop, conta);
                }

                if (coop == null)
                {
                    if (_SCooperados == null)
                        _SCooperados = pSContext.Cooperados.ToList();
                    var cooperado = _SCooperados.Where(c => c.NumeroDocumento == pNumeroDocumento).FirstOrDefault();
                    if (cooperado == null)
                        throw new Exception($"Cooperado [{pNumeroDocumento}] não encontrado.");

                    coop = new Cooperado
                    {
                        NumeroDocumento = cooperado.NumeroDocumento,
                        Nome = cooperado.Nome,
                        Tipo = cooperado.Tipo == "F" ? TipoPessoaCooperado.Fisica : TipoPessoaCooperado.Juridica,
                        NomeFantasia = cooperado.NomeFantasia,
                        Email = cooperado.Email,
                    };
                    _TContext.Cooperado.Add(coop);
                    _TContext.SaveChanges();
                }
                var cac = new CooperadoAgenciaConta
                {
                    AgenciaId = agencia.Id,
                    CooperadoId = coop.Id,
                    ContaCorrente = conta.Codigo
                };
                _TContext.CooperadoAgenciaConta.Add(cac);
                _TContext.SaveChanges();
                Cooperados.Clear();
                _TContext.Cooperado.Include(c => c.CooperadosAgenciasContas).ToList().ForEach(c => Cooperados.Add(c.NumeroDocumento, c));

                coop = Cooperados[pNumeroDocumento];
                cac = coop.CooperadosAgenciasContas.FirstOrDefault(c => c.CooperadoId == coop.Id && c.AgenciaId == agencia.Id && c.ContaCorrente == conta.Codigo);

                return (cac, coop, conta);
            }
        }

        //public ulong GetContaCorrente(ulong pCooperadoId, ulong pAgenciaId, string pConta)
        //{
        //    ArgumentNullException.ThrowIfNull(pConta, nameof(pConta));
        //    ArgumentOutOfRangeException.ThrowIfZero(pAgenciaId, nameof(pAgenciaId));
        //    ArgumentOutOfRangeException.ThrowIfZero(pCooperadoId, nameof(pCooperadoId));

        //    var coop = Cooperados.Values.FirstOrDefault(c => c.Id == pCooperadoId);
        //    if (coop == null)
        //        throw new Exception($"Cooperado Id [{pCooperadoId}] não encontrado no cache.");
        //    var cac = coop.CooperadoAgenciaContas.FirstOrDefault(c => c.AgenciaId == pAgenciaId && c.ContaCorrente == pConta);
        //    if (cac == null)
        //    {
        //        cac = new CooperadoAgenciaConta
        //        {
        //            AgenciaId = pAgenciaId,
        //            ContaCorrente = pConta
        //        };
        //        coop.CooperadoAgenciaContas.Add(cac);
        //        _TContext.CooperadoAgenciaConta.Add(cac);
        //        _TContext.SaveChanges();
        //    }
        //    return cac.Id;
        //}

        public Seguradora GetSeguradora(SxDbContext pSContext, string pCodigo)
        {
            ArgumentNullException.ThrowIfNull(pCodigo, nameof(pCodigo));
            lock (_ToLock)
            {
                if (Seguradoras.ContainsKey(pCodigo))
                    return Seguradoras[pCodigo];
                var seguradoraSrc = pSContext.Seguradoras.Where(s => s.Codigo == pCodigo).FirstOrDefault();
                if (seguradoraSrc == null)
                    throw new Exception($"Seguradora [{pCodigo}] não encontrada.");
                var seguradora = CriarSeguradora(
                    seguradoraSrc.Nome,
                    StatusSeguradora.Ativo,
                    (seguradoraSrc.Codigo + seguradoraSrc.Cnpj).Substring(0, 14), 120, 75, 0.1m, 0.1m, 0.1m, true);

                _TContext.Seguradora.Add(seguradora);
                _TContext.SaveChanges();
                Seguradoras.Add(pCodigo, seguradora);
                return seguradora;
            }
        }

        private static Seguradora CriarSeguradora(string nome, StatusSeguradora status, string cnpj, ushort maxMesesContrato, ushort maxIdade, decimal coberturaMorte, decimal coberturaInvalidez, decimal coberturaPerdaRenda, bool periodicidade30Dias)
        {
            var seguradora = new Seguradora
            {
                Nome = nome,
                Status = status,
                Cnpj = cnpj,
                RazaoSocial = nome
            };

            seguradora.CondicoesSeguradora.Add(new CondicaoSeguradora
            {
                MaxMesesContrato = maxMesesContrato,
                MaxIdade = maxIdade,
                PorcentagemCoberturaMorte = coberturaMorte,
                PorcentagemCoberturaInvalidez = coberturaInvalidez,
                PorcentagemCoberturaPerdaRenda = coberturaPerdaRenda,
                Periodicidade30Dias = periodicidade30Dias
            });

            seguradora.ContabilizacoesSeguradoras.Add(new ContabilizacaoSeguradora
            {
                CreditoPremioContratacao = string.Empty,
                DescricaoCreditoPremioContratacao = string.Empty,
                DebitoPremioContratacao = string.Empty,
                DescricaoDebitoPremioContratacao = string.Empty,
                CreditoComissaoContratacao = string.Empty,
                DescricaoCreditoComissaoContratacao = string.Empty,
                DebitoComissaoContratacao = string.Empty,
                DescricaoDebitoComissaoContratacao = string.Empty,
                CreditoCancelamentoComissaoParcTot = string.Empty,
                DescricaoCreditoCancelamentoComissaoParcTot = string.Empty,
                DebitoCancelamentoComissaoParcTot = string.Empty,
                DescricaoDebitoCancelamentoComissaoParcTot = string.Empty,
                CreditoCancelamentoComissaoAVista = string.Empty,
                DescricaoCreditoCancelamentoComissaoAVista = string.Empty,
                DebitoCancelamentoComissaoAVista = string.Empty,
                DescricaoDebitoCancelamentoComissaoAVista = string.Empty,
                CreditoValorPago = string.Empty,
                DescricaoCreditoValorPago = string.Empty,
                DebitoValorPago = string.Empty,
                DescricaoDebitoValorPago = string.Empty,
                CreditoComissaoValorPago = string.Empty,
                DescricaoComissaoCreditoValorPago = string.Empty,
                DebitoComissaoValorPago = string.Empty,
                DescricaoComissaoDebitoValorPago = string.Empty
            });

            seguradora.SeguradorasLimites.Add(new SeguradoraLimite
            {
                IdadeInicial = 18,
                IdadeFinal = 90,
                Coeficiente = 0.0005m,
                ValorMaximo = 1000000m, // 1 Milhão
                DescricaoRegra = "Regra Padrão Teste"
            });

            return seguradora;
        }

        public ulong GetAgenciaSeguradora(ulong pAgenciaId, ulong pSeguradoraId)
        {
            ArgumentOutOfRangeException.ThrowIfZero(pAgenciaId, nameof(pAgenciaId));
            lock (_ToLock)
            {
                if (AgenciasSeguradoras.ContainsKey((pAgenciaId, pSeguradoraId)))
                    return AgenciasSeguradoras[(pAgenciaId, pSeguradoraId)].Id;

                var ag = new AgenciaSeguradora
                {
                    AgenciaId = pAgenciaId,
                    SeguradoraId = pSeguradoraId,
                };

                var ags = new ApoliceGrupoSeguradora
                {
                    Apolice = "APO-001",
                    Grupo = "GRP-001",
                    SubGrupo = "SUB-001",
                    TipoCapital = TipoCapitalApoliceGrupoSeguradora.Fixo,
                    ModalidadeUnico = "Unico",
                    ModalidadeAVista = 1.5m,
                    ModalidadeParcelado = 10m,
                };
                ag.ApolicesGruposSeguradoras.Add(ags);
                _TContext.AgenciaSeguradora.Add(ag);
                _TContext.SaveChanges();

                AgenciasSeguradoras.Add((pAgenciaId, pSeguradoraId), ag);
                return ags.Id;
            }
        }
    }
}
