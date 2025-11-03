namespace MigracaoTabelas.Target;

public class ContaContabil
{
    public ulong Id { get; set; }
    public string Descricao { get; set; } = null!;
    public string Conta { get; set; } = null!;

    public virtual ICollection<ContabilizacaoSeguradora> ContabilizacaoSeguradoras { get; set; } = new List<ContabilizacaoSeguradora>();
}