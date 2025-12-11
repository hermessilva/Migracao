using System.ComponentModel;

using MigracaoTabelas.Enums;
using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Seguro
{
    private MotivoSeguro _Motivo;
    public ulong Id { get; set; }
    public ulong CooperadoAgenciaContaId { get; set; }
    public ulong PontoAtendimentoId { get; set; }
    public ulong ApoliceGrupoSeguradoraId { get; set; }
    public ulong SeguroParametroId { get; set; }
    public ulong? UsuarioId { get; set; }
    public StatusSeguro Status { get; private set; }
    public MotivoSeguro Motivo
    {
        get
        {
            return _Motivo;
        }
        set
        {
            _Motivo = value;
            switch (_Motivo)
            {
                case MotivoSeguro.EmAnaliseNaSeguradora:
                case MotivoSeguro.AguardandoFaturamento:
                case MotivoSeguro.AguardandoDocumentacao:
                    Status = StatusSeguro.Pendente;
                    break;
                case MotivoSeguro.PagamentoAVista:
                case MotivoSeguro.PagamentoParcelado:
                case MotivoSeguro.Inadimplente:
                case MotivoSeguro.Regular:
                    Status = StatusSeguro.Ativo;
                    break;
                case MotivoSeguro.RecusadoPelaSeguradora:
                    Status = StatusSeguro.Recusado;
                    break;
                case MotivoSeguro.ExpiracaoVigenciaSeguro:
                    Status = StatusSeguro.Expirado;
                    break;
                case MotivoSeguro.Aditivo:
                case MotivoSeguro.CancelamentoPorPrejuizo:
                case MotivoSeguro.Renegociacao:
                case MotivoSeguro.Sinistro:
                case MotivoSeguro.SolicitadoPelaCooperativa:
                case MotivoSeguro.SolicitadoPeloCooperado:
                case MotivoSeguro.LiquidacaoAntecipada:
                    Status = StatusSeguro.Cancelado;
                    break;

                default:
                    throw new Exception($"Não foi implementado equivalência para o motivo informado {_Motivo}.");
            }
        }
    }

    public string Contrato { get; set; }
    public string NumeroContratoEmprestimo { get; set; }
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
    public int CodigoGrupo { get; set; }
    public short QuantidadeParcelas { get; set; }
    public DateTime? Vencimento { get; set; }
    public decimal CapitalSegurado { get; set; }
    public decimal PremioTotal { get; set; }
    public TipoPagamentoSeguro TipoPagamento { get; set; }
    public decimal EstornoProporcional { get; set; }
    public decimal? ValorBase { get; set; }
    public bool? DeclaracaoPessoalSaude { get; set; }
    public decimal? ValorIof { get; set; }

    public virtual CooperadoAgenciaConta CooperadosAgenciasContas { get; set; }
    public virtual ApoliceGrupoSeguradora ApolicesGruposSeguradoras { get; set; }
    public virtual PontoAtendimento PontosAtendimentos { get; set; }
    public virtual SeguroParametro SeguroParametro { get; set; }
    public virtual Usuario Usuarios { get; set; }
    public virtual ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
    public virtual ICollection<SeguroCancelamento> SegurosCancelamentos { get; set; } = new List<SeguroCancelamento>();

    public void AlterarMotivo(MotivoSeguro requestMotivo)
    {
        Motivo = requestMotivo;
    }

    public static bool PermiteAlteracaoManual(MotivoSeguro motivo)
    {
        return MotivoSeguro.EmAnaliseNaSeguradora == motivo ||
               MotivoSeguro.AguardandoFaturamento == motivo ||
               MotivoSeguro.RecusadoPelaSeguradora == motivo;
    }
    public static MotivoSeguro Convert(MotivoSeguroCancelamento motivoCancelamento)
    {
        switch (motivoCancelamento)
        {
            case MotivoSeguroCancelamento.Aditivo:
                return MotivoSeguro.Aditivo;
            case MotivoSeguroCancelamento.CancelamentoPorPrejuizo:
                return MotivoSeguro.CancelamentoPorPrejuizo;
            case MotivoSeguroCancelamento.Renegociacao:
                return MotivoSeguro.Renegociacao;
            case MotivoSeguroCancelamento.Sinistro:
                return MotivoSeguro.Sinistro;
            case MotivoSeguroCancelamento.SolicitadoPelaCooperativa:
                return MotivoSeguro.SolicitadoPelaCooperativa;
            case MotivoSeguroCancelamento.SolicitadoPeloCooperado:
                return MotivoSeguro.SolicitadoPeloCooperado;
            case MotivoSeguroCancelamento.LiquidacaoAntecipada:
                return MotivoSeguro.LiquidacaoAntecipada;
            default:
                throw new ArgumentOutOfRangeException(nameof(motivoCancelamento), $"MotivoSeguroCancelamento '{motivoCancelamento}' não pode ser convertido para MotivoSeguro.");
        }
    }

    public void Assign(SxEpSegPrestamista source)
    {
        // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

        // (SEG_CANCTIPO: Tipo de Cancelamento -> status: Identificador do status)
        // Regra aplicada: > 0 consideramos cancelado (3), senão aberto (1)

        Motivo = MotivoSeguro.Regular;

        // Mapeamentos com correspondência direta
        Contrato = source.SegContrato ?? string.Empty;                         // (SEG_CONTRATO: Contrato -> contrato: Número do contrato do seguro)
        InicioVigencia = source.SegInicio;                                     // (SEG_INICIO: Início do Contrato -> inicio_vigencia: Início de vigência do seguro)
        FimVigencia = source.SegFim;                                           // (SEG_FIM: Final do Contrato -> fim_vigencia: Fim de vigência do seguro)

        // Quantidade de parcelas: preferir valor informado (SegMeses); fallback para quantidade de parcelas de navegação
        QuantidadeParcelas = (short)(source.SegMeses ?? 0);                             // (SEG_MESES: Nº de Meses do Seguro -> quantidade_parcelas: Quantidade total de parcelas)


        // Vencimento: manter como fim de vigência, até haver regra específica
        Vencimento = source.SegFim;                                            // (SEG_FIM: Final do Contrato -> vencimento: Data de vencimento) [assumido]

        // Capital segurado: usar base segurada como principal, com fallback para valor do contrato
        CapitalSegurado = source.Saldo;

        // Prêmio total e tipo de pagamento
        PremioTotal = source.SegPremio ?? 0.00m;                               // (SEG_PREMIO: Valor do Seguro -> premio_total: Valor do prêmio total do seguro)
        ValorBase = source.SegBase;                                               // (SEG_BASE: Valor Base Segurado -> valor_base: Valor base do seguro)
        ValorIof = source.SegIof;                                                   // (SEG_IOF: Valor do IOF -> valor_iof: Valor do IOF do seguro)


        TipoPagamento = TipoPagamentoSeguro.Parcelado;


        /*
        EstornoProporcional =
        */
    }
}

