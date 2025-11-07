namespace MigracaoTabelas.Target;

public class Cooperado
{
    public ulong Id { get; set; }
    public string NumeroDocumento { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public string Nome { get; set; } = null!;
    public string NomeFantasia { get; set; }
    public string Email { get; set; }

    public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
    public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
}