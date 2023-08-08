using System;
using System.Data;
using System.Collections.Generic;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQuery
    {
        public void Query(string queryStr, Action<IDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null)
        {
            _Query(queryStr, readerAction, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout);
        }

        public void Query(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null)
        {
            _Query(queryStr, readerAction, commandType, parameters, transaction, DbConfig.CommandTimeout);
        }

        public DataTable Query(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null)
        {
            return _Query(queryStr, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout);
        }

        public DataTable Query(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null)
        {
            return _Query(queryStr, commandType, parameters, transaction, DbConfig.CommandTimeout);
        }

        public IEnumerable<T> Query<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null) where T : class, new()
        {
            return _Query(queryStr, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout).Map<T>();
        }

        public IEnumerable<T> Query<T>(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null) where T : class, new()
        {
            return _Query(queryStr, commandType, parameters, transaction, DbConfig.CommandTimeout).Map<T>();
        }
    }
}
