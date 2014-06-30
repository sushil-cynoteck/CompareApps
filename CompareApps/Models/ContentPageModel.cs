using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using System.Web.Mvc;

namespace CompareApps.Models
{
    [DisplayName("Content Pages")]
    public class ContentPageModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [UniqueField]
        [DisplayName("Alias*")]
        [SortableField(true)]
        [StringLength(250, ErrorMessage = "Fault Alias must be under 250 characters")]
        public string Alias { get; set; }
        [Required]
        [DisplayName("Title*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Title must be under 50 characters")]
        public string Title { get; set; }



        [UIHint("TinyMCE")]
        [ShowFieldInList(false)]
        [AllowHtml]
        [Required]
        [SortableField(false)]
        public string Content { get; set; }
        [Required]
        [DefaultValue(0)]
        [ShowFieldInList(true, "Display order")]
        [SortableField(true)]
        public int Order { get; set; }
        [DefaultValue(false)]
        [DisplayName("Publish")]
        public bool IsVisible { get; set; }
        [DefaultValue(true)]
        [DisplayName("Show In Menu")]
        public bool ShowInMenu { get; set; }



        public ContentPageModel() { }
        public ContentPageModel(ContentPage page)
        {
            ImportFrom(page);
        }


        #region IViewModel<ContentPage> Members

        public void ImportFrom(ContentPage entity)
        {
            Id = entity.Id;
            Title = entity.Title;
            Alias = entity.Alias;
            Content = entity.Content;
            Order = entity.Order;
            IsVisible = entity.IsVisible;
            ShowInMenu = entity.ShowInMenu;

        }

        public void ExportTo(ContentPage entity)
        {
            entity.Title = Title;
            entity.Alias = Alias;
            entity.Content = Content;
            entity.Order = Order;
            entity.IsVisible = IsVisible;
            entity.ShowInMenu = ShowInMenu;

        }

        #endregion

   
    }
}