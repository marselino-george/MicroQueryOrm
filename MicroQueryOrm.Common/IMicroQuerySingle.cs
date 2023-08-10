using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQuerySingle
    {
        object SingleResult(string queryStr, IDbDataParameter[]? parameters = null, IDbTransaction? transaction = null, int? timeoutSecs = null);
        TDestination SingleResult<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        TDestination SingleResult<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        TDestination SingleResult<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
    }
}
