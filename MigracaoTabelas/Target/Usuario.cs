using System.ComponentModel;

namespace MigracaoTabelas.Target;

public class Usuario
{
    public ulong Id { get; set; }
    public ulong AgenciaId { get; set; }
    public ulong PontoAtendimentoId { get; set; }
    public ulong? PerfilId { get; set; }
    public string Login { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public StatusUsuario Status { get; set; }
    public DateTime CriadoEm { get; set; }
    public Perfil Perfils { get; set; }
    public Agencia Agencias { get; set; }
    public PontoAtendimento PontosAtendimentos { get; set; }

    public Usuario(string usuarioLogin, string nome, string email, ulong? perfilId, ulong agenciaId, ulong pontoAtendimentoId, StatusUsuario status = StatusUsuario.Ativo)
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

    public Usuario()
    {
        Login = string.Empty;
        Nome = string.Empty;
        Email = string.Empty;
    }

    public static Usuario Create(
        string usuarioLogin,
        string nome,
        string email,
        ulong? perfilId,
        ulong agenciaId,
        ulong pontoAtendimentoId,
        StatusUsuario status = StatusUsuario.Ativo)
    {
        return new Usuario(usuarioLogin, nome, email, perfilId, agenciaId, pontoAtendimentoId, status);
    }

    public void Atualizar(
        ulong? perfilId,
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
        ulong? perfilId,
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
        Status = StatusUsuario.Ativo;
    }

    public void Inativar()
    {
        Status = StatusUsuario.Inativo;
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
public enum StatusUsuario
{
    [Description("Ativo")]
    Ativo = 1,
    [Description("Inativo")]
    Inativo = 2
}