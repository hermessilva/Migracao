namespace MigracaoTabelas.Target;

public class Agencia
{
    public ulong Id { get; set; }
    public string Codigo { get; set; }
    public string Nome { get; set; }
    public DateTime CriadoEm { get; set; }

    public virtual ICollection<PontoAtendimento> PontosAtendimentos { get; set; } = new List<PontoAtendimento>();
    public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
    public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
    public virtual ICollection<IntegracaoSenior> IntegracoesSeniores { get; set; } = new List<IntegracaoSenior>();
    public virtual ICollection<AgenciaSeguradora> AgenciasSeguradoras { get; set; } = new List<AgenciaSeguradora>();
}