using System;
using System.Linq;
using System.Reflection;

namespace MicroQueryOrm.SqlServer
{
    public static class ReflectionUtil
    {
        public static bool IsImplementingClass<T>(this object[] classAttributes)
        {
            return classAttributes.Any();
        }

        public static object[] GetGustomAttributesOf<T>(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false);
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
    }
}
