
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Common
{
    public interface IMicroQueryStoredProcedureAsync
    {
        Task<DataTable> StoredProcedureAsync(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<DataTable> StoredProcedureAsync(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<DataTable> StoredProcedureAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
        Task<IEnumerable<TDestination>> StoredProcedureAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new();
        Task<IEnumerable<TDestination>> StoredProcedureAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
