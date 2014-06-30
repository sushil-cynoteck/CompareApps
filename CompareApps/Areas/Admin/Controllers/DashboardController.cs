using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareApps.Models;
namespace CompareApps.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /Admin/Dashboard/
        public ActionResult Index()
        {


    //        var model = new MyDictionaryModel();
    //        model.SelectOptions = new[]
    //{
    //    new SelectListItem { Value = "1", Text = "Categories" },
    //    new SelectListItem { Value = "2", Text = "Phone Make" },
    //    new SelectListItem { Value = "3", Text = "Phone Model" },
    //    new SelectListItem { Value = "4", Text = "Phone OS Versions" },
    //    new SelectListItem { Value = "5", Text = "Users" },
    //    new SelectListItem { Value = "6", Text = "OS Plateforms" },
    //    new SelectListItem { Value = "7", Text = "Applications" },
    //    new SelectListItem { Value = "8", Text = "Sliders" },
    //    new SelectListItem { Value = "9", Text = "Advertisements" },
    //    new SelectListItem { Value = "10", Text = "Static Pages" },
    //    new SelectListItem { Value = "11", Text = "Socail Media" },
    //    new SelectListItem { Value = "12", Text = "Advertisements" },
    //    new SelectListItem { Value = "12", Text = "Comments" },
    //};
            return View();
          
        }

        //
        // GET: /Admin/Dashboard/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/Dashboard/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Admin/Dashboard/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Dashboard/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Dashboard/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/Dashboard/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Admin/Dashboard/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
