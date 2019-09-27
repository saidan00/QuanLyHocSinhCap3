using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;

namespace QuanLyHocSinhCap3.DAL
{
    public class HighSchoolConfiguration : DbConfiguration
    {
        public HighSchoolConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
            DbInterception.Add(new HighSchoolInterceptorTransientErrors());
            DbInterception.Add(new HighSchoolInterceptorLogging());
        }
    }
}