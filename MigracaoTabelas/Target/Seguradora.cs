using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class Seguradora
{
    public Seguradora()
    {
        // EF Core
    }

    public Seguradora(ulong id, string nome, string cnpj, string razaoSocial, StatusSeguradora status = StatusSeguradora.Ativo)
    {
        Id = id;
        Nome = nome;
        Cnpj = cnpj;
        RazaoSocial = razaoSocial;
        Status = status;

        InitializeDefaults();
    }

    public ulong Id { get; set; }
    public string Nome { get; set; }
    public string Cnpj { get; set; }
    public string RazaoSocial { get; set; }
    public StatusSeguradora Status { get; set; }

    public virtual ICollection<CondicaoSeguradora> CondicoesSeguradora { get; set; } = new List<CondicaoSeguradora>();
    public virtual ICollection<GestaoDocumento> GestoesDocumentos { get; set; } = new List<GestaoDocumento>();
    public virtual ICollection<ContabilizacaoSeguradora> ContabilizacoesSeguradoras { get; set; } = new List<ContabilizacaoSeguradora>();
    public virtual ICollection<ContaCorrenteSeguradora> ContasCorrentes { get; set; } = new List<ContaCorrenteSeguradora>();
    public virtual ICollection<PropostaSeguradora> PropostasSeguradoras { get; set; } = new List<PropostaSeguradora>();
    public virtual ICollection<ComissaoSeguradora> ComissoesSeguradoras { get; set; } = new List<ComissaoSeguradora>();
    public virtual ICollection<SeguradoraLimite> SeguradorasLimites { get; set; } = new List<SeguradoraLimite>();
    public virtual ICollection<AgenciaSeguradora> AgenciasSeguradoras { get; set; } = new List<AgenciaSeguradora>();

    private void InitializeDefaults()
    {
        CondicoesSeguradora.Add(new CondicaoSeguradora
        {
            MaxMesesContrato = 0,
            MaxIdade = 0,
            PorcentagemCoberturaMorte = 0m,
            PorcentagemCoberturaInvalidez = 0m,
            PorcentagemCoberturaPerdaRenda = 0m,
            Periodicidade30Dias = false
        });

        ContabilizacoesSeguradoras.Add(new ContabilizacaoSeguradora
        {
            CreditoPremioContratacao = string.Empty,
            DescricaoCreditoPremioContratacao = string.Empty,
            DebitoPremioContratacao = string.Empty,
            DescricaoDebitoPremioContratacao = string.Empty,
            CreditoComissaoContratacao = string.Empty,
            DescricaoCreditoComissaoContratacao = string.Empty,
            DebitoComissaoContratacao = string.Empty,
            DescricaoDebitoComissaoContratacao = string.Empty,
            CreditoCancelamentoComissaoParcTot = string.Empty,
            DescricaoCreditoCancelamentoComissaoParcTot = string.Empty,
            DebitoCancelamentoComissaoParcTot = string.Empty,
            DescricaoDebitoCancelamentoComissaoParcTot = string.Empty,
            CreditoCancelamentoComissaoAVista = string.Empty,
            DescricaoCreditoCancelamentoComissaoAVista = string.Empty,
            DebitoCancelamentoComissaoAVista = string.Empty,
            DescricaoDebitoCancelamentoComissaoAVista = string.Empty,
            CreditoValorPago = string.Empty,
            DescricaoCreditoValorPago = string.Empty,
            DebitoValorPago = string.Empty,
            DescricaoDebitoValorPago = string.Empty,
            CreditoComissaoValorPago = string.Empty,
            DescricaoComissaoCreditoValorPago = string.Empty,
            DebitoComissaoValorPago = string.Empty,
            DescricaoComissaoDebitoValorPago = string.Empty,
            DebitoPremioParcela = string.Empty,
            DescricaoDebitoPremioParcela = string.Empty,
            CreditoPremioParcela = string.Empty,
            DescricaoCreditoPremioParcela = string.Empty,
            DebitoComissaoParcela = string.Empty,
            DescricaoDebitoComissaoParcela = string.Empty,
            CreditoComissaoParcela = string.Empty,
            DescricaoCreditoComissaoParcela = string.Empty
        });

        PropostasSeguradoras.Add(new PropostaSeguradora
        {
            DescricaoSequencial = string.Empty,
            NumeroSequencial = string.Empty
        });

        ContasCorrentes.Add(new ContaCorrenteSeguradora
        {
            ContaCorrentePrestamista = string.Empty,
            DescricaoContaCorrentePrestamista = string.Empty,
            ContaCancelamentoPrestamista = string.Empty,
            DescricaoContaCancelamentoPrestamista = string.Empty,
            ContaEstornoPrestamista = string.Empty,
            DescricaoContaEstornoPrestamista = string.Empty
        });

        ComissoesSeguradoras.Add(new ComissaoSeguradora
        {
            PorcentagemComissaoCorretora = 0m,
            PorcentagemComissaoCooperativa = 0m
        });
    }
}

public enum StatusSeguradora
{
    [Description("Ativo")]
    Ativo = 1,

    [Description("Inativo")]
    Inativo = 2
}