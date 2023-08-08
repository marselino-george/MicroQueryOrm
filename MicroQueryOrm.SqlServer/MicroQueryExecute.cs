using System.Data;
using System;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQueryExecute
    {
        public void Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, Action<Exception>? onError = null)
        {
            _Execute(queryStr, commandType, transaction: transaction, onError: onError);
        }

        public void Execute(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, Action<Exception>? onError = null, int timeoutSecs = 30)
        {
            _Execute(queryStr, commandType, parameters, transaction, onError, timeoutSecs);
        }
        private void _Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, Action<Exception>? onError = null, int timeoutSecs = 30)
        {
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            using var connection = sqlTransaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
            try
            {
                if (sqlTransaction == null)
                {
                    connection.Open();
                }

                var cmd = SqlCmd(connection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                onError?.Invoke(exception);
                if (onError == null) throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                }
            }
        }
    }
}
