using System.Data;
using System.Collections.Generic;
using MicroQueryOrm.SqlServer.Extensions;
using MicroQueryOrm.Core;
using MicroQueryOrm.Core.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : AbstractMicroQuery
    {
        public MicroQuery(IDatabaseStrategy databaseStrategy) : base(databaseStrategy)
        {
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
        public override DataTable Query<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TParams : class, new()
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
        public override IEnumerable<TDestination> Query<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TParams : class, new()
            //where TDestination : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            var dataTable = _Query(queryStr, dbParams, CommandType.Text, transaction, timeoutSecs);
            return dataTable.Map<TDestination>();
        }
    }
}
