namespace MigracaoTabelas.Target;

public class ContabilizacaoSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string CreditoPremioContratacao { get; set; }
    public string DescricaoCreditoPremioContratacao { get; set; }
    public string DebitoPremioContratacao { get; set; }
    public string DescricaoDebitoPremioContratacao { get; set; }
    public string CreditoComissaoContratacao { get; set; }
    public string DescricaoCreditoComissaoContratacao { get; set; }
    public string DebitoComissaoContratacao { get; set; }
    public string DescricaoDebitoComissaoContratacao { get; set; }
    public string CreditoCancelamentoComissaoParcTot { get; set; }
    public string DescricaoCreditoCancelamentoComissaoParcTot { get; set; }
    public string DebitoCancelamentoComissaoParcTot { get; set; }
    public string DescricaoDebitoCancelamentoComissaoParcTot { get; set; }
    public string CreditoCancelamentoComissaoAVista { get; set; }
    public string DescricaoCreditoCancelamentoComissaoAVista { get; set; }
    public string DebitoCancelamentoComissaoAVista { get; set; }
    public string DescricaoDebitoCancelamentoComissaoAVista { get; set; }
    public string CreditoValorPago { get; set; }
    public string DescricaoCreditoValorPago { get; set; }
    public string DebitoValorPago { get; set; }
    public string DescricaoDebitoValorPago { get; set; }
    public string CreditoComissaoValorPago { get; set; }
    public string DescricaoComissaoCreditoValorPago { get; set; }
    public string DebitoComissaoValorPago { get; set; }
    public string DescricaoComissaoDebitoValorPago { get; set; }

    public virtual Seguradora Seguradoras { get; set; }
}