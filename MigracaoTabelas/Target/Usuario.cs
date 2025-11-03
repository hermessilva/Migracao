namespace MigracaoTabelas.Target;

public class Usuario
{
    public ulong Id { get; private set; }
    public string UsuarioLogin { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime CriadoEm { get; private set; }

    public Usuario(string usuarioLogin, string nome, string email, bool ativo = true)
    {
        UsuarioLogin = usuarioLogin;
        Nome = nome;
        Email = email;
        Ativo = ativo;
        CriadoEm = DateTime.UtcNow;
    }

    protected Usuario() { }

    public void Atualizar(string nome, string email, bool ativo)
    {
        Nome = nome;
        Email = email;
        Ativo = ativo;
    }
}