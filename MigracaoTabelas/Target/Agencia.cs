namespace MigracaoTabelas.Target;

public class Agencia
{
    public ulong Id { get; set; }
    public string Codigo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public DateTime CriadoEm { get; set; }

    public virtual ICollection<PontoAtendimento> PontosAtendimento { get; set; } = new List<PontoAtendimento>();
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<ApoliceGrupoSeguradora> ApolicesGrupoSeguradora { get; set; } = new List<ApoliceGrupoSeguradora>();
    public virtual ICollection<CooperadoAgenciaConta> CooperadoAgenciaContas { get; set; } = new List<CooperadoAgenciaConta>();
    public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
    public virtual ICollection<IntegracaoSenior> IntegracoesSenior { get; set; } = new List<IntegracaoSenior>();
}