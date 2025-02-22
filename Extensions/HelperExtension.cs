namespace CuraMundi.Extensions
{
    public static class HelperExtension
    {
        public static string ToUpperCase(this string input) => string.IsNullOrEmpty(input) ? input : input.ToUpper();
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
