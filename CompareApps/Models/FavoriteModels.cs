using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CompareApps.Data;

namespace CompareApps.Models
{
    public class FavoriteListModel
    {
        public FavoriteListModel()
        { Favorites = new List<FavoriteModel>(); }
        public List<FavoriteModel> Favorites { get; set; }

    }
    public class FavoriteModel
    {
        [ScaffoldColumn(false)]
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int AppId { get; set; }

        public ApplicationModel Application { get; set; }

        public FavoriteModel()
        { }
        public FavoriteModel(Favorite entity)
        {
            ImportFrom(entity);

        }

        public void ImportFrom(Favorite entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            AppId = entity.AppId;

        }

        public void ExportTo(Favorite entity)
        {
            entity.UserId = UserId;
            entity.AppId = AppId;

        }


    }
}