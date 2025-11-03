namespace MigracaoTabelas.Target;

public class CooperadoAgenciaConta
{
    public ulong Id { get; set; }
    public ulong CooperadoId { get; set; }
    public ulong AgenciaId { get; set; }
    public string ContaCorrente { get; set; } = null!;

    public virtual Cooperado Cooperado { get; set; } = null!;
    public virtual Agencia Agencia { get; set; } = null!;
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}