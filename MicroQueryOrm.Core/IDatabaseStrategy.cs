using MicroQueryOrm.Common;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Core
{
    public abstract class AbstractDatabaseStrategy : IDatabaseStrategy
    {
        protected readonly IDataBaseConfiguration _dbConfig;
        public AbstractDatabaseStrategy(IDataBaseConfiguration dbConfig)
        {
            if (dbConfig == null)
                throw new ArgumentNullException(nameof(dbConfig));
            if (string.IsNullOrWhiteSpace(dbConfig.ConnectionString))
                throw new ArgumentNullException(nameof(dbConfig.ConnectionString));
            if (dbConfig.CommandTimeout < 1)
                throw new ArgumentOutOfRangeException(nameof(dbConfig.CommandTimeout));

            _dbConfig = dbConfig;
        }

        public abstract Task CloseConnectionAsync(IDbConnection dbConnection);

        public abstract IDbCommand CreateCommand(IDbConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType, IDbDataParameter[]? parameters, int? timeoutSecs);

        public abstract IDbConnection CreateConnection(IDataBaseConfiguration dbConfig);
        
        public IDataBaseConfiguration DbConfig()
        {
            return _dbConfig;
        }

        public abstract Task<int> ExecuteNonQueryAsync(IDbCommand command);

        public abstract Task<IDataReader> ExecuteReaderAsync(IDbCommand command);

        public abstract Task<object?> ExecuteScalarAsync(IDbCommand command);

        public abstract (IDbConnection, IDbTransaction?) GetConnectionTransaction(IDbTransaction? transaction = null);

        public abstract Task<(IDbConnection, IDbTransaction?)> GetConnectionTransactionAsync(IDbTransaction? transaction = null);
    }

    public interface IDatabaseStrategy
    {
        IDataBaseConfiguration DbConfig();
        (IDbConnection, IDbTransaction?) GetConnectionTransaction(IDbTransaction? transaction = null);
        Task<(IDbConnection, IDbTransaction?)> GetConnectionTransactionAsync(IDbTransaction? transaction = null);
        IDbConnection CreateConnection(IDataBaseConfiguration dbConfig);
        IDbCommand CreateCommand(IDbConnection connection, IDbTransaction? transaction, string queryStr, CommandType commandType, IDbDataParameter[]? parameters, int? timeoutSecs);
        Task<IDataReader> ExecuteReaderAsync(IDbCommand command);
        Task<int> ExecuteNonQueryAsync(IDbCommand command);
        Task<object?> ExecuteScalarAsync(IDbCommand command);
        Task CloseConnectionAsync(IDbConnection dbConnection);
    }
}
