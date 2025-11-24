using System.Text;
using System.Text.RegularExpressions;



public static class ToSnakeCaseConverter
{
    public static string Convert(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // 1. Normaliza acentos
        string normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var unicode = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicode != System.Globalization.UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }

        string semAcentos = sb.ToString();

        // 2. Remove qualquer caractere não alfanumérico (exceto espaço)
        semAcentos = Regex.Replace(semAcentos, @"[^a-zA-Z0-9\s]", "");

        // 3. Remove artigos
        semAcentos = Regex.Replace(semAcentos, @"\b(de|da|do|das|dos|a|à|o|e)\b", "", RegexOptions.IgnoreCase);
        semAcentos = Regex.Replace(semAcentos, @"\s+", " ");

        // 4. Troca espaços por underline
        string snake = Regex.Replace(semAcentos, @"\s+", "_");

        // 5. Converte para minúsculo
        snake = snake.ToLowerInvariant();

        return snake;
    }
}
