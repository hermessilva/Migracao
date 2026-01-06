
namespace MigracaoTabelas.Target;

public class SeguroParametro
{
    public ulong Id { get; set; }
    public TipoCapitalSeguro TipoCapital { get; set; }
    public bool Periodicidade30Dias { get; set; }
    public decimal Coeficiente { get; set; }
    public decimal PorcentualIof { get; set; }
    public decimal PorcentagemComissaoCorretora { get; set; }
    public decimal PorcentagemComissaoCooperativa { get; set; }

    // Coberturas
    public decimal PorcentagemCoberturaMorte { get; set; }
    public decimal CapitalMorte { get; set; }
    public decimal PremioMorte { get; set; }
    public decimal PorcentagemCoberturaInvalidez { get; set; }
    public decimal CapitalInvalidez { get; set; }
    public decimal PremioInvalidez { get; set; }
    public virtual Seguro Seguro { get; set; }

    /// <summary>
    /// Cria um novo SeguroParametro a partir dos dados de configuração
    /// </summary>
    /// <param name="apoliceGrupo">Apólice/Grupo da seguradora</param>
    /// <param name="periodicidade30Dias">Indica se a periodicidade é de 30 dias</param>
    /// <param name="coeficiente">Coeficiente obtido da tabela seguradora_limite baseado na idade do cooperado</param>
    /// <param name="porcentualIof">Percentual de IOF obtido da tabela parametrizacao</param>
    /// <param name="porcentagemComissaoCorretora">Percentual de comissão da corretora</param>
    /// <param name="porcentagemComissaoCooperativa">Percentual de comissão da cooperativa</param>
    public static SeguroParametro Criar(
        ApoliceGrupoSeguradora apoliceGrupo,
        bool periodicidade30Dias,
        decimal coeficiente,
        decimal porcentualIof,
        decimal porcentagemComissaoCorretora,
        decimal porcentagemComissaoCooperativa)
    {
        return new SeguroParametro
        {
            TipoCapital = apoliceGrupo.TipoCapital == TipoCapitalApoliceGrupoSeguradora.Fixo
                ? TipoCapitalSeguro.Fixo
                : TipoCapitalSeguro.Variavel,
            Periodicidade30Dias = periodicidade30Dias,
            Coeficiente = coeficiente,
            PorcentualIof = porcentualIof,
            PorcentagemComissaoCorretora = porcentagemComissaoCorretora,
            PorcentagemComissaoCooperativa = porcentagemComissaoCooperativa
        };
    }
}
