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
    public bool Periodicidade30Dias { get; set; } = false;
    public virtual Seguradora Seguradoras { get; set; }
}