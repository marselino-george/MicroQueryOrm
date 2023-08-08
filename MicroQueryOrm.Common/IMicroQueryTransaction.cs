using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQueryTransaction
    {
        IDbTransaction BeginTransaction();
        void CommitTransaction(IDbTransaction transaction);
        void RollbackTransaction(IDbTransaction transaction);
    }
}
