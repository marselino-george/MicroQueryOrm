using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQueryTransaction
    {
        public IDbTransaction BeginTransaction()
        {
            var connection = new SqlConnection(DbConfig.ConnectionString);
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
