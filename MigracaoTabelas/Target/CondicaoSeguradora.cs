namespace MigracaoTabelas.Target;

public class CondicaoSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public ushort MaxMesesContrato { get; set; }
    public ushort MaxIdade { get; set; }
    public decimal PorcentagemCoberturaMorte { get; set; }
    public decimal PorcentagemCoberturaInvalidez { get; set; }
    public decimal PorcentagemCoberturaPerdaRenda { get; set; }
    public decimal PorcentagemComissaoCorretora { get; set; }
    public decimal PorcentagemComissaoCooperativa { get; set; }

    public virtual Seguradora Seguradora { get; set; } = null!;
}