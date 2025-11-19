namespace MigracaoTabelas.Target;

public class Tela
{
    public ulong Id { get; set; }
    public string Descricao { get; set; }
    public string Slug { get; set; }

    public Tela(string descricao, string slug = null)
    {
        Descricao = descricao;
        Slug = slug;
    }

    protected Tela() { }

    public void Atualizar(string descricao)
    {
        Descricao = descricao;
    }


    public virtual ICollection<TelaAcao> TelasAcoes { get; set; } = new List<TelaAcao>();
    public virtual ICollection<TelaAcaoPerfil> TelasAcoesPerfis { get; set; } = new List<TelaAcaoPerfil>();
}