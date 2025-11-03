namespace MigracaoTabelas.Target;

public class Auditoria
{
    public ulong Id { get; set; }
    public ulong UsuarioId { get; set; }
    public ulong AgenciaId { get; set; }
    public string Modulo { get; set; } = null!;
    public string Operacao { get; set; } = null!;
    public string Antes { get; set; } = null!;
    public string Depois { get; set; } = null!;
    public DateTime CriadoEm { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Agencia Agencia { get; set; } = null!;
}