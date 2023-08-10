
using System.Data;

namespace MicroQueryOrm.Common
{
    /// <summary>
    /// Queries or Stored Procedures that do not return a result set
    /// </summary>
    public interface IMicroQueryExecute
    {
        void Execute(string queryStr, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void Execute(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void Execute<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
