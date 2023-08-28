using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core;

namespace MicroQueryOrm.SqlServer
{
    public sealed class SqlServerStrategy : AbstractDatabaseStrategy
    {
        public SqlServerStrategy(IDataBaseConfiguration dbConfig) : base(dbConfig)
        {}

        public override async Task CloseConnectionAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection sqlConnection)
            {
                await sqlConnection.CloseAsync();
            }
            else
            {
                dbConnection.Close();
            }
        }

        public override IDbCommand CreateCommand(IDbConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType, IDbDataParameter[]? parameters, int? timeoutSecs)
        {
            var cmd = new SqlCommand(queryStr, connection as SqlConnection, transaction as SqlTransaction)
            {
                CommandType = commandType,
                CommandTimeout = timeoutSecs ?? _dbConfig.CommandTimeout
            };

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }

        public override IDbConnection CreateConnection(IDataBaseConfiguration dbConfig)
        {
            SqlConnection connection = new SqlConnection(_dbConfig.ConnectionString);
            connection.Open();
            return connection;
        }

        public override async Task<int> ExecuteNonQueryAsync(IDbCommand command)
        {
            if (command is SqlCommand sqlCommand)
            {
                return await sqlCommand.ExecuteNonQueryAsync();
            }
            throw new InvalidOperationException("Command is not of type SqlCommand.");
        }

        public override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command)
        {
            if (command is SqlCommand sqlCommand)
            {
                return await sqlCommand.ExecuteReaderAsync();
            }
            throw new InvalidOperationException("Command is not of type SqlCommand.");
        }

        public override async Task<object?> ExecuteScalarAsync(IDbCommand command)
        {
            if (command is SqlCommand sqlCommand)
            {
                return await sqlCommand.ExecuteScalarAsync();
            }
            throw new InvalidOperationException("Command is not of type SqlCommand.");
        }

        public override (IDbConnection, IDbTransaction?) GetConnectionTransaction(IDbTransaction? transaction = null)
        {
            SqlTransaction? dbTransaction = transaction as SqlTransaction;
            SqlConnection connection = dbTransaction?.Connection as SqlConnection ?? new SqlConnection(_dbConfig.ConnectionString);
            if (dbTransaction == null)
            {
                connection.Open();
            }
            return (connection, dbTransaction);
        }

        public override async Task<(IDbConnection, IDbTransaction?)> GetConnectionTransactionAsync(IDbTransaction? transaction = null)
        {
            SqlTransaction? dbTransaction = transaction as SqlTransaction;
            SqlConnection connection = dbTransaction?.Connection as SqlConnection ?? new SqlConnection(_dbConfig.ConnectionString);
            if (dbTransaction == null)
            {
                await connection.OpenAsync();
            }
            return (connection, dbTransaction);
        }
    }
}
