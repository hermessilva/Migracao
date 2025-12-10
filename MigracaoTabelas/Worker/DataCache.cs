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
                return null;
                //PontosAtendimento[(pAgenciaCodigo, pPontoCodigo)];
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
                var seguradora = CriarSeguradora(seguradoraSrc);

                _TContext.Seguradora.Add(seguradora);
                _TContext.SaveChanges();
                Seguradoras.Add(pCodigo, seguradora);
                return seguradora;
            }
        }

        private static Seguradora CriarSeguradora(SxSeguradoras src)
        {
            // Gera CNPJ fictício baseado no código (14 dígitos)
            var cnpj = (src.Codigo.PadRight(14, '0')).Substring(0, 14);

            var seguradora = new Seguradora
            {
                Nome = src.Nome,
                Status = src.Ativo ? StatusSeguradora.Ativo : StatusSeguradora.Inativo,
                Cnpj = cnpj,
                RazaoSocial = src.Nome
            };

            // ComissaoSeguradora - utiliza PorcentagemComissao da fonte
            seguradora.ComissoesSeguradoras.Add(new ComissaoSeguradora
            {
                PorcentagemComissaoCorretora = 0.15m,
                PorcentagemComissaoCooperativa = src.PorcentagemComissao,
            });

            // CondicaoSeguradora - utiliza MaxMeses e LimiteIdade da fonte
            seguradora.CondicoesSeguradora.Add(new CondicaoSeguradora
            {
                MaxMesesContrato = (ushort)src.MaxMeses,
                MaxIdade = (ushort)src.LimiteIdade,
                PorcentagemCoberturaMorte = 0.1m,
                PorcentagemCoberturaInvalidez = 0.1m,
                PorcentagemCoberturaPerdaRenda = 0.1m,
                Periodicidade30Dias = true
            });

            // ContabilizacaoSeguradora - utiliza as contas contábeis da fonte
            seguradora.ContabilizacoesSeguradoras.Add(new ContabilizacaoSeguradora
            {
                // Contas de Prêmio na Contratação
                CreditoPremioContratacao = src.ContaContabilCredito ?? string.Empty,
                DescricaoCreditoPremioContratacao = "Conta Contábil de Crédito de SEGP contratado",
                DebitoPremioContratacao = src.ContaContabilDebito ?? string.Empty,
                DescricaoDebitoPremioContratacao = "Conta Contábil de Débito de SEGP contratado",

                // Contas de Comissão na Contratação
                CreditoComissaoContratacao = src.ContaContabilCreditoComissao ?? string.Empty,
                DescricaoCreditoComissaoContratacao = "Conta Contábil de Crédito de SEGP COMISSÃO contratado",
                DebitoComissaoContratacao = src.ContaContabilDebitoComissao ?? string.Empty,
                DescricaoDebitoComissaoContratacao = "Conta Contábil de Débito de SEGP COMISSÃO contratado",

                // Contas de Cancelamento Parcial/Total (4966)
                CreditoCancelamentoComissaoParcTot = src.ContaContabilCredito4966 ?? string.Empty,
                DescricaoCreditoCancelamentoComissaoParcTot = "Conta Contábil de Crédito de SEGP contratado 4966",
                DebitoCancelamentoComissaoParcTot = src.ContaContabilDebito4966 ?? string.Empty,
                DescricaoDebitoCancelamentoComissaoParcTot = "Conta Contábil de Débito de SEGP contratado 4966",

                // Contas de Cancelamento À Vista (Comissão 4966)
                CreditoCancelamentoComissaoAVista = src.ContaContabilCreditoComissao4966 ?? string.Empty,
                DescricaoCreditoCancelamentoComissaoAVista = "Conta Contábil de Crédito de SEGP COMISSÃO contratado 4966",
                DebitoCancelamentoComissaoAVista = src.ContaContabilDebitoComissao4966 ?? string.Empty,
                DescricaoDebitoCancelamentoComissaoAVista = "Conta Contábil de Débito de SEGP COMISSÃO contratado 4966",

                // Contas de Valor Pago
                CreditoValorPago = src.ContaContabilCredito ?? string.Empty,
                DescricaoCreditoValorPago = "Conta Contábil de Crédito - Valor Pago",
                DebitoValorPago = src.ContaContabilDebito ?? string.Empty,
                DescricaoDebitoValorPago = "Conta Contábil de Débito - Valor Pago",

                // Contas de Comissão Valor Pago
                CreditoComissaoValorPago = src.ContaContabilCreditoComissao ?? string.Empty,
                DescricaoComissaoCreditoValorPago = "Conta Contábil de Crédito - Comissão Valor Pago",
                DebitoComissaoValorPago = src.ContaContabilDebitoComissao ?? string.Empty,
                DescricaoComissaoDebitoValorPago = "Conta Contábil de Débito - Comissão Valor Pago",

                DebitoPremioParcela = string.Empty,
                DescricaoDebitoPremioParcela = "Conta Contábil de Débito - Prêmio Parcela",
                CreditoPremioParcela = string.Empty,
                DescricaoCreditoPremioParcela = "Conta Contábil de Crédito - Prêmio Parcela",
                DebitoComissaoParcela =  string.Empty,
                DescricaoDebitoComissaoParcela = "Conta Contábil de Débito - Comissão Parcela",
                CreditoComissaoParcela = string.Empty,
                DescricaoCreditoComissaoParcela = "Conta Contábil de Crédito - Comissão Parcela"

            });

            // SeguradoraLimite - cria um registro para cada faixa etária com valores da fonte
            // Faixa 18-30 anos
            if (src.Limite30 > 0 || src.Coef30 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 18,
                    IdadeFinal = 30,
                    Coeficiente = src.Coef30,
                    ValorMaximo = src.Limite30,
                    LimiteDps = src.Dps30 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa até 30 anos"
                });
            }

            // Faixa 31-35 anos
            if (src.Limite35 > 0 || src.Coef35 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 31,
                    IdadeFinal = 35,
                    Coeficiente = src.Coef35,
                    ValorMaximo = src.Limite35,
                    LimiteDps = src.Dps35 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 31 a 35 anos"
                });
            }

            // Faixa 36-40 anos
            if (src.Limite40 > 0 || src.Coef40 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 36,
                    IdadeFinal = 40,
                    Coeficiente = src.Coef40,
                    ValorMaximo = src.Limite40,
                    LimiteDps = src.Dps40 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 36 a 40 anos"
                });
            }

            // Faixa 41-45 anos
            if (src.Limite45 > 0 || src.Coef45 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 41,
                    IdadeFinal = 45,
                    Coeficiente = src.Coef45,
                    ValorMaximo = src.Limite45,
                    LimiteDps = src.Dps45 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 41 a 45 anos"
                });
            }

            // Faixa 46-50 anos
            if (src.Limite50 > 0 || src.Coef50 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 46,
                    IdadeFinal = 50,
                    Coeficiente = src.Coef50,
                    ValorMaximo = src.Limite50,
                    LimiteDps = src.Dps50 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 46 a 50 anos"
                });
            }

            // Faixa 51-55 anos
            if (src.Limite55 > 0 || src.Coef55 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 51,
                    IdadeFinal = 55,
                    Coeficiente = src.Coef55,
                    ValorMaximo = src.Limite55,
                    LimiteDps = src.Dps55 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 51 a 55 anos"
                });
            }

            // Faixa 56-60 anos
            if (src.Limite60 > 0 || src.Coef60 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 56,
                    IdadeFinal = 60,
                    Coeficiente = src.Coef60,
                    ValorMaximo = src.Limite60,
                    LimiteDps = src.Dps60 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 56 a 60 anos"
                });
            }

            // Faixa 61-65 anos
            if (src.Limite65 > 0 || src.Coef65 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 61,
                    IdadeFinal = 65,
                    Coeficiente = src.Coef65,
                    ValorMaximo = src.Limite65,
                    LimiteDps = src.Dps65 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 61 a 65 anos"
                });
            }

            // Faixa 66-70 anos
            if (src.Limite70 > 0 || src.Coef70 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 66,
                    IdadeFinal = 70,
                    Coeficiente = src.Coef70,
                    ValorMaximo = src.Limite70,
                    LimiteDps = src.Dps70 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 66 a 70 anos"
                });
            }

            // Faixa 71-75 anos
            if (src.Limite75 > 0 || src.Coef75 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 71,
                    IdadeFinal = 75,
                    Coeficiente = src.Coef75,
                    ValorMaximo = src.Limite75,
                    LimiteDps = src.Dps75 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 71 a 75 anos"
                });
            }

            // Faixa 76-80 anos
            if (src.Limite80 > 0 || src.Coef80 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 76,
                    IdadeFinal = 80,
                    Coeficiente = src.Coef80,
                    ValorMaximo = src.Limite80,
                    LimiteDps = src.Dps80 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 76 a 80 anos"
                });
            }

            // Faixa 81-85 anos
            if (src.Limite85 > 0 || src.Coef85 > 0)
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 81,
                    IdadeFinal = 85,
                    Coeficiente = src.Coef85,
                    ValorMaximo = src.Limite85,
                    LimiteDps = src.Dps85 ? src.ValorDps : 0,
                    DescricaoRegra = "Faixa 81 a 85 anos"
                });
            }

            // Se nenhuma faixa foi definida, cria uma faixa padrão
            if (!seguradora.SeguradorasLimites.Any())
            {
                seguradora.SeguradorasLimites.Add(new SeguradoraLimite
                {
                    IdadeInicial = 18,
                    IdadeFinal = (ushort)src.LimiteIdade,
                    Coeficiente = 0.0005m,
                    ValorMaximo = 1000000m,
                    LimiteDps = src.ValorDps,
                    DescricaoRegra = "Faixa Padrão"
                });
            }

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
