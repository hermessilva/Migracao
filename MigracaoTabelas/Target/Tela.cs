namespace MigracaoTabelas.Target;

public class Tela
{
    public ulong Id { get; private set; }
    public string Descricao { get; private set; }

    public Tela(string descricao)
    {
        Descricao = descricao;
    }

    protected Tela() { }

    public void Atualizar(string descricao)
    {
        Descricao = descricao;
    }
}