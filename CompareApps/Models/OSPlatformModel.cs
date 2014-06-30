using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
namespace CompareApps.Models
{
    [DisplayName("OS Platforms")]
    [DisplayColumn("Name")]
    public class OSPlatformModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Name must be under 50 characters")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Alias*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Alias must be under 50 characters")]
        public string Alias { get; set; }
        [Required]
        [DisplayName("Order*")]
        [SortableField(true)]
        public int Order { get; set; }

        public OSPlatformModel() { }
        public OSPlatformModel(OSPlatform os)
        {
            ImportFrom(os);
        }

        #region IViewModel<OSPlatform> Members

        public void ImportFrom(OSPlatform entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Alias = entity.Alias;
            Order = entity.Order;
        }

        public void ExportTo(OSPlatform entity)
        {
            entity.Name = Name;
            entity.Alias = Alias;
            entity.Order = Order;
        }
        #endregion

      

       
    }

    [DisplayName("OS Versions")]
    [DisplayColumn("Name")]
    public class OSVersionModel 
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [DisplayName("OS Platform*")]
        [UIHint("Lookup")]
        [Required]
        [SortableField(true)]
        public int OSPlatformRef { get; set; }
        [Required]
        [UniqueField]
        [DisplayName("Name*")]
        [SortableField(true)]
        public string Name { get; set; }
        [Required]
        [DisplayName("Order*")]
        [SortableField(true)]
        public int Order { get; set; }
        [DisplayName("Visible")]
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        #region IViewModel<OSVersion> Members

        public void ImportFrom(OSVersion entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Order = entity.Order;
            OSPlatformRef = entity.OSPlatformRef;
            IsVisible = entity.IsVisible;
            MarkForDeletion = entity.MarkForDeletion;
        }

        public void ExportTo(OSVersion entity)
        {
            entity.Name = Name;
            entity.Order = Order;
            entity.OSPlatformRef = OSPlatformRef;
            entity.IsVisible = IsVisible;
            entity.MarkForDeletion = MarkForDeletion;
        }
        #endregion

     
    }
}