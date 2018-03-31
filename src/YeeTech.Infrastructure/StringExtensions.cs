using YeeTech.Infrastructure.Text;

namespace YeeTech.Infrastructure
{
    public static class StringExtensions
    {
        public static string ToSBC(this string text)
        {
            return TextUtils.ConvertToSBC(text);
        }

        public static string ToDBC(this string text)
        {
            return TextUtils.ConvertToDBC(text);
        }

        public static bool IsNumeric(this string text)
        {
            return ValidateUtils.IsNumeric(text);
        }
    }
}