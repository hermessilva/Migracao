namespace MigracaoTabelas.Target;

public class Parametrizacao
{



    public ulong Id { get; set; }
    public string Descricao { get; set; }
    public string Identificador { get; set; }
    public string Valor { get; private set; }
    public string Tipo { get; private set; }

    public void AtualizarValor(TimeSpan? valor)
    {
        Valor = valor?.ToString();
        Tipo = "TimeSpan";
    }

    public void AtualizarValor(decimal? valor)
    {
        Valor = valor?.ToString();
        Tipo = "Decimal";
    }
}