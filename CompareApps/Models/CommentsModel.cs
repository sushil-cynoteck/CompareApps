using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;
using System.ComponentModel;
using PagedList;
using System.Web.Security;
using CompareApps.Models.services;

namespace CompareApps.Models
{
    public class CommentsListModel
    {
        public int Page { get; set; }
        [DisplayName("Flagged")]
        public bool IsFalgged { get; set; }
        [DisplayName("Application")]
        [UIHint("Lookup")]
        public int? ApplicationRef { get; set; }
        public IPagedList<CommentsModel> Comments { get; set; }

    }
    public class CommentsModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }


        [DisplayName("User")]
        public string UserName { get; set; }

        [DisplayName("User")]
        public Guid UserRef { get; set; }

        public bool IsPostedByAdmin { get; set; }

        public bool IsUserAppDev { get; set; }

        public bool IsMarkedAsSave { get; set; }

        public bool IsSpam { get; set; }

        public bool IsInappropriateLanguage { get; set; }

        [Required(ErrorMessage = "Your comment is too short.")]
        [StringLength(2000, ErrorMessage = "Fault comment text must be under 2000 characters")]
        public string Comment { get; set; }

        public bool IsVisible { get; set; }
        [DisplayName("Application")]
        public string AppName { get; set; }

        public int AppId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy}", ApplyFormatInEditMode = true, NullDisplayText = "(not specified")]
        public DateTime PostedDate { get; set; }
        public CommentsModel(int appId)
        {
            AppId = appId;
        }
        public CommentsModel()
        {
        }
        #region IViewModel<Comment> Members

        public void ImportFrom(Comment entity)
        {
            var service = new AccountService();
            Id = entity.Id;
            AppId = entity.ApplicationRef;
            UserRef = entity.UserRef;
            Comment = entity.CommentText;
            PostedDate = entity.PostedDate;
            IsVisible = entity.IsVisible;
            IsPostedByAdmin = entity.IsPostedByAdmin;
            IsUserAppDev = entity.IsPostedByDeveloper;
            UserName = System.Web.Security.Membership.GetUser(entity.UserRef).UserName;
            IsMarkedAsSave = entity.IsMarkAsSafe;
            IsSpam = entity.IsSpam;
            IsInappropriateLanguage = entity.IsInappropriateLanguage;
           // AppName = entity.ApplicationRef.Name;
        }

        public void ExportTo(Comment entity)
        {
            entity.Id = Id;
            entity.UserRef = UserRef;
            entity.CommentText = Comment;
            entity.PostedDate = PostedDate;
            entity.ApplicationRef = AppId;
            entity.IsVisible = IsVisible;
            entity.IsPostedByAdmin = IsPostedByAdmin;
            entity.IsMarkAsSafe = IsMarkedAsSave;
            entity.IsInappropriateLanguage = IsInappropriateLanguage;
            entity.IsSpam = IsSpam;
            entity.IsPostedByDeveloper = IsUserAppDev;
        }

        #endregion
    }
}