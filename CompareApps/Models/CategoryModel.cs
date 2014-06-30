using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using System.Collections;

namespace CompareApps.Models
{

    public class CategoryListViewDataModel
    {
        public IList Items { get; set; }
    }

    [DisplayName("Categories")]
    public class CategoryModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }

        [Required]
        [UniqueField]
        [OrderField]
        [DisplayName("Name*")]
        [StringLength(50, ErrorMessage = "Fault Name must be under 50 characters")]
        public string Name { get; set; }
        [DefaultValue(true)]
        public bool IsVisible { get; set; }

        [DisplayName("Mark for deletion")]
        [DefaultValue(true)]
        public bool MarkForDeletion { get; set; }

        [ScaffoldColumn(false)]
        public int LeftValue { get; set; }
        [ScaffoldColumn(false)]
        public int RightValue { get; set; }
        [ScaffoldColumn(false)]
        public int? ParentRef { get; set; }
        public CategoryModel() { }
        public CategoryModel(Category category)
        {
            ImportFrom(category);
        }

        #region IViewModel<Category> Members

        public void ImportFrom(Category entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            IsVisible = entity.IsVisible;
            MarkForDeletion = entity.MarkForDeletion;
            LeftValue = entity.LeftValue;
            RightValue = entity.RightValue;
            ParentRef = entity.ParentRef;
        }

        public void ExportTo(Category entity)
        {
            entity.Name = Name;
            entity.IsVisible = IsVisible;
            entity.ParentRef = ParentRef;
            entity.MarkForDeletion = MarkForDeletion;
        }

        #endregion
       
          

        public void ExportLeftRightValues(Category to)
        {
            to.LeftValue = LeftValue;
            to.RightValue = RightValue;
        }



        #region debug
        public override string ToString()
        {
            return Id.ToString() + " " + Name;
        }
        #endregion

    }
}