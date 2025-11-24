namespace MigracaoTabelas.Target;

public class GestaoDocumento
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string NomeDocumento { get; set; }
    public ushort Versao { get; set; }
    public string Label { get; set; }
    public string Campo { get; set; }
    public string Valor { get; set; }
    public uint Ordem { get; set; }
    public DateTime? CriadoEm { get; set; }

    public virtual Seguradora Seguradoras { get; set; }
}