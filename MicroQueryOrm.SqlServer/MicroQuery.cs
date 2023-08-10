using System;
using System.Data;
using System.Collections.Generic;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQuery
    {
        public void Query(string queryStr, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, commandType: CommandType.Text, transaction: transaction, timeoutSecs: timeoutSecs);
        }

        public void Query(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        public DataTable Query(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, commandType: CommandType.Text, transaction: transaction, timeoutSecs: timeoutSecs);
        }

        public DataTable Query(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        public IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            return _Query(queryStr, commandType: CommandType.Text, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout).Map<TDestination>();
        }

        public IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            return _Query(queryStr, parameters, CommandType.Text, transaction, DbConfig.CommandTimeout).Map<TDestination>();
        }

        /// <summary>
        /// Executes a text query accepting parameters as a class object of TParams and returns the result as a DataTable.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public DataTable Query<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            return _Query(queryStr, dbParams, CommandType.Text, transaction, timeoutSecs);
        }

        /// <summary>
        /// Executes a text query accepting parameters as a class object of TParams and returns the result as a IEnumerable mapped to TDestination.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        /// <returns></returns>
        public IEnumerable<TDestination> Query<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            var dataTable = _Query(queryStr, dbParams, CommandType.Text, transaction, timeoutSecs);
            return dataTable.Map<TDestination>();
        }
    }
}
