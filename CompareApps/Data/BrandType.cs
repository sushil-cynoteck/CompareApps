//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CompareApps.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class BrandType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PhoneBrandRef { get; set; }
        public bool MarkForDeletion { get; set; }
    
        public virtual PhoneBrand PhoneBrand { get; set; }
    }
}