using System;
using System.Data;
using System.Collections.Generic;
using MicroQueryOrm.Common;
using MicroQueryOrm.Core.Extensions;

namespace MicroQueryOrm.Core
{
    public abstract partial class AbstractMicroQuery : IMicroQuery
    {
        public void Query(string queryStr, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, commandType: CommandType.Text, transaction: transaction, timeoutSecs: timeoutSecs);
        }

        public void Query(string queryStr, IDbDataParameter[] parameters, Action<IDataReader> readerAction, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            _Query(queryStr, readerAction, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        public DataTable Query(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, commandType: CommandType.Text, transaction: transaction, timeoutSecs: timeoutSecs);
        }

        public DataTable Query(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            return _Query(queryStr, parameters, CommandType.Text, transaction, timeoutSecs);
        }

        public IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            return _Query(queryStr, commandType: CommandType.Text, transaction: transaction, timeoutSecs: _databaseStrategy.DbConfig().CommandTimeout).Map<TDestination>();
        }

        public IEnumerable<TDestination> Query<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TDestination : class, new()
        {
            return _Query(queryStr, parameters, CommandType.Text, transaction, _databaseStrategy.DbConfig().CommandTimeout).Map<TDestination>();
        }

        public abstract DataTable Query<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null) where TParams : class, new();

        public abstract IEnumerable<TDestination> Query<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
            where TParams : class, new()
            where TDestination : class, new();
    }
}
