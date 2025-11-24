namespace MigracaoTabelas.Target;

public class ComissaoSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public decimal PorcentagemComissaoCorretora { get; set; }
    public decimal PorcentagemComissaoCooperativa { get; set; }

    public virtual Seguradora Seguradoras { get; set; }
}