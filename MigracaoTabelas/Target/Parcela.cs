using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Parcela
{

    public void Assign(SxEpSegParcela source)
    {
        if (source.SegCancelado.HasValue)
            Status = StatusParcela.Pago;
        else
            Status = StatusParcela.Pendente;

        NumeroParcela = (ushort)source.SegParcela;
        ValorParcela = source.SegValor;
        ValorOriginal = source.SegValor;
        Vencimento = source.SegVcto;
        Liquidacao = source.SegPgto;
        DataUltimoPagamento = source.SegPgto;

        if (source.SegPgto.HasValue)
            ValorPago = source.SegValor;
        else
            ValorPago = 0.00m;
    }

    public ulong Id { get; set; }
    public ulong SeguroId { get; set; }
    public StatusParcela Status { get; set; }
    public ushort NumeroParcela { get; set; }
    public decimal ValorParcela { get; set; }
    public decimal ValorOriginal { get; set; }
    public decimal ValorPago { get; set; }
    [NotMapped]
    public decimal ValorAPagar => ValorParcela - ValorPago;
    public DateTime Vencimento { get; set; }
    public DateTime? Liquidacao { get; set; }
    public DateTime? DataUltimoPagamento { get; set; }

    public virtual Seguro Seguros { get; set; } = null!;

    public void PagarManualmente(decimal valor)
    {
        ValidarPagamentoManual(valor);
        ValorPago += valor;
        var agora = DateTime.Now;
        DataUltimoPagamento = agora;
        if (ValorAPagar == 0)
        {
            Status = StatusParcela.Pago;
            Liquidacao = agora;
        }

    }

    private void ValidarPagamentoManual(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor do pagamento deve ser maior que zero.", nameof(valor));
        if (valor > ValorAPagar)
            throw new ApplicationException($"Valor do pagamento excede o valor a pagar. Valor a pagar {ValorAPagar}, ValorPagamento = {valor}");
        if (Status != StatusParcela.Pendente && Status != StatusParcela.Cancelada)
            throw new ApplicationException(
                $"Não é permitido pagamento manual para parcela com status diferente de 'Pendente' ou 'Cancelado. Parcela.Status = {Status}");
        if (Status == StatusParcela.Cancelada) //TODO: Existem casos em que parcela cancelada pode ser paga?
        {
            //var canceladaPagavel =
            //    (Seguros.Status == StatusSeguro.ExpiracaoDaVigenciaDoSeguro) ||
            //    (Seguros.SegurosCancelamentos.FirstOrDefault(sc => sc.SeguroId == SeguroId)?.Motivo ==
            //     MotivoSeguroCancelamento.CanceladoPelaCooperativa);
            //if (!canceladaPagavel)
            throw new ApplicationException($"Não é permitido pagamento manual para parcelas canceladas com motivo {Seguros.SegurosCancelamentos.Where(sc => sc.SeguroId == SeguroId).FirstOrDefault().Motivo}");
        }
    }
}

public enum StatusParcela
{
    [Description("Em Aberto")]
    Pendente = 1,
    [Description("Pago")]
    Pago = 2,
    [Description("Cancelada")]
    Cancelada = 3
}