namespace MigracaoTabelas.Target;

public class TelaAcaoPerfil
{
    public ulong Id { get; private set; }
    public ulong TelaId { get; private set; }
    public ulong AcaoId { get; private set; }
    public ulong PerfilId { get; private set; }

    public TelaAcaoPerfil(ulong telaId, ulong acaoId, ulong perfilId)
    {
        TelaId = telaId;
        AcaoId = acaoId;
        PerfilId = perfilId;
    }

    protected TelaAcaoPerfil() { }

    public void Atualizar(ulong telaId, ulong acaoId, ulong perfilId)
    {
        TelaId = telaId;
        AcaoId = acaoId;
        PerfilId = perfilId;
    }
}