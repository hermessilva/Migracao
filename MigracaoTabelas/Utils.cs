using System.Text;

namespace MigracaoTabelas
{
    public static class Utils
    {
        public static String OnlyNumbers(this String pValue)
        {
            if (String.IsNullOrEmpty(pValue))
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (Char ch in pValue)
                if (ch >= '0' && ch <= '9')
                    sb.Append(ch);
            return sb.ToString();
        }

        public static T Read<T>(string varName, T defaultValue = default, string varEmptyError = null)
            where T : IComparable, IConvertible
        {
            if (string.IsNullOrEmpty(varName))
                throw new ArgumentNullException(nameof(varName));
            bool hasDefault = !EqualityComparer<T>.Default.Equals(defaultValue, default);
            var value = Environment.GetEnvironmentVariable(varName);

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            var type = typeof(T);
            if (type.IsEnum)
            {
                if (Enum.TryParse(type, value.ToString(), out object ret))
                    return (T)ret;
            }
            T result = (T)Convert.ChangeType(value, typeof(T));
            if (!EqualityComparer<T>.Default.Equals(result, defaultValue))
                return result;
            return defaultValue;

        }
    }
}
