using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;

namespace CompareApps.Models
{
    [DisplayName("Phone Brand Models")]
    [DisplayColumn("Name")]
    public class BrandTypeModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [DisplayName("Phone Brand*")]
        [UIHint("Lookup")]
        [Required]
        [SortableField(true)]
        public int PhoneBrandRef { get; set; }

        [Required]
        [UniqueField]
        [DisplayName("Name*")]
        [SortableField(true)]
        [StringLength(50, ErrorMessage = "Fault Name must be under 50 characters")]
        public string Name { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        #region IViewModel<BrandType> Members

        public void ImportFrom(BrandType entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            PhoneBrandRef = entity.PhoneBrandRef;
            MarkForDeletion = entity.MarkForDeletion;
        }

        public void ExportTo(BrandType entity)
        {
            entity.Name = Name;
            entity.PhoneBrandRef = PhoneBrandRef;
            entity.MarkForDeletion = MarkForDeletion;
        }

        #endregion

     
    }
}