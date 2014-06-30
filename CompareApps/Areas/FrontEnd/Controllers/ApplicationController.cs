using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CompareApps.Areas.FrontEnd.Controllers
{
    public class ApplicationController : Controller
    {
        //
        // GET: /FrontEnd/Application/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {

            return View();
        
        
        }


        //
        // GET: /FrontEnd/Application/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /FrontEnd/Application/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FrontEnd/Application/Create

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
        // GET: /FrontEnd/Application/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /FrontEnd/Application/Edit/5

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
        // GET: /FrontEnd/Application/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /FrontEnd/Application/Delete/5

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
