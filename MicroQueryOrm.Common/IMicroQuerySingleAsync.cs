using System.Data;
using System.Threading.Tasks;

namespace MicroQueryOrm.Common
{
    public interface IMicroQuerySingleAsync
    {
        Task<object> SingleResultAsync(string queryStr, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<TDestination> SingleResultAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<TDestination> SingleResultAsync<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        Task<TDestination> SingleResultAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
