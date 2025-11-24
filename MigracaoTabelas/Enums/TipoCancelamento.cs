using System.ComponentModel;

namespace MigracaoTabelas.Enums;

public enum TipoCancelamento
{

    [Description("Expiração da Vigência do Seguro")]
    ExpiracaoDaVigenciaDoSeguro,

    [Description("Cancelado pelo Cooperado")]
    CanceladoPeloCooperado,

    [Description("Cancelado pela Cooperativa")]
    CanceladoPelaCooperativa,

    [Description("Sinistro")]
    Sinistro,

    [Description("Recusado pela Seguradora")]
    RecusadoPelaSeguradora,

    [Description("Cancelamento por Prejuízo")]
    CancelamentoPorPrejuizo,

    [Description("Liquidação Antecipada")]
    LiquidacaoAntecipada,

    [Description("Cancelado por Renegociação")]
    CanceladoPorRenegociacao,

    [Description("Cancelado por Aditivo")]
    CanceladoPorAditivo
}
