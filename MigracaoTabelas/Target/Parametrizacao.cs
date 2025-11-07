namespace MigracaoTabelas.Target;

public class Parametrizacao
{
    public ulong Id { get; set; }
    public string Descricao { get; set; } = null!;
    public virtual ICollection<ParametrizacaoResposta> ParametrizacoesRespostas { get; set; } = new List<ParametrizacaoResposta>();

    public void AtualizarResposta(TimeSpan? atualizacao)
    {
        ParametrizacoesRespostas.FirstOrDefault().Resposta = atualizacao?.ToString();
    }
}