using System.ComponentModel;

namespace MigracaoTabelas.Target;

/// <summary>
/// Representa a baixa de comissão de um lote/fatura de seguro prestamista.
/// Cada registro é um agrupamento de parcelas da tabela faturamento_parcela,
/// agrupadas por seguradora, lote, competência e tipo de pagamento.
/// </summary>
public class BaixaComissao
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string Lote { get; set; } = string.Empty;
    public DateTime Competencia { get; set; }
    public TipoPagamentoSeguro TipoPagamento { get; set; }
    public DateTime DataEmissao { get; set; }
    public decimal ValorComissao { get; set; }
    public int QuantidadeCooperados { get; set; }
    public DateTime? DataRecebimento { get; set; }
    public SituacaoBaixaComissao Situacao { get; set; }
    public ulong? UsuarioBaixaId { get; set; }
    public DateTime CriadoEm { get; set; }

    public virtual Seguradora Seguradora { get; set; } = null!;
    public virtual Usuario UsuarioBaixa { get; set; }
}

/// <summary>
/// Situação da baixa de comissão
/// </summary>
public enum SituacaoBaixaComissao
{
    [Description("Pendente")]
    Pendente = 1,

    [Description("Recebida")]
    Recebida = 2
}
