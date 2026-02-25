#nullable enable
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
    public decimal ComissaoCorretora { get; set; }
    public decimal ComissaoCooperativa { get; set; }

    // Relacionamentos
    public virtual Seguro Seguros { get; set; } = null!;
    public virtual FaturamentoParcela? FaturamentoParcela { get; set; }

    public void PagarManualmente(decimal valor)
    {
        ValorPago += valor;
        var agora = DateTime.Now;
        DataUltimoPagamento = agora;
        if (ValorAPagar == 0)
        {
            Status = StatusParcela.Pago;
            Liquidacao = agora;
        }

    }

}

public enum StatusParcela
{
    [Description("Em Aberto")]
    EmAberto = 1,
    [Description("Pago")]
    Pago = 2,
    [Description("Cancelada")]
    Cancelada = 3,
    [Description("Pendente")]
    Pendente = 4
}