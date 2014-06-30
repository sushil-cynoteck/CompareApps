
namespace CompareApps.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.ObjectModel;
    public class ShowFieldInListAttribute : Attribute
    {
        public bool Show { get; private set; }
        public string DisplayName { get; private set; }
        public ShowFieldInListAttribute(bool show)
        {
            Show = show;
            DisplayName = null;
        }
        public ShowFieldInListAttribute(bool show, string displayName)
        {
            Show = show;
            DisplayName = displayName;
        }
    }

}
