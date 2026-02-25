using System.ComponentModel;

namespace MigracaoTabelas.Target;

/// <summary>
/// Representa o histórico de exportações de faturamento para seguradoras
/// </summary>
public class ExportacaoFaturamento
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string UsuarioEmail { get; set; } = string.Empty;
    public TipoPagamentoSeguro TipoPagamento { get; set; }
    public DateTime PeriodoInicio { get; set; }
    public DateTime PeriodoFim { get; set; }
    public int QuantidadeRegistros { get; set; }
    public StatusExportacao Status { get; set; }
    public string NomeArquivo { get; set; } = string.Empty;
    public string ArquivoHash { get; set; } = string.Empty;
    public string MensagemErro { get; set; }
    public DateTime CriadoEm { get; set; }
    
    public virtual Seguradora Seguradora { get; set; } = null!;
}

/// <summary>
/// Status da exportação de faturamento
/// </summary>
public enum StatusExportacao
{
    [Description("Sucesso")]
    Sucesso = 1,
    
    [Description("Erro")]
    Erro = 2
}
