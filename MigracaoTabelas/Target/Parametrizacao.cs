namespace MigracaoTabelas.Target;

public class Parametrizacao
{
    public ulong Id { get; set; }
    public string Descricao { get; set; } = null!;

    public virtual ICollection<ParametrizacaoResposta> ParametrizacaoRespostas { get; set; } = new List<ParametrizacaoResposta>();
}