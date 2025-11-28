using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class ApoliceGrupoSeguradora
{
    public ulong Id { get; set; }
    public ulong AgenciaSeguradoraId { get; set; }
    public string Apolice { get; set; }
    public string Grupo { get; set; }
    public string SubGrupo { get; set; }
    public TipoCapitalApoliceGrupoSeguradora TipoCapital { get; set; }
    public string ModalidadeUnico { get; set; }
    public decimal? ModalidadeAVista { get; set; }
    public decimal? ModalidadeParcelado { get; set; }
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();

    public virtual AgenciaSeguradora AgenciasSeguradoras { get; set; }
}

public enum TipoCapitalApoliceGrupoSeguradora
{
    [Description("Fixo")]
    Fixo,
    [Description("Variável")]
    Variavel
}