using System;
using System.Data;
using System.Threading.Tasks;
using MicroQueryOrm.Core;
using System.Collections.Generic;

namespace MicroQueryOrm.SqlLite
{
    public class MicroQuery : AbstractMicroQuery
    {
        public MicroQuery(IDatabaseStrategy databaseStrategy) : base(databaseStrategy)
        {
            SQLitePCL.Batteries.Init();
        }

        public override void Execute<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task ExecuteAsync<TParams>(string queryStr, TParams parameters, CommandType commandType = CommandType.Text, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override DataTable Query<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TDestination> Query<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DataTable> QueryAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TDestination>> QueryAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override TDestination SingleResult<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<TDestination> SingleResultAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TDestination> StoredProcedure<TDestination>(string queryStr, IDbDataParameter[] parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override DataTable StoredProcedure<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<TDestination> StoredProcedure<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<DataTable> StoredProcedureAsync<TParams>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TDestination>> StoredProcedureAsync<TDestination>(string queryStr, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }

        public override Task<IEnumerable<TDestination>> StoredProcedureAsync<TParams, TDestination>(string queryStr, TParams parameters, IDbTransaction? transaction = null, int? timeoutSecs = null)
        {
            throw new NotImplementedException();
        }
    }
}
