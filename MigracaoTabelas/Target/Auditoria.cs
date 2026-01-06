using System.ComponentModel;

namespace MigracaoTabelas.Target;


public class Auditoria
{
    public ulong Id { get; set; }
    public ulong? Chave { get; set; }
    public ulong? UsuarioId { get; set; }
    public ulong? AgenciaId { get; set; }
    public string Modulo { get; set; }
    public string Rota { get; set; }
    public string Tabela { get; set; }
    public OperacaoAuditoria Operacao { get; set; }
    public string DadosAnteriores { get; set; }
    public DateTime CriadoEm { get; set; }
}

public enum OperacaoAuditoria
{
    [Description("Inserção")]
    Insert = 1,
    [Description("Atualização")]
    Update = 2,
    [Description("Deleção")]
    Delete = 3,
    [Description("Login")]
    Login = 4,
    [Description("Refresh Token")]
    RefreshToken = 5
}