
using System;
using System.Collections.Generic;
using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQueryStoredProcedure
    {
        DataTable StoredProcedure(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void StoredProcedure(string queryStr, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null);
        DataTable StoredProcedure(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void StoredProcedure(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null);
        IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null) where TDestination : class, new();
        IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null) where TDestination : class, new();
        DataTable StoredProcedure<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null) where TParams : class, new();
        IEnumerable<TDestination> StoredProcedure<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
