using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using PagedList;
using System.ComponentModel;
using System.Web.Mvc;

namespace CompareApps.Models
{


    public class CompanyDetailsModel
    {
        [Key]
        public int Id { get; set; }
       
        [StringLength(100, ErrorMessage = "Fault Name must be under 100 characters")]
        public string Name { get; set; }
        
        public string Info { get; set; }

        [UIHint("ImageUpload")]
        [DisplayName("Logo*")]
        [SortableField(false)]
        public string Logo { get; set; }
        public int Page { get; set; }
        public Guid UserId { get; set; }
        public IPagedList<ApplicationModel> Applications { get; set; }

     
        [SortableField(false)]
        [DisplayName("Facebook company link")]
        [StringLength(250, ErrorMessage = "Fault InfoUrlFacebook must be under 250 characters")]
        public string InfoUrlFacebook { get; set; }

      
        [SortableField(false)]
        [DisplayName("Twitter company link")]
        [StringLength(250, ErrorMessage = "Fault InfoUrlTwitter must be under 250 characters")]
        public string InfoUrlTwitter { get; set; }

        public void ImportFrom(Company entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Logo = entity.Logo;
            Info = entity.Info;
        }

        public void ExportTo(Company entity)
        {
            entity.Name = Name;
            entity.Info = Info;
            entity.Logo = Logo;
          
        }
       
    }

    
}