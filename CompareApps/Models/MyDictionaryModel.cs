using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace CompareApps.Models
{
    public class MyDictionaryModel
    {
        public string SelectedOption { get; set; }
        public IEnumerable<SelectListItem> SelectOptions { get; set; }
    }
}