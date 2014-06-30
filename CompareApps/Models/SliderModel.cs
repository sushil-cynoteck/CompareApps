using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using System;
using PagedList;
using System.Collections.Generic;


namespace CompareApps.Models
{
    public class SliderListModel
    {
        public IPagedList<SliderModel> Sliders { get; set; }
    }

    public class SliderFrontendListModel
    {
        public IList<SliderModel> Sliders { get; set; }
        public IDictionary<int, ApplicationModel> Applications { get; set; }
    }

    [DisplayName("Sliders")]
    [DisplayColumn("Id")]
    public class SliderModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [DisplayName("Application*")]
        [UIHint("Lookup")]
        [Required]
        [SortableField(true)]
        public int ApplicationRef { get; set; }

        [DisplayName("Image*")]
        [UIHint("ImageUpload")]
        [Required]
        public string Image { get; set; }

        [DisplayName("Order")]
        [SortableField(true)]
        public int Order { get; set; }


        [DisplayName("Visible")]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        public void ImportFrom(Slider entity)
        {
            Id = entity.Id;
            IsVisible = entity.IsVisible;
            Image = entity.Image;
            Order = entity.Order;
            ApplicationRef = entity.ApplicationRef;
            MarkForDeletion = entity.MarkForDeletion;
        }

        public void ExportTo(Slider entity)
        {
            entity.Image = Image;
            entity.IsVisible = IsVisible;
            entity.Order = Order;
            entity.ApplicationRef = ApplicationRef;
            entity.MarkForDeletion = MarkForDeletion;
        }

      
    }
}