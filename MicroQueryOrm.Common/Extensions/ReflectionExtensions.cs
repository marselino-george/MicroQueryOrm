using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace MicroQueryOrm.Common.Extensions
{
    public static class ReflectionExtensions
    {
        public sealed class ReflectedClassProp
        {
            public string Name { get; set; }
            public object Value { get; set; }
            public Type Type { get; set; }
        }

        public static IEnumerable<ReflectedClassProp> ClassPropertiesWithValues<T>(this T objSource) where T : class, new()
        {
            var propsValues = from prop in objSource.GetType().GetProperties()
                              select new ReflectedClassProp
                              {
                                  Name = prop.Name,
                                  Value = prop.GetValue(objSource, null),
                                  Type = prop.PropertyType
                              };
            return propsValues;
        }

        public static T ChangeType<T>(this object value)
        {
            var t = typeof(T);

            if (!t.IsGenericType || t.GetGenericTypeDefinition() != typeof(Nullable<>))
                return (T)Convert.ChangeType(value, t);

            if (value == null)
            {
                return default(T);
            }

            t = Nullable.GetUnderlyingType(t);

            return (T)Convert.ChangeType(value, t);
        }

        public static object[] GetGustomAttributesOf<T>(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false);
        }
    }
}
