using System.ComponentModel;
using System.Reflection;

public static class EnumHelper
{
    public static string AsString<TEnum>(this TEnum valueAnum)
        where TEnum : struct, Enum
        => EnumCache<TEnum>.GetDescription(valueAnum);

    public static bool IsEqual<TEnum>(TEnum leftEnum, string rightEnum) where TEnum : struct, Enum
    {
        var lkey = leftEnum.AsString();

        if (EnumCache<TEnum>.TryGetValue(rightEnum, out var value) && EnumCache<TEnum>.TryGetValue(lkey, out var rvalue))
            return rightEnum == lkey;
        return false;
    }

    public static bool IsEqual<TEnum>(string leftEnum, TEnum rightEnum) where TEnum : struct, Enum
    {
        var rkey = rightEnum.AsString();

        if (EnumCache<TEnum>.TryGetValue(leftEnum, out var value) && EnumCache<TEnum>.TryGetValue(rkey, out var rvalue))
            return leftEnum == rkey;
        return false;
    }

    public static TEnum FromString<TEnum>(string text)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Valor não pode ser nulo ou vazio.", nameof(text));

        if (EnumCache<TEnum>.TryGetValue(text, out var value))
            return value;

        throw new ArgumentException($"Valor inválido: {text}", nameof(text));
    }

    public static bool EhValido<TEnum>(string text)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(text))
            return false;

        return EnumCache<TEnum>.TryGetValue(text, out _);
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

                if (!Map.ContainsKey(name))
                    Map[name] = vlr;

                if (!Map.ContainsKey(description))
                    Map[description] = vlr;
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
