using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQuerySingle
    {
        object SingleResult(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure);
        T SingleResult<T>(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure);
        T SingleResult<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null);
    }
}
