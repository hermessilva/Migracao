using System.Globalization;

namespace MigracaoTabelas.Target;

public class Parametrizacao
{
    public ulong Id { get; set; }
    public string Descricao { get; set; }
    public string Identificador { get; set; }
    public string Valor { get; set; }
    public string Tipo { get; set; }

    public void AtualizarValor(TimeSpan? valor)
    {
        Valor = valor?.ToString();
        Tipo = "TimeSpan";
    }

    public void AtualizarValor(decimal? valor)
    {
        Valor = valor?.ToString(CultureInfo.InvariantCulture);
        Tipo = "Decimal";
    }
}