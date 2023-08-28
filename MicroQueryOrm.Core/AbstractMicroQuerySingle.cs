using MicroQueryOrm.Common;
using System;
using System.Data;

namespace MicroQueryOrm.Core
{
    public partial class AbstractMicroQuery : IMicroQuerySingle
    {
        /// <summary>
        /// Executes a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object SingleResult(string queryStr, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResult<object>(queryStr, parameters, CommandType.Text,  transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TDestination SingleResult<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResult<TDestination>(queryStr, null, CommandType.Text, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public TDestination SingleResult<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _SingleResult<TDestination>(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a query and returns the first column of the first row. Additional columns or rows are ignored.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public abstract TDestination SingleResult<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
