using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using MicroQueryOrm.Common.Extensions;
using static MicroQueryOrm.Common.Extensions.ReflectionExtensions;

namespace MicroQueryOrm.SqlServer.Extensions
{
    /// <summary>
    /// Contains methods to create or modify IDataParameter[].
    /// </summary>
    public static class SqlParameterConverterExtensions
    {
        public static IDataParameter[] AddSqlParameter(this IDataParameter[] dataParameters, string name, object value)
        {
            var dataParams = new IDataParameter[dataParameters.Length + 1];
            dataParameters.CopyTo(dataParams, 0);
            dataParams[dataParameters.Length] = new SqlParameter(name, value);
            return dataParams;
        }

        public static IDataParameter[] ExtendParameter<T>(this IDataParameter[] dataParameters, string name, Action<T> modifyAction) where T : class, new()
        {
            var p = (T)dataParameters.SingleOrDefault(param => param.ParameterName == name);
            modifyAction(p);
            return dataParameters;
        }

        public static IDataParameter[] RemoveParameter(this IDataParameter[] dataParameters, string name)
        {
            var newParameters = dataParameters.Where(parameter => parameter.ParameterName != name).ToArray();
            return newParameters;
        }

        public static IDataParameter[] RemoveParameter(this IDataParameter[] dataParameters, string[] names)
        {
            var newParameters = dataParameters.Where(parameter => !names.Contains(parameter.ParameterName)).ToArray();
            return newParameters;
        }

        /// <summary>
        /// Converts all the properties of the object to a collection of IDataParameter.
        /// </summary>
        /// <example>
        /// The following Anonymous Object:
        /// var param = new { p_name = "George", p_lastname = "Romero" }.ToSqlParams();
        /// 
        /// It's equivalent to:
        /// var param = new IDataParameter[]
        /// {
        ///     new SqlParameter
        ///     {
        ///         ParameterName = "p_nombre",
        ///         Value = "George"
        ///     },
        ///     new SqlParameter
        ///     {
        ///         ParameterName = "p_lastname",
        ///         Value = "Romero"
        ///     }
        /// };
        /// </example>
        /// <param name="container">Object to convert to IDataParameter[]</param>
        /// <returns></returns>
        public static IDataParameter[] ToSqlParams(this object container)
        {
            return ToSqlParams<object>(container);
        }

        /// <summary>
        /// Converts all the properties of a class (T) to a collection of IDataParameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IDbDataParameter[] ToSqlParams<T>(this T container) where T : class, new()
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            Func<ReflectedClassProp, IDbDataParameter> createSqlParameter = (reflectedClassProp) =>
            {
                var sqlParameter = new SqlParameter(reflectedClassProp.Name, dbType: GetSqlDbType(reflectedClassProp.Type))
                {
                    Value = reflectedClassProp.Value ?? DBNull.Value
                };
                return sqlParameter;
            };

            return GenericDbParameterConverterExtensions.ToDbParams<T>(container, createSqlParameter);
        }

        public static SqlDbType GetSqlDbType(this Type type)
        {
            if (type == typeof(string)) return SqlDbType.NVarChar;
            if (type == typeof(int) || type == typeof(int?)) return SqlDbType.Int;
            if (type == typeof(long) || type == typeof(long?)) return SqlDbType.BigInt;
            if (type == typeof(short) || type == typeof(short?)) return SqlDbType.SmallInt;
            if (type == typeof(byte) || type == typeof(byte?)) return SqlDbType.TinyInt;
            if (type == typeof(DateTime) || type == typeof(DateTime?)) return SqlDbType.DateTime;
            if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?)) return SqlDbType.DateTimeOffset;
            if (type == typeof(TimeSpan) || type == typeof(TimeSpan?)) return SqlDbType.Time;
            if (type == typeof(byte[])) return SqlDbType.VarBinary;
            if (type == typeof(decimal) || type == typeof(decimal?)) return SqlDbType.Decimal;
            if (type == typeof(double) || type == typeof(double?)) return SqlDbType.Float;
            if (type == typeof(float) || type == typeof(float?)) return SqlDbType.Real;
            if (type == typeof(bool) || type == typeof(bool?)) return SqlDbType.Bit;
            if (type == typeof(Guid) || type == typeof(Guid?)) return SqlDbType.UniqueIdentifier;
            if (type == typeof(object)) return SqlDbType.Variant;

            // For other types, throw an exception
            throw new ArgumentException($"Unsupported type: {type.FullName}");
        }
    }
}
