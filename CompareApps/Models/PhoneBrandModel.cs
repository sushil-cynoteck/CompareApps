using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;

namespace CompareApps.Models
{
    [DisplayName("Phone Brands")]
    [DisplayColumn("Name")]
    public class PhoneBrandModel
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
        [StringLength(50, ErrorMessage = "Fault Name must be under 50 characters")]
        public string Name { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        #region IViewModel<PhoneBrand> Members

        public void ImportFrom(PhoneBrand entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            OSPlatformRef = entity.OSPlatformRef;
            MarkForDeletion = entity.MarkForDeletion;
        }

        public void ExportTo(PhoneBrand entity)
        {
            entity.Name = Name;
            entity.OSPlatformRef = OSPlatformRef;
            entity.MarkForDeletion = MarkForDeletion;
        }

        #endregion

       
    }
}