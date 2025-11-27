namespace MigracaoTabelas.Target;

public class Agencia
{
    public ulong Id { get; set; }
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public DateTime CriadoEm { get; set; }

    public virtual ICollection<PontoAtendimento> PontosAtendimentos { get; set; } = new List<PontoAtendimento>();
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
    public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
    public virtual ICollection<IntegracaoSenior> IntegracoesSeniores { get; set; } = new List<IntegracaoSenior>();
    public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
    public virtual ICollection<AgenciaSeguradora> AgenciasSeguradoras { get; set; } = new List<AgenciaSeguradora>();
}