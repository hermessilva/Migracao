using System.ComponentModel;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Parcela
{

    public void Assign(SxEpSegParcela source)
    {
        if (source.SegCancelado.HasValue)
            Status = StatusParcela.Pago;
        else if (!source.SegPgto.HasValue)
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
    public DateTime Vencimento { get; set; }
    public DateTime? Liquidacao { get; set; }
    public DateTime? DataUltimoPagamento { get; set; }

    public virtual Seguro Seguros { get; set; }
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