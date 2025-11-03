namespace MigracaoTabelas.Target;

public class CondicaoSeguradoraLimite
{
    public ulong Id { get; set; }
    public ulong CondicaoSeguradoraId { get; set; }
    public ushort IdadeInicial { get; set; }
    public ushort IdadeFinal { get; set; }
    public decimal ValorMaximo { get; set; }
    public decimal Coeficiente { get; set; }
    public decimal LimiteDps { get; set; }
    public string DescricaoRegra { get; set; } = null!;

    public virtual CondicaoSeguradora CondicaoSeguradora { get; set; } = null!;
}