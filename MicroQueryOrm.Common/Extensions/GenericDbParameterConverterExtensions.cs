using System;
using System.Data;
using System.Linq;
using static MicroQueryOrm.Common.Extensions.ReflectionExtensions;

namespace MicroQueryOrm.Common.Extensions
{
    public static class GenericDbParameterConverterExtensions
    {
        public static IDbDataParameter[] ToDbParams<T>(this T container, Func<ReflectedClassProp, IDbDataParameter> createParameter) where T : class, new()
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            var classProps = container.ClassPropertiesWithValues().ToList();
            var dataParams = new IDbDataParameter[classProps.Count];
            var counter = 0;
            foreach (var classProp in classProps)
            {
                var dbParameter = createParameter(classProp);
                dataParams[counter++] = dbParameter;
            }
            return dataParams;
        }
    }
}
