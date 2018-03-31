namespace YeeTech.Infrastructure
{
    public static class NumberExtensions
    {
        public static string ToRMB<T>(this T num) where T : struct
        {
            return RMBUtils.ConvertToRMB(num.ToString());
        }

        public static string ToRMB(this string numText)
        {
            return RMBUtils.ConvertToRMB(numText);
        }
    }
}