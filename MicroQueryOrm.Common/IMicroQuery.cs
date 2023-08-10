
using System;
using System.Collections.Generic;
using System.Data;

namespace MicroQueryOrm.Common
{
    public interface IMicroQuery
    {
        DataTable Query(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void Query(string queryStr, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null);
        DataTable Query(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null);
        void Query(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null);
        IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null) where TDestination : class, new();
        IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null) where TDestination : class, new();
        DataTable Query<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null) where TParams : class, new();
        IEnumerable<TDestination> Query<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
