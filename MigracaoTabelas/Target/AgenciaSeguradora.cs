namespace MigracaoTabelas.Target;

public class AgenciaSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public byte Ordem { get; set; }

    public virtual Agencia Agencias { get; set; }
    public virtual Seguradora Seguradoras { get; set; }
    public virtual ICollection<ApoliceGrupoSeguradora> ApolicesGruposSeguradoras { get; set; } = new List<ApoliceGrupoSeguradora>();
}