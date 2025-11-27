namespace MigracaoTabelas.Target;

public class Auditoria
{
    public ulong Id { get; set; }
    public ulong UsuarioId { get; set; }
    public ulong AgenciaId { get; set; }
    public string Modulo { get; set; }
    public OperacaoAuditoria Operacao { get; set; }
    public string Antes { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Usuario Usuarios { get; set; }
    public virtual Agencia Agencias { get; set; }
}

public enum OperacaoAuditoria
{
    Insert,
    Delete,
    Update
}