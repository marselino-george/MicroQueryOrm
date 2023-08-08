
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Common
{
    public interface IMicroQuery
    {
        DataTable Query(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null);
        void Query(string queryStr, Action<IDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null);
        DataTable Query(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null);
        void Query(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null);
        IEnumerable<T> Query<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null) where T : class, new();
        IEnumerable<T> Query<T>(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null) where T : class, new();
        Task<DataTable> QueryAsync(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, int timeoutSecs = 30);
    }
}
