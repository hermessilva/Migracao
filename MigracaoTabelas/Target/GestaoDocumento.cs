using System.ComponentModel;

namespace MigracaoTabelas.Target;

public sealed class GestaoDocumento
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public TipoGestaoDocumento Tipo { get; set; }
    public DateTime Validade { get; set; }
    public StatusGestaoDocumento Status { get; set; }
    public byte[] Modelo { get; set; }
    public string Extensao { get; set; }
    public DateTime? CriadoEm { get; set; }

    public Seguradora Seguradora { get; set; }
}

public enum TipoGestaoDocumento
{
    [Description("Termo de Adesão com DPS")]
    TermoDeAdesaoComDPS = 1,

    [Description("Termo de Adesão sem DPS")]
    TermoDeAdesaoSemDPS = 2
}

public enum StatusGestaoDocumento
{
    [Description("Ativo")]
    Ativo = 1,

    [Description("Inativo")]
    Inativo = 2
}