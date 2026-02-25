#nullable enable

using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class FaturamentoImportacaoHistorico
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public TipoPagamentoSeguro TipoPagamento { get; set; }
    public string Lote { get; set; } = string.Empty;
    public string NomeArquivo { get; set; } = string.Empty;
    public ulong UsuarioId { get; set; }
    public TipoFaturamentoParcelaOrigem Origem { get; set; }
    public StatusImportacaoFaturamento StatusImportacao { get; set; }
    public int TotalRegistrosArquivo { get; set; }
    public int QuantidadeSegurosAceitos { get; set; }
    public int QuantidadeSegurosNaoAceitos { get; set; }
    public decimal ValorTotalSegurosAceitos { get; set; }
    public decimal ValorTotalSegurosNaoAceitos { get; set; }
    public decimal ValorTotalArquivo { get; set; }
    public decimal ValorTotalFaturar { get; set; }
    public string? Observacao { get; set; }
    public DateTime CriadoEm { get; set; }

    public virtual Seguradora Seguradora { get; set; } = null!;
    public virtual Usuario Usuario { get; set; } = null!;
}

public enum StatusImportacaoFaturamento
{
    [Description("Processado")]
    Processado = 1,

    [Description("Erro")]
    Erro = 2,

    [Description("Processado Parcialmente")]
    ProcessadoParcialmente = 3
}
