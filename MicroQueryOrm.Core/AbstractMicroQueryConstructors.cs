using System;
using MicroQueryOrm.Common;

namespace MicroQueryOrm.Core
{
    /// <summary>
    /// MicroQuery's Constructor(s)
    /// </summary>
    public abstract partial class AbstractMicroQuery
    {
        protected readonly IDatabaseStrategy _databaseStrategy;

        public AbstractMicroQuery(IDatabaseStrategy databaseStrategy)
        {
            _databaseStrategy = databaseStrategy;
        }
    }
}
