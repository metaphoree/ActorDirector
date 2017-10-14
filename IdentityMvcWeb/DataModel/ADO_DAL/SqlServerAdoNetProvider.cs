using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Kloud21.ADODAL
{
    public class SqlServerAdoNetProvider : IAdoNetProvider
    {
        public IDbConnection GetConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public IDbCommand GetCommand()
        {
            return new SqlCommand();
        }

        public DbParameter GetParameter()
        {
            return new SqlParameter();
        }
    }
}