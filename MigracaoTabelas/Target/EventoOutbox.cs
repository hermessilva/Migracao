using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class EventoOutbox
{
    public ulong Id { get; set; }
    // Identificador lógico da operação (ex.: "PROCESSAR_DEBITO_PARCELA-123")
    public string IdempotencyKey { get; set; }
    public TipoEvento Tipo { get; set; }
    public string ChaveNegocio { get; set; }
    public string Payload { get; set; }
    public OutboxStatus Status { get; set; } = OutboxStatus.Pendente;
    public int Tentativas { get; set; }
    public string UltimaAtualizacao { get; set; }
    public string ExternalId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public DateTime? ProcessadoEm { get; set; }
}

public enum OutboxStatus
{
    [Description("Pendente")]
    Pendente = 0,
    [Description("Processando")]
    Processando = 1,
    [Description("Sucesso")]
    Sucesso = 2,
    [Description("Falha")]
    Falha = 3
}

public enum TipoEvento
{
    [Description("Processar débito da parcela")]
    ProcessarDebitoParcela = 0,
    [Description("Processar lançamento contábil da parcela")]
    ProcessarLancamentoContabilParcela = 1
}