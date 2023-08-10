using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Common;
using MicroQueryOrm.SqlServer.Extensions;

namespace MicroQueryOrm.SqlServer
{
    /// <summary>
    /// MicroQueryStoredProcedureAsync
    /// </summary>
    public partial class MicroQuery : IMicroQueryStoredProcedureAsync
    {
        public async Task<DataTable> StoredProcedureAsync(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters: null, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public async Task<DataTable> StoredProcedureAsync(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public async Task<DataTable> StoredProcedureAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            return await _QueryAsync(queryStr, dbParams, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public async Task<IEnumerable<TDestination>> StoredProcedureAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            var dataTable = _QueryAsync(queryStr, commandType: CommandType.StoredProcedure, transaction: transaction, timeoutSecs: timeoutSecs);
            return await dataTable.MapAsync<TDestination>();
        }

        public async Task<IEnumerable<TDestination>> StoredProcedureAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new()
        {
            IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
            var dataTable = _QueryAsync(queryStr, dbParams, CommandType.Text, transaction, timeoutSecs);
            return await dataTable.MapAsync<TDestination>();
        }
    }
}
