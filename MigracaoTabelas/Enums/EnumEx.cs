using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;


namespace MigracaoTabelas.Enums
{

    public static class EnumEx
    {
        public static string AsString<TEnum>(this TEnum valueAnum)
            where TEnum : struct, Enum
            => EnumCache<TEnum>.GetDescription(valueAnum);

        public static bool IsEqual<TEnum>(TEnum leftEnum, string rightEnum) where TEnum : struct, Enum
        {
            var rkey = NormalizeKey(rightEnum);
            var lkey = NormalizeKey(leftEnum.AsString());

            if (EnumCache<TEnum>.TryGetValue(rkey, out var value) && EnumCache<TEnum>.TryGetValue(lkey, out var rvalue))
                return rkey == lkey;
            return false;
        }

        public static bool IsEqual<TEnum>(string leftEnum, TEnum rightEnum) where TEnum : struct, Enum
        {
            var lkey = NormalizeKey(leftEnum);
            var rkey = NormalizeKey(rightEnum.AsString());

            if (EnumCache<TEnum>.TryGetValue(lkey, out var value) && EnumCache<TEnum>.TryGetValue(rkey, out var rvalue))
                return lkey == rkey;
            return false;
        }

        public static TEnum FromString<TEnum>(this TEnum _, string text)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Valor não pode ser nulo ou vazio.", nameof(text));

            var key = NormalizeKey(text);
            if (EnumCache<TEnum>.TryGetValue(key, out var value))
                return value;

            throw new ArgumentException($"Valor inválido: {text}", nameof(text));
        }

        public static bool EhValido<TEnum>(string text)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(text))
                return false;

            var key = NormalizeKey(text);
            return EnumCache<TEnum>.TryGetValue(key, out _);
        }

        public static bool EhValido<TEnum>(this TEnum valueAnum)
            where TEnum : struct, Enum
            => Enum.IsDefined(typeof(TEnum), valueAnum);

        public static IReadOnlyList<string> GetValues<TEnum>()
            where TEnum : struct, Enum
            => EnumCache<TEnum>.GetAllDescriptions();

        public static IReadOnlyList<TEnum> GetItens<TEnum>()
            where TEnum : struct, Enum
            => EnumCache<TEnum>.GetAllItems().Select(item => item.Valor).ToList();

        static string NormalizeKey(string text)
        {
            if (text == null)
                return string.Empty;

            var temp = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(temp.Length);

            for (int i = 0; i < temp.Length; i++)
                if (CharUnicodeInfo.GetUnicodeCategory(temp[i]) != UnicodeCategory.NonSpacingMark)
                    sb.Append(temp[i]);

            return sb.ToString()
                     .Normalize(NormalizationForm.FormC)
                     .ToLowerInvariant()
                     .Trim();
        }

        static class EnumCache<TEnum> where TEnum : struct, Enum
        {
            static readonly Dictionary<string, TEnum> Map;
            static readonly Dictionary<TEnum, string> DescriptionMap;
            static readonly IReadOnlyList<string> Descriptions;
            static readonly IReadOnlyList<EnumItem<TEnum>> Items;

            static EnumCache()
            {
                var type = typeof(TEnum);
                var values = (TEnum[])Enum.GetValues(type);

                Map = new Dictionary<string, TEnum>(values.Length * 2);
                DescriptionMap = new Dictionary<TEnum, string>(values.Length);

                var descriptions = new string[values.Length];
                var items = new EnumItem<TEnum>[values.Length];

                for (int i = 0; i < values.Length; i++)
                {
                    var vlr = values[i];
                    var name = vlr.ToString();
                    var field = type.GetField(name);
                    var attr = field.GetCustomAttribute<DescriptionAttribute>();
                    var description = attr?.Description ?? name;

                    DescriptionMap[vlr] = description;
                    descriptions[i] = description;
                    items[i] = new EnumItem<TEnum>(vlr, name, description);

                    var normName = NormalizeKey(name);
                    if (!Map.ContainsKey(normName))
                        Map[normName] = vlr;

                    var normDesc = NormalizeKey(description);
                    if (!Map.ContainsKey(normDesc))
                        Map[normDesc] = vlr;
                }

                Descriptions = descriptions;
                Items = items;
            }

            public static bool TryGetValue(string key, out TEnum valueEnum)
                => Map.TryGetValue(key, out valueEnum);

            public static string GetDescription(TEnum valueEnum)
                => DescriptionMap[valueEnum];

            public static IReadOnlyList<string> GetAllDescriptions()
                => Descriptions;

            public static IReadOnlyList<EnumItem<TEnum>> GetAllItems()
                => Items;
        }

        public readonly record struct EnumItem<TEnum>(TEnum Valor, string Nome, string Descricao)
            where TEnum : struct, Enum;
    }
}
