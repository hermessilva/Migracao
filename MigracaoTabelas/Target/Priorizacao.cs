namespace MigracaoTabelas.Target;

public class Priorizacao
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong SeguradoraId { get; set; }
    public byte Ordem { get; set; }

    public virtual Agencia Agencias { get; set; } = null!;
    public virtual Seguradora Seguradoras { get; set; } = null!;
}

