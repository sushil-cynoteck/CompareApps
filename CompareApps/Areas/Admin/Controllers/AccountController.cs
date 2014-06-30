using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareApps.Models;
using CompareApps.Models.services;
using System.Web.Security;
namespace CompareApps.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {

        # region "Login"
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginUserModel model, string returnUrl)
        {
            AccountService objAccService = new AccountService();
            var userName = Membership.GetUserNameByEmail(model.Email);
            if (ModelState.IsValid && objAccService.ValidateUser(userName, model.Password))
            {
                
               // var account = accountService.GetAccount(userName);
              
                if (model.RememberMe)
                { FormsAuthentication.SetAuthCookie(userName, true); }
                else
                { FormsAuthentication.SetAuthCookie(userName, false); }
                return RedirectToAction("Index","Dashboard");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

       
        #endregion

        # region "Register"
        public ActionResult Register()
        {
            return View();

        }
        #endregion

        #region "Changed password"




        #endregion

        #region "Reset Password"


        #endregion

        #region "Log Off"

        //
        // POST: /Account/LogOff

       
      
         public ActionResult LogOff()
         {
             FormsAuthentication.SignOut();
             return RedirectToAction("Login","Account");
         }

    


        #endregion

        #region "Protected Methods"


        #endregion

    


    }
}
