using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery
    {
        /// <summary>
        /// This method does not return anything but you can add an Action to have direct access when you get the data row by row.
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="readerAction">Action in order to have direct access when you get the data row by row.</param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.StoredProcedure.</param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        private void _Query(string queryStr,
            Action<SqlDataReader> readerAction,
            CommandType commandType = CommandType.StoredProcedure,
            IDbDataParameter[]? parameters = null,
            IDbTransaction? transaction = null,
            int timeoutSecs = 30)
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

                using var cmd = SqlCmd(connection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                using var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    readerAction(rdr);
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// This is the async method to execute a query and return a DataTable
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryAsync(string queryStr,
            CommandType commandType = CommandType.StoredProcedure,
            IDbDataParameter[]? parameters = null,
            IDbTransaction? transaction = null,
            int timeoutSecs = 30)
        {
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            var table = new DataTable();
            using var connection = new SqlConnection(DbConfig.ConnectionString);
            try
            {
                if (sqlTransaction == null)
                {
                    await connection.OpenAsync();
                }
                using var cmd = SqlCmd(connection, queryStr, commandType, parameters, timeoutSecs);
                using var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(table);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    await connection.CloseAsync();
                }
            }

            return table;
        }

        /// <summary>
        /// Returns a DataTable
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.StoredProcedure.</param>
        /// <param name="parameters"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        private DataTable _Query(string queryStr,
            CommandType commandType = CommandType.StoredProcedure,
            IDbDataParameter[]? parameters = null,
            IDbTransaction? transaction = null,
            int timeoutSecs = 30)
        {
            var table = new DataTable();
            using var connection = new SqlConnection(DbConfig.ConnectionString);
            try
            {
                if (transaction == null)
                {
                    connection.Open();
                }
                using var cmd = SqlCmd(connection, queryStr, commandType, parameters, timeoutSecs);
                using var dataAdapter = new SqlDataAdapter(cmd);
                dataAdapter.Fill(table);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (transaction == null)
                {
                    connection.Close();
                }
            }

            return table;
        }

        public static SqlCommand SqlCmd(SqlConnection connection, string queryStr,
            CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null,
            int timeoutSecs = 30)
        {
            return SqlCmd(connection, null, queryStr, commandType, parameters, timeoutSecs);
        }

        public static SqlCommand SqlCmd(SqlConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null, int timeoutSecs = 30)
        {
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            var cmd = sqlTransaction == null ? new SqlCommand(queryStr, connection)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSecs
            } : new SqlCommand(queryStr, connection, sqlTransaction)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSecs
            };

            if (parameters == null) return cmd;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }
    }
}
