namespace MigracaoTabelas.Target;

public class GestaoDocumento
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string NomeDocumento { get; set; } = null!;
    public ushort Versao { get; set; }
    public string Label { get; set; } = null!;
    public string Campo { get; set; } = null!;
    public string Valor { get; set; } = null!;
    public uint Ordem { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Seguradora Seguradoras { get; set; } = null!;
}