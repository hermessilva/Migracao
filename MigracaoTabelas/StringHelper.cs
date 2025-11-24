namespace Seguros.Helpers;

public static class StringHelper
{
    public static string ExtrairNumerosConta(this string conta)
    {
        return conta.Replace(".", string.Empty).Replace("-", string.Empty).Trim();
    }
}