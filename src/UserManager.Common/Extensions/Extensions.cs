using Ganss.Xss;

namespace UserManager.Common.Extensions
{
    public static class Extensions
    {
        public static string SanitizeAndTrim(this string str)
        {
            if (str == null)
            {
                return null;
            }

            var sanitizer = new HtmlSanitizer();
            string sanitizedString = sanitizer.Sanitize(str)?.Trim();
            if (string.IsNullOrEmpty(sanitizedString))
            {
                return null;
            }
            return sanitizedString;
        }
    }
}
