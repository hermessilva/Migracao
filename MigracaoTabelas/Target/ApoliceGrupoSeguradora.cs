namespace MigracaoTabelas.Target;

public class ApoliceGrupoSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public string Apolice { get; set; } = null!;
    public string Grupo { get; set; } = null!;
    public string SubGrupo { get; set; } = null!;
    public string TipoCapital { get; set; } = null!;
    public string ModalidadeUnico { get; set; }
    public decimal? ModalidadeAvista { get; set; }
    public decimal? ModalidadeParcelado { get; set; }
    public byte? Ordem { get; set; }

    public virtual Agencia Agencia { get; set; } = null!;
    public virtual Seguradora Seguradora { get; set; } = null!;
}