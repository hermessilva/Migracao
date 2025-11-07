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

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<TelaAcaoPerfil> TelasAcoesPerfis { get; set; } = new List<TelaAcaoPerfil>();
}