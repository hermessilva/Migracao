namespace MigracaoTabelas.Target;

public class AgenciaSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }

    public virtual Agencia Agencias { get; set; } = null!;
    public virtual Seguradora Seguradoras { get; set; } = null!;
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}