using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class SeguroHistoricoPagamento
{
    public ulong Id { get; set; }
    public ulong ParcelaId { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorPago { get; set; }
    public OperacaoSeguroHistoricoPagamento Operacao { get; set; }

    public virtual Parcela Parcela { get; set; } = null!;
}

public enum OperacaoSeguroHistoricoPagamento
{
    [Description("Recebimento")]
    Recebimento = 1,
    [Description("Estorno")]
    Estorno = 2
}
