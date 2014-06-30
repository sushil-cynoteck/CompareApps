
namespace CompareApps.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.ObjectModel;
    public class LookupFieldAttribute : Attribute
    {
        public Type LookupEntityType { get; private set; }
        public bool IncludeEmptyValue { get; private set; }

        public LookupFieldAttribute(Type lookupEntityType, bool includeEmptyValue = false)
        {
            LookupEntityType = lookupEntityType;
            IncludeEmptyValue = includeEmptyValue;
        }
    }

}
