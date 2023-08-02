using System;

namespace MicroQueryOrm.SqlServer
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
