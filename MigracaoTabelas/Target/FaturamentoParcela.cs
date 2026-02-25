using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class FaturamentoParcela
{
    public ulong Id { get; set; }
    public ulong ParcelaId { get; set; }
    public ulong UsuarioId { get; set; }
    public string Lote { get; set; } = string.Empty;
    public TipoFaturamentoParcelaStatus Status { get; set; }
    public TipoFaturamentoParcelaOrigem Origem { get; set; }
    public DateTime DataPagamento { get; set; }
    public DateTime CriadoEm { get; set; }
    public virtual Parcela Parcela { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
}

public enum TipoFaturamentoParcelaStatus
{
    [Description("Faturado")]
    Faturado = 1,

    [Description("Cancelado")]
    Cancelado = 2,

    [Description("Não Faturado")]
    NaoFaturado = 3,

    [Description("EmProcessamento")]
    EmProcessamento = 4
}

public enum TipoFaturamentoParcelaOrigem
{
    [Description("Manual")]
    Manual = 1,

    [Description("Automatico")]
    Automatico = 2,
}
