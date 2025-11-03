namespace MigracaoTabelas.Target;

public class Acao
{
    public ulong Id { get; private set; }
    public string Descricao { get; private set; }

    public Acao(string descricao)
    {
        Descricao = descricao;
    }

    protected Acao() { }

    public void Atualizar(string descricao)
    {
        Descricao = descricao;
    }
}