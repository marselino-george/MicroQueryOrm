using MicroQueryOrm.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MicroQueryOrm.Core
{
    /// <summary>
    /// AbstractMicroQueryStoredProcedure
    /// </summary>
    public partial class AbstractMicroQuery : IMicroQueryStoredProcedure
    {
        public DataTable StoredProcedure(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, parameters: null, CommandType.StoredProcedure, transaction);
        }

        public void StoredProcedure(string queryStr, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, parameters: null, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public void StoredProcedure(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, parameters, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public DataTable StoredProcedure(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, parameters, CommandType.StoredProcedure, transaction, timeoutSecs);
        }

        public abstract IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new();
        //{
        //    return _Query(queryStr, commandType: CommandType.StoredProcedure, transaction: transaction, timeoutSecs: DbConfig.CommandTimeout).Map<TDestination>();
        //}

        public abstract IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new();
        //{
        //    return _Query(queryStr, parameters, CommandType.StoredProcedure, transaction, DbConfig.CommandTimeout).Map<TDestination>();
        //}

        public abstract DataTable StoredProcedure<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new();
        //{
        //    IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
        //    return _Query(queryStr, dbParams, CommandType.StoredProcedure, transaction, timeoutSecs);
        //}

        public abstract IEnumerable<TDestination> StoredProcedure<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
        //{
        //    IDbDataParameter[] dbParams = parameters.ToSqlParams<TParams>();
        //    var dataTable = _Query(queryStr, dbParams, CommandType.StoredProcedure, transaction, timeoutSecs);
        //    return dataTable.Map<TDestination>();
        //}
    }
}