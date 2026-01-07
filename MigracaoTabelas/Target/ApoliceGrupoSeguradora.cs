using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class ApoliceGrupoSeguradora
{
    public ulong Id { get; set; }
    
    // Campos migrados de AgenciaSeguradora
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public int Ordem { get; set; }
    
    // Campos originais
    public string Apolice { get; set; }
    public string Grupo { get; set; }
    public string SubGrupo { get; set; }
    public TipoCapitalApolice TipoCapital { get; set; } = TipoCapitalApolice.Fixo;
    public string ModalidadeUnico { get; set; }
    public decimal? ModalidadeAVista { get; set; }
    public decimal? ModalidadeParcelado { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
    public virtual Agencia Agencia { get; set; }
    public virtual Seguradora Seguradora { get; set; }
}

public enum TipoCapitalApolice
{
    [Description("Fixo")]
    Fixo = 1,
    [Description("Variável")]
    Variavel = 2
}