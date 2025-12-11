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

        public static MotivoSeguroCancelamento ConvertMotivo(byte motivoERP)
        {
            switch (motivoERP)
            {
                case 1:
                    return MotivoSeguroCancelamento.SolicitadoPeloCooperado;
                case 2:
                    return MotivoSeguroCancelamento.SolicitadoPelaCooperativa;
                case 3:
                    return MotivoSeguroCancelamento.Sinistro;
                case 5:
                    return MotivoSeguroCancelamento.Aditivo;
                case 6:
                    return MotivoSeguroCancelamento.LiquidacaoAntecipada;
                default:
                    return MotivoSeguroCancelamento.NãoPermitido;
            }
        }
        public static byte ConvertMotivo(MotivoSeguroCancelamento motivo)
        {
            return motivo switch
            {
                MotivoSeguroCancelamento.SolicitadoPeloCooperado => 1,
                MotivoSeguroCancelamento.SolicitadoPelaCooperativa => 2,
                MotivoSeguroCancelamento.Sinistro => 3,
                MotivoSeguroCancelamento.Aditivo => 5,
                MotivoSeguroCancelamento.LiquidacaoAntecipada => 6,
                _ => 0
            };
        }


        public static MotivoSeguroCancelamento Convert(MotivoSeguro motivoSeguro)
        {
            switch (motivoSeguro)
            {
                case MotivoSeguro.Aditivo:
                    return MotivoSeguroCancelamento.Aditivo;
                case MotivoSeguro.CancelamentoPorPrejuizo:
                    return MotivoSeguroCancelamento.CancelamentoPorPrejuizo;
                case MotivoSeguro.Renegociacao:
                    return MotivoSeguroCancelamento.Renegociacao;
                case MotivoSeguro.Sinistro:
                    return MotivoSeguroCancelamento.Sinistro;
                case MotivoSeguro.SolicitadoPelaCooperativa:
                    return MotivoSeguroCancelamento.SolicitadoPelaCooperativa;
                case MotivoSeguro.SolicitadoPeloCooperado:
                    return MotivoSeguroCancelamento.SolicitadoPeloCooperado;
                case MotivoSeguro.LiquidacaoAntecipada:
                    return MotivoSeguroCancelamento.LiquidacaoAntecipada;
                default:
                    throw new ArgumentOutOfRangeException(nameof(motivoSeguro), $"MotivoSeguro '{motivoSeguro}' não pode ser convertido para MotivoSeguroCancelamento.");
            }
        }
    }

    public enum MotivoSeguroCancelamento
    {
        [Description("Não Permitido")]
        NãoPermitido = 0,
        [Description("Aditivo")]
        Aditivo = 1,
        [Description("Cancelamento por prejuízo")]
        CancelamentoPorPrejuizo = 2,
        [Description("Renegociaçao")]
        Renegociacao = 3,
        [Description("Sinistro")]
        Sinistro = 4,
        [Description("Solicitado pela cooperativa")]
        SolicitadoPelaCooperativa = 5,
        [Description("Solicitado pelo cooperado")]
        SolicitadoPeloCooperado = 6,
        [Description("Liquidação Antecipada")]
        LiquidacaoAntecipada = 7,
    }

    public enum MotivoCancelamentoERP
    {
        [Description("Não cancelado")]
        NaoCancelado = 0,

        [Description("Cancelamento solicitado pelo cooperado")]
        CancelamentoSolicitadoPeloCooperado = 1,

        [Description("Cancelamento solicitado pela agência")]
        CancelamentoSolicitadoPelaAgencia = 2,

        [Description("Sinistro")]
        Sinistro = 3,

        [Description("Rejeitado pela seguradora")]
        RejeitadoPelaSeguradora = 4,

        [Description("Rejeitado por aditivo")]
        RejeitadoPorAditivo = 5,

        [Description("Operação com liquidação antecipada")]
        OperacaoComLiquidacaoAntecipada = 6,

        [Description("")]
        Vazio7 = 7,

        [Description("")]
        Vazio8 = 8,

        [Description("Cancelados pelo sistema - (Contratos que não foram liberados na mesa)")]
        CanceladosPeloSistema = 9
    }

}
