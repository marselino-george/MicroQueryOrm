using System;

namespace MicroQueryOrm.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// Name of the column in DB that represents this property.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new instance of ColumnAttribute.
        /// </summary>
        /// <param name="name">Name of the column in DB that represents this property.</param>
        public ColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
