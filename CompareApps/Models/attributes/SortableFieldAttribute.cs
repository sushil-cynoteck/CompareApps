namespace CompareApps.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.ObjectModel;
    public class SortableFieldAttribute : Attribute
    {
        public bool Show { get; private set; }
        public SortableFieldAttribute(bool show)
        {
            Show = show;
        }
    }

}
