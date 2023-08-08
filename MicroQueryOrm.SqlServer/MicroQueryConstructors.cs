using System;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.SqlServer
{
    public partial class MicroQuery
    {
        protected readonly IDataBaseConfiguration DbConfig;

        public MicroQuery(IDataBaseConfiguration dbConfig)
        {
            if (dbConfig.CommandTimeout < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(dbConfig.CommandTimeout));
            }
            DbConfig = dbConfig;
        }
    }
}
