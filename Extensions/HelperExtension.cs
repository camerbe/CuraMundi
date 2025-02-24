namespace CuraMundi.Extensions
{
    public static class HelperExtension
    {
        public static string ToUpperCase(this string input) => string.IsNullOrEmpty(input) ? input : input.ToUpper();
        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return string.Join(" ", input.Split(' ')
                .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower()));
        }
    }
}
