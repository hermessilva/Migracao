namespace MigracaoTabelas.Target;

public class ContabilizacaoSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public ulong ContaContabilId { get; set; }
    public string Tipo { get; set; } = null!;

    public virtual Seguradora Seguradora { get; set; } = null!;
    public virtual ContaContabil ContaContabil { get; set; } = null!;
}