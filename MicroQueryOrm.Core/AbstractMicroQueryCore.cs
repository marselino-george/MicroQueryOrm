using System;
using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Common.Extensions;

namespace MicroQueryOrm.Core
{
    /// <summary>
    /// AbstractMicroQuery: Contains all the core methods for the connection to the Database.
    /// </summary>
    public abstract partial class AbstractMicroQuery
    {
        /// <summary>
        /// This method accepts an Action to return the results as IDataReader. This way gives direct access to the data row by row.
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="readerAction">Action in order to have direct access when you get the data row by row.</param>
        /// <param name="parameters"></param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.Text.</param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        protected void _Query(string queryStr,
            Action<IDataReader> readerAction,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = _databaseStrategy.GetConnectionTransaction(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
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
                if (dbTransaction == null)
                {
                    dbConnection.Close();
                }
            }
        }

        /// <summary>
        /// Returns a DataTable
        /// </summary>
        /// <param name="queryStr">Query or Name of Stored Procedure</param>
        /// <param name="parameters"></param>
        /// <param name="commandType">Stored Procedure or Transact-SQL statement. The default is CommandType.Text.</param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs">Time in seconds to wait for the command to execute. The default is 30 seconds.</param>
        /// <returns></returns>
        protected DataTable _Query(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = _databaseStrategy.GetConnectionTransaction(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
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
                if (dbTransaction == null)
                {
                    dbConnection.Close();
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
        protected async Task<DataTable> _QueryAsync(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = _databaseStrategy.GetConnectionTransaction(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
                using var reader = await _databaseStrategy.ExecuteReaderAsync(cmd);
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
                if (dbTransaction == null)
                {
                    await _databaseStrategy.CloseConnectionAsync(dbConnection);
                }
            }
        }

        protected void _Execute(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = _databaseStrategy.GetConnectionTransaction(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbTransaction == null)
                {
                    dbConnection.Close();
                }
            }
        }

        protected async Task _ExecuteAsync(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = await _databaseStrategy.GetConnectionTransactionAsync(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
                await _databaseStrategy.ExecuteNonQueryAsync(cmd);
            }
            catch
            {
                throw;
            }
            finally
            {
                if (dbTransaction == null)
                {
                    await _databaseStrategy.CloseConnectionAsync(dbConnection);
                }
            }
        }

        protected TDestination _SingleResult<TDestination>(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = _databaseStrategy.GetConnectionTransaction(transaction);
            try
            {
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
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
                if (dbTransaction == null)
                {
                    dbConnection.Close();
                }
            }
        }

        protected async Task<TDestination> _SingleResultAsync<TDestination>(string queryStr,
            IDbDataParameter[]? parameters = null,
            CommandType commandType = CommandType.Text,
            IDbTransaction? transaction = null,
            int? timeoutSecs = null)
        {
            var (dbConnection, dbTransaction) = await _databaseStrategy.GetConnectionTransactionAsync(transaction);
            try
            {
                //using var cmd = SqlCmd(sqlConnection, sqlTransaction, queryStr, CommandType.Text, parameters, timeoutSecs);
                using var cmd = _databaseStrategy.CreateCommand(dbConnection, dbTransaction, queryStr, commandType, parameters, timeoutSecs);
                var scalarResult = await _databaseStrategy.ExecuteScalarAsync(cmd);
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
                if (dbTransaction == null)
                {
                    await _databaseStrategy.CloseConnectionAsync(dbConnection);
                }
            }
        }

        //private (SqlConnection, SqlTransaction?) GetSqlConnectionTransaction(IDbTransaction? transaction = null)
        //{
        //    SqlTransaction? sqlTransaction = null;
        //    if (transaction != null)
        //    {
        //        sqlTransaction = transaction as SqlTransaction;
        //    }

        //    SqlConnection connection = sqlTransaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
        //    if (sqlTransaction == null)
        //    {
        //        connection.Open();
        //    }
        //    return (connection!, sqlTransaction);
        //}

        //private async Task<(SqlConnection, SqlTransaction?)> GetSqlConnectionTransactionAsync(IDbTransaction? transaction = null)
        //{
        //    SqlTransaction? sqlTransaction = null;
        //    if (transaction != null)
        //    {
        //        sqlTransaction = transaction as SqlTransaction;
        //    }

        //    SqlConnection connection = sqlTransaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
        //    if (sqlTransaction == null)
        //    {
        //        await connection.OpenAsync();
        //    }
        //    return (connection!, sqlTransaction);
        //}

        //private SqlCommand SqlCmd(SqlConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType = CommandType.Text, IDbDataParameter[]? parameters = null, int? timeoutSecs = null)
        //{
        //    int newTimeoutSecs = timeoutSecs ?? DbConfig.CommandTimeout;
        //    SqlTransaction? sqlTransaction = null;
        //    if (transaction != null)
        //    {
        //        sqlTransaction = transaction as SqlTransaction;
        //    }

        //    var cmd = sqlTransaction == null ? new SqlCommand(queryStr, connection)
        //    {
        //        CommandType = commandType,
        //        CommandTimeout = newTimeoutSecs
        //    } : new SqlCommand(queryStr, connection, sqlTransaction)
        //    {
        //        CommandType = commandType,
        //        CommandTimeout = newTimeoutSecs
        //    };

        //    if (parameters == null) return cmd;
        //    cmd.Parameters.AddRange(parameters);
        //    return cmd;
        //}
    }
}
