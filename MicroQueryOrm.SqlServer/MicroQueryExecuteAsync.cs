using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQueryExecuteAsync
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public Task ExecuteAsync(string queryStr, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _ExecuteAsync(queryStr, commandType: commandType, transaction: transaction, timeoutSecs: timeoutSecs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task ExecuteAsync(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _ExecuteAsync(queryStr, parameters, commandType, transaction, timeoutSecs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task ExecuteAsync<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null) where TParams : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            return _ExecuteAsync(queryStr, dbParams, commandType, transaction, timeoutSecs);
        }
    }
}
