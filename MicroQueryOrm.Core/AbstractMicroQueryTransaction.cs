using System.Data;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.Core
{
    public abstract partial class AbstractMicroQuery : IMicroQueryTransaction
    {
        public IDbTransaction BeginTransaction()
        {
            var connection = _databaseStrategy.CreateConnection(_databaseStrategy.DbConfig());
            connection.Open();
            return connection.BeginTransaction();
        }

        public void CommitTransaction(IDbTransaction transaction)
        {
            transaction.Commit();
            transaction.Connection.Close();
        }

        public void RollbackTransaction(IDbTransaction transaction)
        {
            transaction.Rollback();
            transaction.Connection.Close();
        }
    }
}
