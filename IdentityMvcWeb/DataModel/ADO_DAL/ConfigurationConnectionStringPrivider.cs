using System.Configuration;

namespace Kloud21.ADODAL
{
    public class ConfigurationConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionStringName;

        public ConfigurationConnectionStringProvider(string connectionStringName)
        {
            _connectionStringName = connectionStringName;
        }

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
        }
    }
}