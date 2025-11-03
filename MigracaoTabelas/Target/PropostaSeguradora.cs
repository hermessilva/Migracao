namespace MigracaoTabelas.Target;

public class PropostaSeguradora
{
    public ulong Id { get; set; }
    public ulong SeguradoraId { get; set; }
    public string DescricaoSequencial { get; set; }
    public string NumeroSequencial { get; set; }

    public virtual Seguradora Seguradora { get; set; } = null!;
}