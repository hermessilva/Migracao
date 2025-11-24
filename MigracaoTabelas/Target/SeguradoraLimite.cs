namespace MigracaoTabelas.Target;

public class SeguradoraLimite
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public ushort IdadeInicial { get; set; }
    public ushort IdadeFinal { get; set; }
    public decimal ValorMaximo { get; set; }
    public decimal Coeficiente { get; set; }
    public decimal LimiteDps { get; set; }
    public string DescricaoRegra { get; set; }

    public virtual Seguradora Seguradoras { get; set; }
}