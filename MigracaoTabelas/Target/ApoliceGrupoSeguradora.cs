namespace MigracaoTabelas.Target;

public class ApoliceGrupoSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public string Apolice { get; set; }
    public string Grupo { get; set; }
    public string SubGrupo { get; set; }
    public string TipoCapital { get; set; } = null!;
    public string ModalidadeUnico { get; set; }
    public decimal? ModalidadeAVista { get; set; }
    public decimal? ModalidadeParcelado { get; set; }

    public virtual Agencia Agencias { get; set; } = null!;
    public virtual Seguradora Seguradoras { get; set; } = null!;
}