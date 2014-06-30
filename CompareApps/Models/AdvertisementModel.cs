using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using System.Web.Mvc;
using PagedList;
namespace CompareApps.Models
{
    public class AdvertisementListModel
    {
        public int Page { get; set; }
        [DisplayName("Name")]
        public string Query { get; set; }
        public int? PlatformRef { get; set; }
        public IPagedList<AdvertisementModel> Advertisements { get; set; }
        public List<OSPlatformModel> OSPlatforms { get; set; }
    }

    public enum AdvertisementType
    {
        Image,
        Flash,
        ThirdParty
    }

    public enum AdvertisementInputType
    {
        File,
        ThirdParty
    }
    public class AdvertisementModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [DisplayName("OS Platform*")]
        [Required]
      
        [UIHint("Lookup")]
        [SortableField(true)]
        public int OSPlatformRef { get; set; }

        [Required]
        [DisplayName("Name*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Name must be under 50 characters")]
        public string Name { get; set; }


        [DisplayName("Image link")]
        [SortableField(false)]
        public string Link { get; set; }

        [UIHint("FileUpload")]
        [DisplayName("File")]
        [SortableField(false)]
        public string FilePath { get; set; }

        [DefaultValue(true)]
        [DisplayName("Is Top")]
        public bool IsTop { get; set; }

        [DisplayName("Type")]
        public AdvertisementInputType AdvertisementInputType { get; set; }

        public AdvertisementType UserInputType { get; set; }

        [DisplayName("Code")]

        [ShowFieldInList(false)]
        [AllowHtml]
        [SortableField(false)]
        public string Code { get; set; }

        public AdvertisementModel() { }
        public AdvertisementModel(Advertisement adv)
        {
            ImportFrom(adv);
        }

        #region IViewModel<Advertisement> Members

        public void ImportFrom(Advertisement entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            OSPlatformRef = entity.OSPlatformRef;
            Code = entity.Code;
            Link = entity.Link;
            FilePath = entity.FileUrl;
            IsTop = entity.IsTop;
            if (entity.Type == AdvertisementType.Flash.ToString())
            {
                UserInputType = AdvertisementType.Flash;
                AdvertisementInputType = AdvertisementInputType.File;
            }
            if (entity.Type == AdvertisementType.Image.ToString())
            {
                UserInputType = AdvertisementType.Image;
                AdvertisementInputType = AdvertisementInputType.File;
            }
            if (entity.Type == AdvertisementType.ThirdParty.ToString())
            {
                UserInputType = AdvertisementType.ThirdParty;
                AdvertisementInputType = AdvertisementInputType.ThirdParty;
            }
        }

        public void ExportTo(Advertisement entity)
        {
            entity.Name = Name;
            entity.OSPlatformRef = OSPlatformRef;
            entity.Code = Code;
            entity.IsTop = IsTop;
            entity.Link = Link;
            entity.FileUrl = FilePath;
            entity.Type = UserInputType.ToString();
        }

        #endregion
       
    }
}