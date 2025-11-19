using System.ComponentModel;

namespace MigracaoTabelas.Enums
{
    public enum StatusSeguro  // 12
    {
        /// <summary>
        /// Pode
        /// </summary>
        [Description("Em análise pela Seguradora")]
        EmAnalisePelaSeguradora,

        /// <summary>
        /// Pode
        /// </summary>
        [Description("Pendente de Documentação")]
        PendenteDeDocumentacao,

        /// <summary>
        /// Pode
        /// </summary>
        [Description("Ativo")]
        Ativo,

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
}
        