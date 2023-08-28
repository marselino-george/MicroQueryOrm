using System;
using System.Data;
using System.Linq;

namespace MicroQueryOrm.SqlServer.Extensions
{
    /// <summary>
    /// Contains methods to create or modify IDbDataParameter[].
    /// </summary>
    public static class DbParameterConverterExtensions
    {
        public static IDbDataParameter[] ExtendParameter<T>(this IDbDataParameter[] dataParameters, string name, Action<T> modifyAction) where T : class, new()
        {
            var p = (T)dataParameters.SingleOrDefault(param => param.ParameterName == name);
            modifyAction(p);
            return dataParameters;
        }

        public static IDbDataParameter[] RemoveParameter(this IDbDataParameter[] dataParameters, string name)
        {
            var newParameters = dataParameters.Where(parameter => parameter.ParameterName != name).ToArray();
            return newParameters;
        }

        public static IDbDataParameter[] RemoveParameter(this IDbDataParameter[] dataParameters, string[] names)
        {
            var newParameters = dataParameters.Where(parameter => !names.Contains(parameter.ParameterName)).ToArray();
            return newParameters;
        }
    }
}
