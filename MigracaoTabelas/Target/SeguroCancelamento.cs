using System.ComponentModel;

namespace MigracaoTabelas.Target
{
    public class SeguroCancelamento
    {
        public ulong Id { get; set; }
        public ulong SeguroId { get; set; }
        public DateOnly Data { get; set; }
        public DateTime CriadoEm { get; set; }
        public MotivoSeguroCancelamento Motivo { get; set; }
        public decimal ValorRestituir { get; set; }
        public decimal ValorComissao { get; set; }
        public int DiasUtilizados { get; set; }
        public virtual Seguro Seguros { get; set; }
    }

    public enum MotivoSeguroCancelamento
    {

        [Description("Expiração da Vigência do Seguro")]
        ExpiracaoDaVigenciaDoSeguro = 1,

        [Description("Cancelado pelo Cooperado")]
        CanceladoPeloCooperado = 2,

        [Description("Cancelado pela Cooperativa")]
        CanceladoPelaCooperativa = 3,

        [Description("Sinistro")]
        Sinistro = 4,

        [Description("Recusado pela Seguradora")]
        RecusadoPelaSeguradora = 5,

        [Description("Cancelamento por Prejuízo")]
        CancelamentoPorPrejuizo = 6,

        [Description("Liquidação Antecipada")]
        LiquidacaoAntecipada = 7,

        [Description("Cancelado por Renegociação")]
        CanceladoPorRenegociacao = 8,

        [Description("Cancelado por Aditivo")]
        CanceladoPorAditivo = 9
    }
}