public enum TipoPagamentoSeguro
{
    [Description("À Vista")]
    AVista = 1,
    [Description("Parcelado")]
    Parcelado = 2,
    [Description("Único")]
    Unico = 3
}

public enum TipoCapitalSeguro
{
    [Description("Fixo")]
    Fixo = 1,
    [Description("Variável")]
    Variavel = 2
}

public enum StatusSeguro
{
    [Description("Não Permitido")]
    NaoPermitido = 0,
    [Description("Pendente")]
    Pendente = 1,
    [Description("Ativo")]
    Ativo = 2,
    [Description("Recusado")]
    Recusado = 3,
    [Description("Expirado")]
    Expirado = 4,
    [Description("Cancelado")]
    Cancelado = 5,
}

public enum MotivoSeguro
{
    [Description("Não Permitido")]
    NaoPermitido = 0,
    [Description("Em analise na seguradora")]
    EmAnaliseNaSeguradora = 1,
    [Description("Aguardando faturamento")]
    AguardandoFaturamento = 2,
    [Description("Aguardando documentação")]
    AguardandoDocumentacao = 3,
    [Description("Pagamento à vista")]
    PagamentoAVista = 4,
    [Description("Pagamento parcelado")]
    PagamentoParcelado = 5,
    [Description("Inadimplente")]
    Inadimplente = 6,
    [Description("Regular")]
    Regular = 7,
    [Description("Recusado pela seguradora")]
    RecusadoPelaSeguradora = 8,
    [Description("Expiração da vigência do seguro")]
    ExpiracaoVigenciaSeguro = 9,

    [Description("Aditivo")]
    Aditivo = 10,
    [Description("Cancelamento por prejuízo")]
    CancelamentoPorPrejuizo = 11,
    [Description("Renegociação")]
    Renegociacao = 12,
    [Description("Sinistro")]
    Sinistro = 13,
    [Description("Solicitado pela cooperativa")]
    SolicitadoPelaCooperativa = 14,
    [Description("Solicitado pelo cooperado")]
    SolicitadoPeloCooperado = 15,
    [Description("Liquidação antecipada")]
    LiquidacaoAntecipada = 16,
}