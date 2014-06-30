using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using PagedList;
using System.Web.Mvc;
namespace CompareApps.Models
{
    public class ApplicationListModel
    {
        public int Page { get; set; }
        [DisplayName("Name")]
        public string Query { get; set; }
        [DisplayName("Category")]
        [UIHint("Lookup")]
        [SortableField(true)]
        public int? CategoryRef { get; set; }
        public int? PlatformRef { get; set; }
        public IPagedList<ApplicationModel> Applications { get; set; }
        public List<OSPlatformModel> OSPlatforms { get; set; }
    }

    public enum ApplicationVizardSteps
    {
        General = 0,
        Android = 1,
        Apple = 2,
        WF = 3,
        BlackBerry = 4,
        Summary = 5
    }

    public class ApplicationVizardModel : ApplicationModel
    {
        public ApplicationModel Application { get; set; }
    }

    public class ApplicationFontendListModel
    {
        public int Page { get; set; }
        public int PagesCuont { get; set; }
        public string Query { get; set; }
        public string Category { get; set; }
        public string Platform { get; set; }
        public IPagedList<ApplicationModel> Applications { get; set; }
        public List<OSPlatformModel> OSPlatforms { get; set; }
    }

    public class AppModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

    }

    [DisplayName("Applications")]
    [DisplayColumn("Name")]
    public class ApplicationModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Application name*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Title must be under 50 characters")]
        public string Name { get; set; }

        [UIHint("TinyMCE")]
        [AllowHtml]
        [Required]
        [SortableField(false)]
        [ShowFieldInList(false)]
        [DisplayName("Description*")]
        public string Description { get; set; }

        [DisplayName("Visible")]
        [DefaultValue(true)]
        [SortableField(false)]
        public bool IsVisible { get; set; }


        public bool IsAddedToFavorites { get; set; }

        [SortableField(false)]
        [DisplayName("Facebook link")]
        [StringLength(250, ErrorMessage = "Fault InfoUrlFacebook must be under 250 characters")]
        public string InfoUrlFacebook { get; set; }

        [SortableField(false)]
        [DisplayName("Twitter link")]
        [StringLength(250, ErrorMessage = "Fault InfoUrlTwitter must be under 250 characters")]
        public string InfoUrlTwitter { get; set; }

        [SortableField(false)]
        [StringLength(250, ErrorMessage = "Fault SiteUrl must be under 250 characters")]
        public string SiteUrl { get; set; }

        [DisplayName("Developer*")]
        [UIHint("Lookup")]
        [Required]
        public int Developer { get; set; }

        public string CompanyName { get; set; }
        public bool IsChecked { get; set; }

        [Required]
        [UIHint("ImageUpload")]
        [DisplayName("Application icon*")]
        [SortableField(false)]
        public string Icon { get; set; }

        [DisplayName("Category")]
        [UIHint("Lookup")]
        [Required]
        [SortableField(true)]
        public int CategoryRef { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        [SortableField(false)]
        public bool MarkForDeletion { get; set; }

        [HiddenInput]
        public int PagesCount { get; set; }

        [Required]
        [DisplayName("OS Platform*")]
        [UIHint("OSPlatforms")]
        public string[] OSPlatforms { get; set; }

        [SortableField(false)]
        public IEnumerable<Applications_OSPlatformModel> Applications_OSPlatforms { get; set; }

        public IEnumerable<CommentsModel> Comments { get; set; }

        public ApplicationModel()
        { }
        public ApplicationModel(ApplicationsnApp entity)
        {
            ImportFrom(entity);

        }
        #region IViewModel<Application> Members


        public void ImportFrom(ApplicationsnApp entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            IsVisible = entity.IsVisible;
            Description = entity.Description;
            InfoUrlFacebook = entity.InfoUrlFacebook;
            InfoUrlTwitter = entity.InfoUrlTwitter;
            SiteUrl = entity.SiteUrl;
            Icon = entity.Icon;
            CategoryRef = entity.CategoryRef;
            Developer = entity.Developer;
            MarkForDeletion = entity.MarkForDeletion;
        }

        public void ExportTo(ApplicationsnApp entity)
        {
            entity.IsVisible = IsVisible;
            entity.Name = Name;
            entity.Icon = Icon;
            entity.Description = Description;
            entity.InfoUrlFacebook = InfoUrlFacebook;
            entity.InfoUrlTwitter = InfoUrlTwitter;
            entity.SiteUrl = SiteUrl;
            entity.CategoryRef = CategoryRef;
            entity.Developer = Developer;
            entity.MarkForDeletion = MarkForDeletion;
        }

        #endregion
    }
}