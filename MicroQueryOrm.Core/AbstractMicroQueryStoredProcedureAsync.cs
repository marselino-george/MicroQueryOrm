using MicroQueryOrm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace MicroQueryOrm.Core
{
    /// <summary>
    /// AbstractMicroQueryStoredProcedureAsync
    /// </summary>
    public partial class AbstractMicroQuery : IMicroQueryStoredProcedureAsync
    {
        public async Task<DataTable> StoredProcedureAsync(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters: null, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public async Task<DataTable> StoredProcedureAsync(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return await _QueryAsync(queryStr, parameters, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public abstract Task<DataTable> StoredProcedureAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();

        public abstract Task<IEnumerable<TDestination>> StoredProcedureAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new();


        public abstract Task<IEnumerable<TDestination>> StoredProcedureAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
