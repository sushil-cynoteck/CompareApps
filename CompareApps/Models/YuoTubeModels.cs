using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Google.GData.Client;
using Google.YouTube;

namespace CompareApps.Models
{
    public class YuoTubeUploadModel 
    {
        public string PostUrl { get; set; }
        public string NextUrl { get; set; }
        public string Token { get; set; }
        [Required]
        public string Source { get; set; }
        public YuoTubeCreateVideoModel YuoTubeCreateVideoModel { get; set; }
    }

    public class YuoTubeCreateVideoModel 
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = @"Enter tags separated by ','")]
        public string Keywords { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Description { get; set; }
        
    }

    public class YuoTubeSuccessModel
    {
        public string Id { get; set; }
        public string Massage { get; set; }
     
    }

    public class YuoTubeListModel
    {
        public Feed<Video> Videos { get; set; }
       
    }
}
