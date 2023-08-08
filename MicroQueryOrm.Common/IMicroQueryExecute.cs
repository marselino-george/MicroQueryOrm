
using System;
using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQueryExecute
    {
        void Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, Action<Exception>? onError = null);
        void Execute(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, Action<Exception>? onError = null, int timeoutSecs = 30);
    }
}
