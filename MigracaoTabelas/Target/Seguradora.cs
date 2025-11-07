namespace MigracaoTabelas.Target;

public class Seguradora
{
    public ulong Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Cnpj { get; set; } = null!;
    public string RazaoSocial { get; set; } = null!;
    public string Status { get; set; } = null!;

    public virtual ICollection<CondicaoSeguradora> CondicoesSeguradora { get; set; } = new List<CondicaoSeguradora>();
    public virtual ICollection<GestaoDocumento> GestoesDocumentos { get; set; } = new List<GestaoDocumento>();
    public virtual ICollection<ApoliceGrupoSeguradora> ApolicesGruposSeguradoras { get; set; } = new List<ApoliceGrupoSeguradora>();
    public virtual ICollection<ContabilizacaoSeguradora> ContabilizacoesSeguradoras { get; set; } = new List<ContabilizacaoSeguradora>();
    public virtual ICollection<ContaCorrenteSeguradora> ContasCorrentes { get; set; } = new List<ContaCorrenteSeguradora>();
    public virtual ICollection<PropostaSeguradora> PropostasSeguradoras { get; set; } = new List<PropostaSeguradora>();
    public virtual ICollection<ComissaoSeguradora> ComissoesSeguradoras { get; set; } = new List<ComissaoSeguradora>();
    public virtual ICollection<SeguradoraLimite> SeguradorasLimites { get; set; } = new List<SeguradoraLimite>();
    public virtual ICollection<Priorizacao> Priorizacoes { get; set; } = new List<Priorizacao>();
    public virtual ICollection<AgenciaSeguradora> AgenciasSeguradoras { get; set; } = new List<AgenciaSeguradora>();
}