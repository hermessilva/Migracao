namespace MigracaoTabelas.Target;

public class ContabilizacaoSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string CreditoPremioContratacao { get; set; } = null!;
    public string DescricaoCreditoPremioContratacao { get; set; } = null!;
    public string DebitoPremioContratacao { get; set; } = null!;
    public string DescricaoDebitoPremioContratacao { get; set; } = null!;
    public string CreditoComissaoContratacao { get; set; } = null!;
    public string DescricaoCreditoComissaoContratacao { get; set; } = null!;
    public string DebitoComissaoContratacao { get; set; } = null!;
    public string DescricaoDebitoComissaoContratacao { get; set; } = null!;
    public string CreditoCancelamentoComissaoParcTot { get; set; } = null!;
    public string DescricaoCreditoCancelamentoComissaoParcTot { get; set; } = null!;
    public string DebitoCancelamentoComissaoParcTot { get; set; } = null!;
    public string DescricaoDebitoCancelamentoComissaoParcTot { get; set; } = null!;
    public string CreditoCancelamentoComissaoAVista { get; set; } = null!;
    public string DescricaoCreditoCancelamentoComissaoAVista { get; set; } = null!;
    public string DebitoCancelamentoComissaoAVista { get; set; } = null!;
    public string DescricaoDebitoCancelamentoComissaoAVista { get; set; } = null!;
    public string CreditoValorPago { get; set; } = null!;
    public string DescricaoCreditoValorPago { get; set; } = null!;
    public string DebitoValorPago { get; set; } = null!;
    public string DescricaoDebitoValorPago { get; set; } = null!;
    public string CreditoComissaoValorPago { get; set; } = null!;
    public string DescricaoComissaoCreditoValorPago { get; set; } = null!;
    public string DebitoComissaoValorPago { get; set; } = null!;
    public string DescricaoComissaoDebitoValorPago { get; set; } = null!;

    public virtual Seguradora Seguradoras { get; set; } = null!;
}