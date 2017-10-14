using System.Data;
using System.Data.Common;

namespace Kloud21.ADODAL
{
    public interface IAdoNetProvider
    {
        IDbConnection GetConnection(string connectionString);

        IDbCommand GetCommand();

        DbParameter GetParameter();
    }
}