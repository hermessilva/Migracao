namespace MigracaoTabelas.Target;

public class PontoAtendimento
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public DateTime? CriadoEm { get; set; }

    public virtual Agencia Agencia { get; set; } = null!;
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}