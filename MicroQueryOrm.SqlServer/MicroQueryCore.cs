using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common.Extensions;

namespace MicroQueryOrm.SqlServer
{
    /// <summary>
    /// MicroQueryCore: Contains all the core methods that connect to the Database.
    /// </summary>
    public partial class MicroQuery
    {
        /// <summary>
        /// This method accepts an Action to return the results as SqlDataReader. This way gives direct access to the data row by row.
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="readerAction">Action in order to have direct access when you get the data row by row.</param>
        /// <param name="parameters"></param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.StoredProcedure.</param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        private void _Query(string queryStr,
            Action<IDataReader> readerAction,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.StoredProcedure,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = GetSqlConnectionTransaction(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    readerAction(reader);
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
                    sqlConnection.Close();
                }
            }
        }
        
        /// <summary>
        /// Returns a DataTable
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="parameters"></param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.StoredProcedure.</param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        private DataTable _Query(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.StoredProcedure,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = GetSqlConnectionTransaction(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                using var reader = cmd.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    sqlConnection.Close();
                }
            }
        }

        /// <summary>
        /// This is the async method to execute a query and return a DataTable
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        private async Task<DataTable> _QueryAsync(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.StoredProcedure,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = await GetSqlConnectionTransactionAsync(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                using var reader = await cmd.ExecuteReaderAsync();
                var table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    await sqlConnection.CloseAsync();
                }
            }
        }

        private void _Execute(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = GetSqlConnectionTransaction(transaction);

            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    sqlConnection.Close();
                }
            }
        }

        private async Task _ExecuteAsync(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = await GetSqlConnectionTransactionAsync(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, commandType, parameters, timeoutSecs);
                await cmd.ExecuteNonQueryAsync();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    await sqlConnection.CloseAsync();
                }
            }
        }

        private TDestination _SingleResult<TDestination>(string queryStr,
            IDbDataParameter[]? parameters = null,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = GetSqlConnectionTransaction(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, CommandType.Text, parameters, timeoutSecs);
                var scalarResult = cmd.ExecuteScalar();
                if (scalarResult == DBNull.Value || scalarResult == null) return default(TDestination);
                var result = scalarResult.ChangeType<TDestination>();
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    sqlConnection.Close();
                }
            }
        }

        private async Task<TDestination> _SingleResultAsync<TDestination>(string queryStr,
            IDbDataParameter[]? parameters = null,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (sqlConnection, sqlTransaction) = await GetSqlConnectionTransactionAsync(transaction);
            try
            {
                using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, CommandType.Text, parameters, timeoutSecs);
                var scalarResult = await cmd.ExecuteScalarAsync();
                if (scalarResult == DBNull.Value || scalarResult == null) return default(TDestination);
                var result = scalarResult.ChangeType<TDestination>();
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (sqlTransaction == null)
                {
                    await sqlConnection.CloseAsync();
                }
            }
        }

        private (SqlConnection, SqlTransaction?) GetSqlConnectionTransaction(IDbTransaction? transaction = null)
        {
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            SqlConnection connection = sqlTransaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
            if (sqlTransaction == null)
            {
                connection.Open();
            }
            return (connection!, sqlTransaction);
        }

        private async Task<(SqlConnection, SqlTransaction?)> GetSqlConnectionTransactionAsync(IDbTransaction? transaction = null)
        {
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            SqlConnection connection = sqlTransaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
            if (sqlTransaction == null)
            {
                await connection.OpenAsync();
            }
            return (connection!, sqlTransaction);
        }

        private SqlCommand SqlCmd(SqlConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null, int? timeoutSecs = null)
        {
            int newTimeoutSecs = timeoutSecs ?? DbConfig.CommandTimeout;
            SqlTransaction? sqlTransaction = null;
            if (transaction != null)
            {
                sqlTransaction = transaction as SqlTransaction;
            }

            var cmd = sqlTransaction == null ? new SqlCommand(queryStr, connection)
            {
                CommandType = commandType,
                CommandTimeout = newTimeoutSecs
            } : new SqlCommand(queryStr, connection, sqlTransaction)
            {
                CommandType = commandType,
                CommandTimeout = newTimeoutSecs
            };

            if (parameters == null) return cmd;
            cmd.Parameters.AddRange(parameters);
            return cmd;
        }
    }
}
