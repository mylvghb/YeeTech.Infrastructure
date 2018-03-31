using System.Data;

namespace YeeTech.Infrastructure
{
    public static class DataExtensions
    {
        public static bool IsNullOrEmpty(this DataSet ds)
        {
            return !ValidateUtils.CheckedDataSet(ds);
        }

        public static bool IsNullOrEmpty(this DataTable dt)
        {
            return !ValidateUtils.CheckedDataTable(dt);
        }
    }
}