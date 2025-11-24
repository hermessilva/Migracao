namespace MigracaoTabelas.Target;

public class PontoAtendimento
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Agencia Agencia { get; set; }
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
}