using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class Cooperado
{
    public ulong Id { get; set; }
    public string NumeroDocumento { get; set; }
    public TipoPessoaCooperado Tipo { get; set; }
    public string Nome { get; set; }
    public string NomeFantasia { get; set; }
    public string Email { get; set; }

    public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
    public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
}

public enum TipoPessoaCooperado
{
    [Description("Física")]
    Fisica = 1,

    [Description("Jurídica")]
    Juridica = 2
}