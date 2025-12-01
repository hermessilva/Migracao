namespace MigracaoTabelas.Target;

public class SeguroParametro
{
    public ulong Id { get; set; }
    public TipoCapitalSeguro TipoCapital { get; set; }
    public bool Periodicidade30Dias { get; set; }
    public decimal Coeficiente { get; set; }
    public decimal Iof { get; set; }
    public decimal PorcentagemComissaoCorretora { get; set; }
    public decimal PorcentagemComissaoCooperativa { get; set; }

    public virtual Seguro Seguro { get; set; }
}
