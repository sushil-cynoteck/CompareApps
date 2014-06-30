using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using PagedList;
using Google.YouTube;

namespace CompareApps.Models
{
    public class Applications_OSPlatformListModel
    {
        public IPagedList<Applications_OSPlatformModel> Applications_OSPlatforms { get; set; }
    }

    [DisplayName("Application platforms")]
    [DisplayColumn("Name")]
    public class Applications_OSPlatformModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [DisplayName("Application*")]
        [UIHint("DisabledLookup")]
        [Required]
        public int ApplicationRef { get; set; }

        [DisplayName("OS Platform*")]
        [UIHint("DisabledLookup")]
        [Required]
        public int OSPlatformRef { get; set; }

        [Display(Name = "OS Versions")]
        [UIHint("OSVersions")]
        public string[] OSVersions { get; set; }

        [Display(Name = "Permissions")]
        [UIHint("Permissions")]
        public string Permissions { get; set; }

        [Required]
        [DisplayName("App version*")]
        [StringLength(50, ErrorMessage = "Fault Version must be under 50 characters")]
        public string Version { get; set; }

        [Required]
        [DisplayName("Size* (Kb)")]
        public int Size { get; set; }

        [Required]
        [DisplayName("Market URL*")]
        [StringLength(250, ErrorMessage = "Fault MarketURL must be under 250 characters")]
        public string MarketURL { get; set; }

        [Required]
        [DisplayName("Price USD*")]
        [Range(0, Double.MaxValue, ErrorMessage = "Price USD must be a positive number")]
        public double PriceUSD { get; set; }

        [Required]
        [DisplayName("Price EUR*")]
        [Range(0, Double.MaxValue, ErrorMessage = "Price EUR must be a positive number")]
        public double PriceEUR { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Release Date*")]
        public DateTime ReleaseDate { get; set; }


        [DisplayName("Visible")]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        [ShowFieldInList(false)]
        [DisplayName("Upload Images")]
        [UIHint("ImagesUpload")]
        [FileStorage("Applications_OSPlatform")]
        [SortableField(false)]
        public string Images { get; set; }

        [DisplayName("Youtube video")]
        public string Video { get; set; }

        [DisplayName("OS Platform*")]
        [UIHint("OSPlatforms")]
        public string[] OSPlatforms { get; set; }
        public YuoTubeCreateVideoModel VideoSettings { get; set; }

    }
}