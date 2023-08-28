using System.Collections.Generic;
using System.Data;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core;
using MicroQueryOrm.Core.Extensions;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    /// <summary>
    /// MicroQueryStoredProcedure
    /// </summary>
    public partial class MicroQuery : AbstractMicroQuery, IMicroQueryStoredProcedure
    {
        public override IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TDestination : class, new()
        {
            return _Query(queryStr, commandType: CommandType.StoredProcedure, transaction: transaction, timeoutSecs: _databaseStrategy.DbConfig().CommandTimeout).Map<TDestination>();
        }

        public override IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TDestination : class, new()
        {
            return _Query(queryStr, parameters, CommandType.StoredProcedure, transaction, _databaseStrategy.DbConfig().CommandTimeout).Map<TDestination>();
        }

        public override DataTable StoredProcedure<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            //where TParams : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            return _Query(queryStr, dbParams, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public override IEnumerable<TDestination> StoredProcedure<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            // where TParams : class, new()
            // where TDestination : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            var dataTable = _Query(queryStr, dbParams, CommandType.StoredProcedure, transaction, timeoutSecs);
            return dataTable.Map<TDestination>();
        }
    }
}
