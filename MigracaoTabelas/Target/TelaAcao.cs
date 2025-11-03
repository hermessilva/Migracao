namespace MigracaoTabelas.Target;

public class TelaAcao
{
    public ulong Id { get; private set; }
    public ulong TelaId { get; private set; }
    public ulong AcaoId { get; private set; }

    public virtual Tela Tela { get; private set; }
    public virtual Acao Acao { get; private set; }

    public TelaAcao(ulong telaId, ulong acaoId)
    {
        TelaId = telaId;
        AcaoId = acaoId;
    }

    protected TelaAcao() { }

    public void Atualizar(ulong telaId, ulong acaoId)
    {
        TelaId = telaId;
        AcaoId = acaoId;
    }
}