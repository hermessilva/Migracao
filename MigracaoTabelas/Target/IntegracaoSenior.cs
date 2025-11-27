using System.ComponentModel;



namespace MigracaoTabelas.Target;

public class IntegracaoSenior
{
    public ulong Id { get; private set; }
    public ulong AgenciaId { get; private set; }
    public StatusEnvioIntegracaoSenior Status { get; private set; }
    public TipoLancamentoContabilIntegracaoSenior TipoLancamentoContabil { get; private set; }
    public string CodigoPa { get; private set; }
    public string ContaContabilDebito { get; private set; }
    public string ContaContabilCredito { get; private set; }
    public DateTime DataMovimentacao { get; private set; }
    public decimal Valor { get; private set; }
    public int NumeroLancamento { get; private set; }
    public string Descricao { get; private set; }
    public virtual Agencia Agencia { get; private set; }

    protected IntegracaoSenior() { }

    public IntegracaoSenior(
        TipoLancamentoContabilIntegracaoSenior tipoLancamentoContabil,
        string codigoPa,
        Agencia agencia,
        string contaContabilDebito,
        string contaContabilCredito,
        DateTime dataMovimentacao,
        decimal valor)
    {
        Status = StatusEnvioIntegracaoSenior.Enviado;
        Descricao = "Lançamento criado";
        TipoLancamentoContabil = tipoLancamentoContabil;
        CodigoPa = codigoPa;
        Agencia = agencia;
        AgenciaId = agencia.Id;
        ContaContabilDebito = contaContabilDebito;
        ContaContabilCredito = contaContabilCredito;
        DataMovimentacao = dataMovimentacao;
        Valor = valor;
    }

    public void AtualizarEnvioLancamento(TipoLancamentoContabilIntegracaoSenior tipoLancamentoContabil, bool enviadoSenior, string mensagemErroSenior)
    {
        if (enviadoSenior)
        {
            Status = StatusEnvioIntegracaoSenior.Enviado;
            Descricao = $"{tipoLancamentoContabil.AsString()} enviado com sucesso";
            return;
        }
        Status = StatusEnvioIntegracaoSenior.Falha;
        Descricao = $"{tipoLancamentoContabil.AsString()} falhou ao enviar: {mensagemErroSenior}";
    }
}

public enum StatusEnvioIntegracaoSenior
{
    Enviado,
    Falha
}

public enum TipoLancamentoContabilIntegracaoSenior
{
    [Description("Seguro Prestamista Contratado")]
    SeguroPrestamistaContratado = 1,

    [Description("Comissão Seguro Prestamista Contratado")]
    ComissaoSeguroPrestamistaContratado = 2,

    [Description("Cancelamento Seguro Prestamista Parcelado")]
    CancelamentoSeguroPrestamistaParcelado = 3,

    [Description("Cancelamento Seguro Prestamista Parcelado Comissão")]
    CancelamentoSeguroPrestamistaParceladoComissao = 4,

    [Description("Cancelamento Seguro Prestamista À Vista Proporcional Comissão")]
    CancelamentoSeguroPrestamistaAVistaProporcionalComissao = 5,

    [Description("Pagamento Seguro Prestamista")]
    PagamentoSeguroPrestamista = 6,

    [Description("Recebimento Comissão Seguro Prestamista")]
    RecebimentoComissaoSeguroPrestamista = 7
}