using System;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core;

namespace MicroQueryOrm.SqlLite
{
    public class SqliteStrategy : AbstractDatabaseStrategy
    {
        public SqliteStrategy(IDataBaseConfiguration dbConfig) : base(dbConfig)
        {}

        public override async Task CloseConnectionAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqliteConnection sqliteConnection)
            {
                await sqliteConnection.CloseAsync();
            }
            else
            {
                dbConnection.Close();
            }
        }

        public override IDbCommand CreateCommand(IDbConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType, IDbDataParameter[]? parameters, int? timeoutSecs)
        {
            var cmd = new SqliteCommand(queryStr, connection as SqliteConnection, transaction as SqliteTransaction)
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
            SqliteConnection connection = new SqliteConnection(_dbConfig.ConnectionString);
            connection.Open();
            return connection;
        }

        public override async Task<int> ExecuteNonQueryAsync(IDbCommand command)
        {
            if (command is SqliteCommand sqliteCommand)
            {
                return await sqliteCommand.ExecuteNonQueryAsync();
            }
            throw new InvalidOperationException("Command is not of type SqliteCommand.");
        }

        public override async Task<IDataReader> ExecuteReaderAsync(IDbCommand command)
        {
            if (command is SqliteCommand sqliteCommand)
            {
                return await sqliteCommand.ExecuteReaderAsync();
            }
            throw new InvalidOperationException("Command is not of type SqliteCommand.");
        }

        public override async Task<object?> ExecuteScalarAsync(IDbCommand command)
        {
            if (command is SqliteCommand sqliteCommand)
            {
                return await sqliteCommand.ExecuteScalarAsync();
            }
            throw new InvalidOperationException("Command is not of type SqliteCommand.");
        }

        public override (IDbConnection, IDbTransaction?) GetConnectionTransaction(IDbTransaction? transaction = null)
        {
            SqliteTransaction? dbTransaction = transaction as SqliteTransaction;
            SqliteConnection connection = dbTransaction?.Connection as SqliteConnection ?? new SqliteConnection(_dbConfig.ConnectionString);
            if (dbTransaction == null)
            {
                connection.Open();
            }
            return (connection, dbTransaction);
        }

        public override async Task<(IDbConnection, IDbTransaction?)> GetConnectionTransactionAsync(IDbTransaction? transaction = null)
        {
            SqliteTransaction? dbTransaction = transaction as SqliteTransaction;
            SqliteConnection connection = dbTransaction?.Connection as SqliteConnection ?? new SqliteConnection(_dbConfig.ConnectionString);
            if (dbTransaction == null)
            {
                await connection.OpenAsync();
            }
            return (connection, dbTransaction);
        }
    }
}
