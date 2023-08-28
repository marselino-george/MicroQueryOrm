using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core.Extensions;

namespace MicroQueryOrm.Core
{
    /// <summary>
    /// AbstractMicroQueryAsync: Contains all the async methods to execute text queries.
    /// </summary>
    public abstract partial class AbstractMicroQuery : IMicroQueryAsync
    {
        /// <summary>
        /// Executes a text query asynchronously without parameters and returns the result as a DataTable.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryAsync(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters: null, CommandType.Text, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a text query asynchronously accepting parameters as IDbDataParameter[] and returns the result as a DataTable.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public async Task<DataTable> QueryAsync(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a text query asynchronously accepting parameters as a class object of TParams and returns the result as a DataTable.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public abstract Task<DataTable> QueryAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();

        /// <summary>
        /// Executes a text query asynchronously returns the result as a IEnumerable mapped to TDestination.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TDestination>> QueryAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            var dataTable = _QueryAsync(queryStr, commandType: CommandType.Text, transaction: transaction, timeoutSecs: timeoutSecs);
            return await dataTable.MapAsync<TDestination>();
        }

        /// <summary>
        /// Executes a text query asynchronously accepting parameters as a class object of TParams and returns the result as a IEnumerable mapped to TDestination.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public abstract Task<IEnumerable<TDestination>> QueryAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
