using System.Data;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : AbstractMicroQuery, IMicroQuerySingle
    {
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
        public override TDestination SingleResult<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TParams : class, new()
        {
            var dbParams = parameters.ToSqlParams<TParams>();
            return _SingleResult<TDestination>(queryStr, dbParams, CommandType.Text, transaction, timeoutSecs);
        }
    }
}
