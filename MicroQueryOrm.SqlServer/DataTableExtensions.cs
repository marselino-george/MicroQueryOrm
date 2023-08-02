using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace MicroQueryOrm.SqlServer
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Converts all the rows of the DataTable to a collection of arbitrary objects.
        /// In order for the mapping to work, the properties of the TDestination class must have the ColumnAttribute attribute.
        /// </summary>
        public static IEnumerable<TDestination> Map<TDestination>(this DataTable table) where TDestination : class, new()
        {
            //var result = new List<TDestination>();
            //foreach (DataRow row in table.Rows)
            //{
            //    var f = Map<TDestination>(row);
            //    result.Add(f);
            //}
            //return result;
            return from DataRow row in table.Rows select Map<TDestination>(row);
        }

        /// <summary>
        /// Converts the DataRow to an arbitrary object.
        /// In order for the mapping to work, the properties of the TDestination class must have the ColumnAttribute attribute.
        /// </summary>
        public static TDestination Map<TDestination>(this DataRow row) where TDestination : class, new()
        {
            return DataRowMapper<TDestination>.Map(row);
        }

        /// <summary>
        /// Helper class to cache the object mapping.
        /// </summary>
        private static class DataRowMapper<TDestination> where TDestination : class, new()
        {
            // Lambdas that assign 1 of the DataRow columns to the TDestination object
            private static readonly List<Action<DataRow, TDestination>> PropertyMappers;
            // DEBUG: Helper to show the lambda that failed, in case a conversion fails.
            private static readonly List<string> PropertyMappersDebugNames;
            // DEBUG: Helper to get the original object of the row, in case a conversion fails.
            private static readonly List<Func<DataRow, object>> RowOriginalValue;

            /// <summary>
            /// Static Constructor.
            /// </summary>
            static DataRowMapper()
            {
                PropertyMappers = new List<Action<DataRow, TDestination>>();
                PropertyMappersDebugNames = new List<string>();
                RowOriginalValue = new List<Func<DataRow, object>>();

                // Obtains the indexer of the DataRow (i.e. the property "Object row[string columName]")
                var datarowIndexer = typeof(DataRow).GetProperty("Item", new[] { typeof(string) });
                var isNullMethod = typeof(DataRow).GetMethod("IsNull", new[] { typeof(string) });

                // The goal is to create a lambda for each property of the class that implements ColumnAttribute,
                // that transfers the value of a DataRow column to a property of the destination object.
                // For example for the property item.Name, of type string, the lambda is created:
                // (row, item) => item.Name = (string)Convert.ChangeType(row["NAME"], typeof(string))
                // For nullable types, for example item.Amount, of type int? (or Nulleable<Int32>):
                // (row, item) => item.Amount = row.IsNull("NUMBER") ? 
                //                                 (int?)null
                //                                 : (int?)Convert.ChangeType(row["AMOUNT"], typeof(int)) 
                foreach (var propertyInfo in typeof(TDestination).GetProperties())
                {
                    if (propertyInfo.GetGustomAttributesOf<IgnoreAttribute>().Any())
                        continue;

                    var classAttribute = propertyInfo.GetGustomAttributesOf<ColumnAttribute>();

                    string columnDbName = classAttribute.Any()
                        ? ((ColumnAttribute)classAttribute[0]).Name
                        : propertyInfo.Name;

                    var rowParamExpr = Expression.Parameter(typeof(DataRow), "row");
                    var itemParamExpr = Expression.Parameter(typeof(TDestination), "item");

                    var rowAccessorExpr = Expression.MakeIndex(rowParamExpr, datarowIndexer,
                        new[] { Expression.Constant(columnDbName) });

                    Expression convertExpr;
                    // Nullable<T> types have a special handling: you can not apply Convert.ChangeType() with a typeof(Nullable<T>).
                    var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                    if (underlyingType != null)
                    {
                        convertExpr = Expression.Condition(
                            Expression.Call(rowParamExpr, isNullMethod, Expression.Constant(columnDbName)),
                            Expression.Constant(null, propertyInfo.PropertyType),
                            Expression.Convert(
                                Expression.Call(typeof(Convert), "ChangeType", null, rowAccessorExpr, Expression.Constant(underlyingType)),
                                propertyInfo.PropertyType)
                        );
                    }
                    else
                    {
                        convertExpr = Expression.Call(typeof(Convert), "ChangeType", null, rowAccessorExpr,
                            Expression.Constant(propertyInfo.PropertyType));

                        convertExpr = Expression.Convert(convertExpr, propertyInfo.PropertyType);
                    }

                    var propertyExpr = Expression.Property(itemParamExpr, propertyInfo);
                    var assignExpr = Expression.Assign(propertyExpr, convertExpr);

                    var lambda = Expression.Lambda<Action<DataRow, TDestination>>(assignExpr, rowParamExpr, itemParamExpr);

                    PropertyMappers.Add(lambda.Compile());
                    PropertyMappersDebugNames.Add(lambda.ToString());
                    RowOriginalValue.Add(Expression.Lambda<Func<DataRow, object>>(rowAccessorExpr, rowParamExpr).Compile());
                }

                if (!PropertyMappers.Any())
                {
                    throw new ArgumentException(
                        $"The object of type {typeof(TDestination).FullName} must have at least one property with attribute [ColumnAttribute]");
                }
            }

            public static TDestination Map(DataRow row)
            {
                var item = new TDestination();
                for (var i = 0; i < PropertyMappers.Count; i++)
                {
                    try
                    {
                        // Map the property
                        PropertyMappers[i](row, item);
                    }
                    catch (Exception ex)
                    {
                        // Obtain extra diagnostic info if possible:
                        object? rowValue = null;
                        try
                        {
                            rowValue = RowOriginalValue[i](row);
                        }
                        catch { /* Couldn't retrieve the original value. Is the column name valid? */ }

                        throw new InvalidCastException(
                            $"Error performing mapping for the type {typeof(TDestination).Name}, see inner exception.\n" +
                            $"Mapping expression: {PropertyMappersDebugNames[i]}.\n" +
                            $"Original row type: {rowValue?.GetType()}.\n" +
                            $"Original row value: {rowValue}.", ex);
                    }
                }

                return item;
            }
        }
    }
}
