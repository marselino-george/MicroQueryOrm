using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.SqlServer
{
    public class MicroQuery
    {
        protected readonly IDataBaseConfiguration DbConfig;

        public MicroQuery(IDataBaseConfiguration dbConfig)
        {
            if (dbConfig.CommandTimeout < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dbConfig.CommandTimeout));
            }
            DbConfig = dbConfig;
        }

        public SqlTransaction BeginTransaction()
        {
            var connection = new SqlConnection(DbConfig.ConnectionString);
            connection.Open();
            return connection.BeginTransaction();
        }

        public void CommitTransaction(SqlTransaction transaction)
        {
            transaction.Commit();
            transaction.Connection.Close();
        }

        public void RollbackTransaction(SqlTransaction transaction)
        {
            transaction.Rollback();
            transaction.Connection.Close();
        }

        public void Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null, Action<Exception>? onError = null)
        {
            _Execute(queryStr, commandType, transaction: transaction, onError: onError);
        }

        public void Execute(string queryStr, IDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null, Action<Exception>? onError = null, int timeoutSecs = 30)
        {
            _Execute(queryStr, commandType, parameters, transaction, onError, timeoutSecs);
        }

        private void _Execute(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDataParameter[]? parameters = null, SqlTransaction? transaction = null, Action<Exception>? onError = null, int timeoutSecs = 30)
        {
            using var connection = transaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);
            try
            {
                if (transaction == null)
                {
                    connection.Open();
                }

                var cmd = SqlCmd(connection, transaction, queryStr, commandType, parameters, timeoutSecs);
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

        public DataTable Query(string queryStr, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null)
        {
            return _Query(queryStr, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout);
        }

        public DataTable Query(string queryStr, IDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null)
        {
            return _Query(queryStr, commandType, parameters, transaction, DbConfig.CommandTimeout);
        }

        public IEnumerable<T> Query<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null) where T : class, new()
        {
            return _Query(queryStr, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout).Map<T>();
        }

        public IEnumerable<T> Query<T>(string queryStr, IDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null) where T : class, new()
        {
            return _Query(queryStr, commandType, parameters, transaction, DbConfig.CommandTimeout).Map<T>();
        }

        public void Query(string queryStr, Action<SqlDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null)
        {
            _Query(queryStr, readerAction, commandType, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout);
        }

        public void Query(string queryStr, IDataParameter[] parameters, Action<SqlDataReader> readerAction, CommandType commandType = CommandType.StoredProcedure, SqlTransaction? transaction = null)
        {
            _Query(queryStr, readerAction, commandType, parameters, transaction, DbConfig.CommandTimeout);
        }

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
            IDataParameter[]? parameters = null,
            SqlTransaction? transaction = null,
            int timeoutSecs = 30)
        {
            SqlDataReader? rdr = null;
            var connection = transaction?.Connection ?? new SqlConnection(DbConfig.ConnectionString);

            try
            {
                if (transaction == null)
                {
                    connection.Open();
                }

                var cmd = SqlCmd(connection, transaction, queryStr, commandType, parameters, timeoutSecs);

                rdr = cmd.ExecuteReader();
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
                rdr?.Close();
                if (transaction == null)
                {
                    connection.Close();
                }
            }
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
            IDataParameter[]? parameters = null,
            SqlTransaction? transaction = null,
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
                var cmd = SqlCmd(connection, queryStr, commandType, parameters, timeoutSecs);
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

        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object UniqueResult(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDataParameter[] parameters = null)
        {
            return UniqueResult<object>(queryStr, commandType, parameters);
        }

        public T UniqueResult<T>(string queryStr, IDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return UniqueResult<T>(queryStr, commandType, parameters);
        }

        public T UniqueResult<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDataParameter[] parameters = null)
        {
            using (var connection = new SqlConnection(DbConfig.ConnectionString))
            {
                var result = default(T);
                try
                {
                    connection.Open();
                    var cmd = SqlCmd(connection, queryStr, commandType, parameters, DbConfig.CommandTimeout);
                    var scalarResult = cmd.ExecuteScalar();
                    if (scalarResult == DBNull.Value || scalarResult == null) return result;
                    //result = (T)Convert.ChangeType(scalarResult, typeof(T));
                    result = scalarResult.ChangeType<T>();
                    return result;
                }
                catch
                {
                    throw;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public static SqlCommand SqlCmd(SqlConnection connection, string queryStr,
            CommandType commandType = CommandType.StoredProcedure, IDataParameter[] parameters = null,
            int timeoutSecs = 30)
        {
            return SqlCmd(connection, null, queryStr, commandType, parameters, timeoutSecs);
        }
        
        public static SqlCommand SqlCmd(SqlConnection connection, SqlTransaction? transaction, string queryStr, CommandType commandType = CommandType.StoredProcedure, IDataParameter[]? parameters = null, int timeoutSecs = 30)
        {
            var cmd = transaction == null ? new SqlCommand(queryStr, connection)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSecs
            } : new SqlCommand(queryStr, connection, transaction)
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
