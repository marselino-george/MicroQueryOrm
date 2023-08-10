using System;
using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQuerySingleAsync
    {
        /// <summary>
        /// Executes asynchronously a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<object> SingleResultAsync(string queryStr, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResultAsync<object>(queryStr, parameters, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes asynchronously a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TDestination> SingleResultAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResultAsync<TDestination>(queryStr, null, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes asynchronously a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TDestination> SingleResultAsync<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResultAsync<TDestination>(queryStr, parameters, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes asynchronously a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<TDestination> SingleResultAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
        {
            var dbParams = parameters.ToSqlParams<TParams>();
            return _SingleResultAsync<TDestination>(queryStr, dbParams, transaction, timeoutSecs);
        }
    }
}
