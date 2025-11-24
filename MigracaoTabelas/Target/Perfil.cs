namespace MigracaoTabelas.Target;

public class Perfil
{
    public ulong Id { get; set; }
    public string Nome { get; set; }
    public string Slug { get; set; }

    public Perfil(string nome, string slug = null)
    {
        Nome = nome;
        Slug = slug;
    }

    protected Perfil() { }

    public void Atualizar(string nome)
    {
        Nome = nome;
    }

    public void AdicionarSlug(string nome)
    {
        Slug = ToSnakeCaseConverter.Convert(nome);
    }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public virtual ICollection<TelaAcaoPerfil> TelasAcoesPerfis { get; set; } = new List<TelaAcaoPerfil>();
}