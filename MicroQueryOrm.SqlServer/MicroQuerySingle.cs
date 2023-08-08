using System;
using System.Data;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common.Extensions;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery : IMicroQuerySingle
    {
        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. Additional columns or rows are ignored.
        /// </summary>
        /// <param name="queryStr"></param>
        /// <param name="commandType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object SingleResult(string queryStr, IDbDataParameter[]? parameters = null, CommandType commandType = CommandType.StoredProcedure)
        {
            return SingleResult<object>(queryStr, commandType, parameters);
        }

        public T SingleResult<T>(string queryStr, IDbDataParameter[] parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            return SingleResult<T>(queryStr, commandType, parameters);
        }

        public T SingleResult<T>(string queryStr, CommandType commandType = CommandType.StoredProcedure, IDbDataParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(DbConfig.ConnectionString);
            try
            {
                connection.Open();
                using var cmd = SqlCmd(connection, queryStr, commandType, parameters, DbConfig.CommandTimeout);
                var scalarResult = cmd.ExecuteScalar();
                if (scalarResult == DBNull.Value || scalarResult == null) return default(T);
                var result = scalarResult.ChangeType<T>();
                return result;
            }
            catch
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
