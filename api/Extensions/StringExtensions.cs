using System.Text.RegularExpressions;

namespace MockResponse.Api.Extensions
{
    public static class StringExtensions
    {
        public static string ToRegexPattern(this string input)
        {
            return "^" + Regex.Escape(input).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }
    }
}
