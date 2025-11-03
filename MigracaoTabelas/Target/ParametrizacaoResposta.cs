namespace MigracaoTabelas.Target;

public class ParametrizacaoResposta
{
    public ulong Id { get; set; }
    public ulong ParametrizacaoId { get; set; }
    public string Resposta { get; set; } = null!;

    public virtual Parametrizacao Parametrizacao { get; set; } = null!;
}