using System;
using System.Collections.Generic;
using System.Data;

namespace Kloud21.ADODAL
{
    public interface IDbSession : IDisposable
    {
        void BeginTransaction(IsolationLevel level = IsolationLevel.Unspecified);

        IList<T> ExecuteReader<T>(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null) 
            where T : new();

        void ExecuteScalar<T>(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null);

        int ExecuteNonQuery(CommandType commandType, string commandText, IDictionary<string, object> parameters, IDictionary<string, object> outputParameters = null);

        void CommitTransaction();

        void RollbackTransaction();

        void DisposeTransaction();
    }
}