namespace MigracaoTabelas.Target;

public class IntegracaoSenior
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public bool ContaCredito { get; set; }
    public bool ContaDebito { get; set; }
    public string Status { get; set; } = null!;
    public DateTime DataMovimentacao { get; set; }
    public decimal Valor { get; set; }
    public uint Lancamento { get; set; }
    public string Descricao { get; set; } = null!;
    public string TipoLancamentoContabil { get; set; } = null!;
    public string CodigoPontoAtendimento { get; set; } = null!;

    public virtual Agencia Agencias { get; set; } = null!;
}