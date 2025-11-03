namespace MigracaoTabelas.Target;

public class LancamentoEfetivar
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong CooperadoId { get; set; }
    public string ContaCorrente { get; set; } = null!;
    public DateTime? DataMovimentacao { get; set; }
    public string Descricao { get; set; } = null!;
    public decimal Valor { get; set; }
    public DateTime? DataLancamento { get; set; }

    public virtual Agencia Agencia { get; set; } = null!;
    public virtual Cooperado Cooperado { get; set; } = null!;
}