using System.Text.RegularExpressions;

namespace TranslatorApp.Utils
{
    public static class StringEx
    {
        public static bool IsCyrilic(this string str)
        {
            return Regex.IsMatch(str, @"\p{IsCyrillic}");
        }
    }
}
