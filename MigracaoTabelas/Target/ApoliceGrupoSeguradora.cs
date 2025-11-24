using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class ApoliceGrupoSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public string Apolice { get; set; }
    public string Grupo { get; set; }
    public string SubGrupo { get; set; }
    public byte Ordem { get; set; }
    public TipoCapitalApoliceGrupoSeguradora TipoCapital { get; set; }
    public string ModalidadeUnico { get; set; }
    public decimal? ModalidadeAVista { get; set; }
    public decimal? ModalidadeParcelado { get; set; }

    public virtual Agencia Agencias { get; set; }
    public virtual Seguradora Seguradoras { get; set; }
}

public enum TipoCapitalApoliceGrupoSeguradora
{
    [Description("Fixo")]
    Fixo,
    [Description("Variável")]
    Variavel
}