namespace MigracaoTabelas.Target;

public class ContaCorrenteSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string ContaCorrentePrestamista { get; set; }
    public string DescricaoContaCorrentePrestamista { get; set; }
    public string ContaCancelamentoPrestamista { get; set; }
    public string DescricaoContaCancelamentoPrestamista { get; set; }
    public string ContaEstornoPrestamista { get; set; }
    public string DescricaoContaEstornoPrestamista { get; set; }

    public virtual Seguradora Seguradoras { get; set; }
}