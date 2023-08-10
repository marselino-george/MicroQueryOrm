using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Common
{
    /// <summary>
    /// Asynchromous queries or Stored Procedures that do not return a result set
    /// </summary>
    public interface IMicroQueryExecuteAsync
    {
        Task ExecuteAsync(string queryStr, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task ExecuteAsync(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task ExecuteAsync<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
