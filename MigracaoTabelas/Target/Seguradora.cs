namespace MigracaoTabelas.Target;

public class Seguradora
{
    public ulong Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string RazaoSocial { get; set; } = null!;
    public string Status { get; set; } = null!;

    public virtual ICollection<CondicaoSeguradora> CondicoesSeguradora { get; set; } = new List<CondicaoSeguradora>();
    public virtual ICollection<GestaoDocumento> GestaoDocumentos { get; set; } = new List<GestaoDocumento>();
    public virtual ICollection<ApoliceGrupoSeguradora> ApolicesGrupoSeguradora { get; set; } = new List<ApoliceGrupoSeguradora>();
    public virtual ICollection<ContabilizacaoSeguradora> ContabilizacaoSeguradoras { get; set; } = new List<ContabilizacaoSeguradora>();
}