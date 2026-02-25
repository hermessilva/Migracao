using System.ComponentModel;

namespace MigracaoTabelas.Target;

/// <summary>
/// Representa o armazenamento de documentos em provedores de nuvem ou on-premises,
/// vinculados a um seguro específico. Permite rastreabilidade completa e auditoria
/// de todos os documentos armazenados.
/// </summary>
public sealed class ArmazenamentoDocumento
{
    public ulong Id { get; set; }
    public ulong SeguroId { get; set; }
    public string NomeOriginal { get; set; }
    public string ExtensaoArquivo { get; set; }
    public TipoDocumentoArmazenado Tipo { get; set; }
    public string Finalidade { get; set; }
    public LocalArmazenamentoDocumento Local { get; set; }
    public string UnidadeArmazenamento { get; set; }
    public string CaminhoArquivo { get; set; }
    public string CodigoRegiao { get; set; }
    public string EndpointAlias { get; set; }
    public string TipoMime { get; set; }
    public long TamanhoBytes { get; set; }
    public string HashControle { get; set; }
    public StatusArmazenamentoDocumento Status { get; set; }
    public string CriadoPor { get; set; }
    public DateTime CriadoEm { get; set; }
    public Seguro Seguro { get; set; }
}

/// <summary>
/// Tipo de documento armazenado no sistema.
/// Indica a natureza/finalidade do documento vinculado ao seguro.
/// </summary>
public enum TipoDocumentoArmazenado
{
    [Description("Termo de Adesão com DPS")]
    TermoAdesaoComDps = 1,

    [Description("Termo de Adesão sem DPS")]
    TermoAdesaoSemDps = 2,

    [Description("Documentação Complementar")]
    DocumentacaoComplementar = 3
}

/// <summary>
/// Local de armazenamento em nuvem ou on-premises utilizado.
/// </summary>
public enum LocalArmazenamentoDocumento
{
    [Description("Amazon Web Services S3")]
    AWS = 1,

    [Description("Microsoft Azure Blob Storage")]
    Azure = 2,

    [Description("Google Cloud Platform Storage")]
    GCP = 3,

    [Description("Oracle Cloud Infrastructure Object Storage")]
    OCI = 4,

    [Description("Armazenamento On-Premises")]
    OnPrem = 5
}

/// <summary>
/// Status do documento armazenado.
/// </summary>
public enum StatusArmazenamentoDocumento
{
    [Description("Ativo")]
    Ativo = 1,

    [Description("Inativo")]
    Inativo = 2,

    [Description("Pendente de Processamento")]
    PendenteProcessamento = 3,

    [Description("Erro no Upload")]
    ErroUpload = 4,

    [Description("Excluído")]
    Excluido = 5
}
