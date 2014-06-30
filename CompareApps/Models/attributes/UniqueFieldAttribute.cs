using System;

namespace CompareApps.Models
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class UniqueFieldAttribute : Attribute
    {
        public UniqueFieldAttribute()
        {
        }
    }
}