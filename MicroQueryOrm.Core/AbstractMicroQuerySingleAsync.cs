using MicroQueryOrm.Common;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Core
{
    public abstract partial class AbstractMicroQuery : IMicroQuerySingleAsync
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
            return _SingleResultAsync<object>(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
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
            return _SingleResultAsync<TDestination>(queryStr, null, CommandType.Text, transaction, timeoutSecs);
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
            return _SingleResultAsync<TDestination>(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
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
        public abstract Task<TDestination> SingleResultAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
