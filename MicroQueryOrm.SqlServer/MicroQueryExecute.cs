using System.Data;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : AbstractMicroQuery, IMicroQueryExecute
    {
        /// <summary>
        /// Executes a text query such as Insert, Update, Delete with parameters as a class object of TParams that wont get any results back.
        /// </summary>
        /// <typeparam name="TParams"></typeparam>
        /// <param name="queryStr"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="transaction"></param>
        /// <param name="timeoutSecs"></param>
        public override void Execute<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TParams : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            _Execute(queryStr, dbParams, commandType, transaction, timeoutSecs);
        }
    }
}
