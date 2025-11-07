namespace MigracaoTabelas.Target;

public class Usuario
{
    public ulong Id { get; private set; }
    public ulong AgenciaId { get; private set; }
    public ulong PontoAtendimentoId { get; private set; }
    public ulong PerfilId { get; private set; }
    public string Login { get; private set; }
    public string Nome { get; private set; }
    public string Email { get; private set; }
    public string Status { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public Perfil Perfils { get; private set; }
    public Agencia Agencias { get; private set; }
    public PontoAtendimento PontosAtendimentos { get; private set; }

    public Usuario(string usuarioLogin, string nome, string email, ulong perfilId, ulong agenciaId, ulong pontoAtendimentoId, string status = "Ativo")
    {
        ValidarUsuarioLogin(usuarioLogin);
        ValidarNome(nome);
        ValidarEmail(email);

        Login = usuarioLogin;
        Nome = nome;
        Email = email;
        PerfilId = perfilId;
        AgenciaId = agenciaId;
        PontoAtendimentoId = pontoAtendimentoId;
        Status = status;
        CriadoEm = DateTime.UtcNow;
    }

    protected Usuario()
    {
        Login = string.Empty;
        Nome = string.Empty;
        Email = string.Empty;
    }

    public static Usuario Create(
        string usuarioLogin,
        string nome,
        string email,
        ulong perfilId,
        ulong agenciaId,
        ulong pontoAtendimentoId,
        string status = "Ativo")
    {
        return new Usuario(usuarioLogin, nome, email, perfilId, agenciaId, pontoAtendimentoId, status);
    }

    public void Atualizar(
        ulong perfilId,
        ulong agenciaId,
        ulong pontoAtendimentoId)
    {
        PerfilId = perfilId;
        AgenciaId = agenciaId;
        PontoAtendimentoId = pontoAtendimentoId;
    }

    public void AtualizarComDadosSensiveis(
        string nome,
        string email,
        ulong perfilId,
        ulong agenciaId,
        ulong pontoAtendimentoId)
    {
        if (!string.IsNullOrEmpty(nome))
        {
            ValidarNome(nome);
            Nome = nome;
        }

        if (!string.IsNullOrEmpty(email))
        {
            ValidarEmail(email);
            Email = email;
        }

        PerfilId = perfilId;
        AgenciaId = agenciaId;
        PontoAtendimentoId = pontoAtendimentoId;
    }

    public void Ativar()
    {
        Status = "Ativo";
    }

    public void Inativar()
    {
        Status = "Inativo";
    }

    private static void ValidarUsuarioLogin(string usuarioLogin)
    {
        if (string.IsNullOrWhiteSpace(usuarioLogin))
        {
            throw new ArgumentException("Login do usuário é obrigatório.", nameof(usuarioLogin));
        }

        if (usuarioLogin.Contains(' '))
        {
            throw new ArgumentException("Login do usuário não pode conter espaços.", nameof(usuarioLogin));
        }
    }

    private static void ValidarNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        }

        if (nome.Length > 250)
        {
            throw new ArgumentException("Nome não pode ter mais de 250 caracteres.", nameof(nome));
        }
    }

    private static void ValidarEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email é obrigatório.", nameof(email));
        }

        if (!email.Contains('@') || !email.Contains('.'))
        {
            throw new ArgumentException("Email com formato inválido.", nameof(email));
        }

        var emailRegex = new System.Text.RegularExpressions.Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("Email com formato inválido.", nameof(email));
        }
    }
}