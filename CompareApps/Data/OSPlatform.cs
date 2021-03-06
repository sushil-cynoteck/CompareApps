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
    
    public partial class OSPlatform
    {
        public OSPlatform()
        {
            this.Advertisements = new HashSet<Advertisement>();
            this.Applications_OSPlatforms = new HashSet<Applications_OSPlatforms>();
            this.OSVersions = new HashSet<OSVersion>();
            this.PhoneBrands = new HashSet<PhoneBrand>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public int Order { get; set; }
    
        public virtual ICollection<Advertisement> Advertisements { get; set; }
        public virtual ICollection<Applications_OSPlatforms> Applications_OSPlatforms { get; set; }
        public virtual ICollection<OSVersion> OSVersions { get; set; }
        public virtual ICollection<PhoneBrand> PhoneBrands { get; set; }
    }
}
