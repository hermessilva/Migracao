namespace MigracaoTabelas.Target;

public class CooperadoAgenciaConta
{
    public ulong Id { get; set; }
    public ulong CooperadoId { get; set; }
    public ulong AgenciaId { get; set; }
    public string ContaCorrente { get; set; }

    public virtual Cooperado Cooperados { get; set; }
    public virtual Agencia Agencias { get; set; }
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}