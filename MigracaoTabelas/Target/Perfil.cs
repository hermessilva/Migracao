namespace MigracaoTabelas.Target;

public class Perfil
{
    public ulong Id { get; private set; }
    public string Nome { get; private set; }

    public Perfil(string nome)
    {
        Nome = nome;
    }

    protected Perfil() { }

    public void Atualizar(string nome, string email, bool ativo)
    {
        Nome = nome;
    }
}