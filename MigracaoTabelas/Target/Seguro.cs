using System.ComponentModel;

using MigracaoTabelas.Source;


namespace MigracaoTabelas.Target;

public class Seguro
{
    private MotivoSeguro _motivo;
    public ulong Id { get; set; }
    public ulong CooperadoAgenciaContaId { get; set; }
    public ulong PontoAtendimentoId { get; set; }
    public ulong ApoliceGrupoSeguradoraId { get; set; }
    public ulong SeguroParametroId { get; set; }
    public ulong? UsuarioId { get; set; }
    public string ContratoSequencia { get; set; } = "00";
    public StatusSeguro Status { get; private set; }
    public MotivoSeguro Motivo
    {
        get
        {
            return _motivo;
        }
        set
        {
            var statusAnterior = Status;
            _motivo = value;
            
            switch (_motivo)
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
                    // Se o status anterior era Cancelado, mantém Cancelado
                    // Isso permite atualizar o motivo sem alterar o status
                    if (statusAnterior != StatusSeguro.Cancelado)
                    {
                        Status = StatusSeguro.Ativo;
                    }
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
                    throw new Exception($"Não foi implementado equivalência para o motivo informado {_motivo}.");
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
        Motivo = MotivoSeguro.Regular;
        Contrato = source.SegContrato ?? string.Empty;
        InicioVigencia = source.SegInicio;
        FimVigencia = source.SegFim;
        QuantidadeParcelas = (short)(source.SegMeses ?? 0);
        Vencimento = source.SegFim;
        CapitalSegurado = source.Saldo == 0 ? source.SegBase ?? 0 : source.Saldo;
        PremioTotal = source.SegPremio ?? 0.00m;
        ValorBase = source.SegBase;
        ValorIof = source.SegIof;
        ContratoSequencia = source.ContratoSequencia.ToString("00");
        TipoPagamento = source.TipoPagamento == 1 ? TipoPagamentoSeguro.Parcelado : TipoPagamentoSeguro.AVista;
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