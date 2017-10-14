using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kloud21.ADODAL
{
    public class DbSession : IDbSession
    {
        private readonly string _connectionString;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly IAdoNetProvider _adoNetProvider;
        private IDbConnection _connection;
        private IDbCommand _command;
        private IDbTransaction _transaction;

        #region [    From external provider    ]

        private IDbConnection GetConnection(string connectionString)
        {
            return _adoNetProvider.GetConnection(connectionString);
        }

        private IDbCommand GetCommand()
        {
            return _adoNetProvider.GetCommand();
        }

        private DbParameter GetParameter()
        {
            return _adoNetProvider.GetParameter();
        }

        #endregion

        public DbSession(IConnectionStringProvider connectionStringProvider, IAdoNetProvider adoNetProvider)
        {
            _connectionStringProvider = connectionStringProvider;
            _adoNetProvider = adoNetProvider;
            _connectionString = _connectionStringProvider.GetConnectionString();
        }

        private void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = GetConnection(_connectionString);
            }

            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
            }
            catch (SqlException ex)
            {
                //RecordSQLError(ex);
                CloseConnection();
                throw ex;
            }
        }

        public void BeginTransaction(IsolationLevel level = IsolationLevel.Unspecified)
        {
            _transaction = _connection.BeginTransaction(level);
        }

        private void PrepareCommand(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
            _command = GetCommand();
            _command.Connection = _connection;
            _command.CommandType = commandType;
            _command.CommandText = commandText;

            // set input paramters 
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var param = GetParameter();
                    param.ParameterName = parameter.Key;
                    param.Value = parameter.Value;
                    param.Direction = ParameterDirection.Input;
                    _command.Parameters.Add(param);
                }
            }

            // include output paramters 
            if (outputParameters != null)
            {
                foreach (var parameter in outputParameters)
                {
                    var param = GetParameter();
                    param.ParameterName = parameter.Key;
                    param.Value = parameter.Value;
                    param.Direction = ParameterDirection.Output;
                    _command.Parameters.Add(param);
                }
            }

            if (_transaction != null)
            {
                _command.Transaction = _transaction;
            }
        }

        public IList<T> ExecuteReader<T>(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null) 
            where T : new()
        {
            OpenConnection();

            var result = new List<T>();
            try
            {
                PrepareCommand(commandType, commandText, parameters, outputParameters);
                using (var dataReader = _command.ExecuteReader())
                {
                    if (dataReader == null)
                        return result;

                    while (dataReader.Read())
                    {
                        var entity = FillEntity<T>(dataReader);
                        result.Add(entity);
                    }
                }

                if (outputParameters != null)
                {
                    var temp = new Dictionary<string, object>();
                    foreach (var parameter in outputParameters)
                    {
                        var p = (DbParameter)_command.Parameters[parameter.Key];
                        temp.Add(parameter.Key, p.Value);
                    }
                    outputParameters.Clear();
                    foreach (var item in temp)
                    {
                        outputParameters.Add(item.Key, item.Value);
                    }
                }

                return result;
            }
            catch (SqlException ex)
            {
                //base.RecordSQLError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //base.RecordApplicationError(LogEntry.LogTypes.logError, ex);
                throw ex;
            }
            finally
            {
                CloseCommand();
            }
        }

        public T ExecuteScalar<T>(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
            OpenConnection();
            try
            {
                PrepareCommand(commandType, commandText, parameters);
                var result = _command.ExecuteScalar();
                return (T)Convert.ChangeType(result, typeof (T));
            }
            catch (SqlException ex)
            {
                //base.RecordSQLError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //base.RecordApplicationError(LogEntry.LogTypes.logError, ex);
                throw ex;
            }
            finally
            {
                CloseCommand();
            }
        }

        public int ExecuteNonQuery(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null)
        {
            OpenConnection();
            try
            {
                PrepareCommand(commandType, commandText, parameters);
                return _command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                //base.RecordSQLError(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                //base.RecordApplicationError(LogEntry.LogTypes.logError, ex);
                throw ex;
            }
            finally
            {
                CloseCommand();
            }
        }

        private static T FillEntity<T>(IDataReader dataReader) where T : new()
        {
            var entity = new T();

            var destinyType = entity.GetType();
            var destinyProperties = new List<PropertyInfo>(destinyType.GetProperties());
            // iterate through data reader fields
            for (var i = 0; i < dataReader.FieldCount; i++)
            {
                var propertyName = dataReader.GetName(i);
                var propertyValue = dataReader.GetValue(i);

                // find a property that matches the name, type and is writable
                var propertyInfo = destinyProperties.Find(x => x.Name == propertyName && x.PropertyType == propertyValue.GetType() && x.CanWrite);
                if (propertyInfo != null)
                {
                    if (propertyValue.GetType() == typeof(string)) {  propertyValue = propertyValue.ToString().Trim();}                   
                    propertyInfo.SetValue(entity, propertyValue, null);
                }
                
                // find out if the destination property could be Enum type
                propertyInfo = destinyProperties.Find(x => x.Name == propertyName && x.PropertyType.IsEnum && x.CanWrite);
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(entity, propertyValue, null);
                }
            }

            return entity;
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
                _transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
                _transaction.Rollback();
        }

        private void CloseCommand()
        {
            if (_command != null)
            {
                _command.Dispose();
                _command = null;
            }
        }

        public void DisposeTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private void CloseConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
