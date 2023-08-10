using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Common
{
    public interface IMicroQueryAsync
    {
        Task<DataTable> QueryAsync(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<DataTable> QueryAsync(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<DataTable> QueryAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
        Task<IEnumerable<TDestination>> QueryAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new();
        Task<IEnumerable<TDestination>> QueryAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}