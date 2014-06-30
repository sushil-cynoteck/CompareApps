using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CompareApps.Models;
using CompareApps.Models.services;
using CompareApps.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using CompareApps.Mailers;
using Mvc.Mailer;
namespace CompareApps.Areas.FrontEnd.Controllers
{
    public class AccountController : Controller
    {
        CompareAppsBDEntities objCompDB;

        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }



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
            var userName = System.Web.Security.Membership.GetUserNameByEmail(model.Email);
            if (ModelState.IsValid && objAccService.ValidateUser(userName, model.Password))
            {
               
                // var account = accountService.GetAccount(userName);

                if (model.RememberMe)
                { FormsAuthentication.SetAuthCookie(userName, true); }
                else
                { FormsAuthentication.SetAuthCookie(userName, false); }
                return RedirectToAction("Index", "Dashboard");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        public ActionResult VerifiedAccount()
        {
           
            var hidden = Convert.ToString(this.Request.QueryString["UserName"]);

            if (!string.IsNullOrEmpty(hidden))
            {
                System.Web.Security.MembershipUser MUser = System.Web.Security.Membership.GetUser(hidden);

                if (MUser != null)
                {
                    MUser.IsApproved = true;
                    System.Web.Security.Membership.UpdateUser(MUser);
                    ViewData["ErrorMessage"] = "Your account has been activated successfully, please login to use your account.";
                }
                else
                {

                    ViewData["ErrorMessage"] = "Username does not exist in the system.";
                }
               
            }
            else
            {
                ViewData["ErrorMessage"] = "Either username is empty or null.";
            
            }
           
           
            return View();
        
        }


        #endregion
        # region "Register"
        public ActionResult Register()
        {


            ViewBag.UserList = getUserList();
            ViewBag.GenderList = getGenderList();
            ViewBag.CountryList = getCountryList();
            return View();

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(AccountMainModel model)
        {
            if (ModelState.IsValid)
            {
                //validate captcha
               
                AccountService objAccService = new AccountService();
                try
                {
                    string Result = string.Empty;
                    if (model.HiddenFieldVal1 == "1")
                    {
                        //user registration

                        if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.RegisterUserModel.Captcha)
                        {
                            ModelState.AddModelError("Captcha", "Wrong value of sum, please try again.");
                            //dispay error and generate a new captcha
                            return View(model);
                        }
                        else
                        {

                            Result = objAccService.RegisterUser(model.RegisterUserModel);

                        }
                    }
                    else if (model.HiddenFieldVal2 == "2")
                    {
                        //developer registration
                        if (Session["Captcha"] == null || Session["Captcha"].ToString() != model.RegisterDeveloperModel.Captcha)
                        {
                            ModelState.AddModelError("Captcha", "Wrong value of sum, please try again.");
                            //dispay error and generate a new captcha
                            return View(model);
                        }
                        else
                        {
                            Result = objAccService.RegisterDeveloper(model.RegisterDeveloperModel);
                        }
                    }
                    else
                    {
                        //please select any user type
                        
                    }

                    if (string.IsNullOrEmpty(Result))
                    {
                        ViewData["ErrorMessage"] = "Thanks to register with Compare Apps, Activation link has been sent to your email ID.";
                    return View("SuccessfulRegistration");
                    }
                    else 
                    {
                     return View(model);
                    }
                }
                catch (MembershipCreateUserException e)
                {

                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private SelectList getGenderList()
        {

            objCompDB = new CompareAppsBDEntities();

            IEnumerable<SelectListItem> GenderList = (from m in objCompDB.GenderTypes select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.Gender, Value = m.GenderID.ToString() });
            return new SelectList(GenderList, "Value", "Text");
        }


        private SelectList getUserList()
        {
            objCompDB = new CompareAppsBDEntities();

            IEnumerable<SelectListItem> UserList = (from m in objCompDB.UserTypes select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.UserType1, Value = m.UserTypeID.ToString() });
            return new SelectList(UserList, "Value", "Text");
        }

        private SelectList getCountryList()
        {
            objCompDB = new CompareAppsBDEntities();

            IEnumerable<SelectListItem> CountryList = (from m in objCompDB.Countries select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });
            return new SelectList(CountryList, "Value", "Text");

        }

        private SelectList getStateList()
        {
            objCompDB = new CompareAppsBDEntities();

            IEnumerable<SelectListItem> StateList = (from m in objCompDB.States select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.Name, Value = m.Id.ToString() });
            return new SelectList(StateList, "Value", "Text");
        }



        public JsonResult GetStates(string id)
        {
            objCompDB = new CompareAppsBDEntities();
            List<SelectListItem> states = new List<SelectListItem>();
            Int32 intID = Convert.ToInt32(id);
            var StateList = (from m in objCompDB.States where m.CountryRef == intID select m).ToList();
            states.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            foreach (var i in StateList)
            {
                states.Add(new SelectListItem { Text = i.Name, Value = Convert.ToString(i.Id) });
            }

            return Json(new SelectList(states, "Value", "Text"));
        }

        public JsonResult GetCityList(string id)
        {
            objCompDB = new CompareAppsBDEntities();
            List<SelectListItem> Citys = new List<SelectListItem>();
            Int32 intID = Convert.ToInt32(id);
            var CityList = (from m in objCompDB.Cities where m.StateRef == intID select m).ToList();
            Citys.Add(new SelectListItem { Text = "--Select--", Value = "0" });
            foreach (var i in CityList)
            {
                Citys.Add(new SelectListItem { Text = i.Name, Value = Convert.ToString(i.Id) });
            }

            return Json(new SelectList(Citys, "Value", "Text"));
        }



         public ActionResult CaptchaImage(string prefix, bool noisy = true)
        {
            var rand = new Random((int)DateTime.Now.Ticks);
            //generate new question
            int a = rand.Next(10, 99);
            int b = rand.Next(0, 9);
            var captcha = string.Format("{0} + {1} = ?", a, b);

            //store answer
            Session["Captcha" + prefix] = a + b;

            //image stream
            FileContentResult img = null;

            using (var mem = new MemoryStream())
            using (var bmp = new Bitmap(130, 30))
            using (var gfx = Graphics.FromImage((Image)bmp))
            {
                gfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.FillRectangle(Brushes.White, new Rectangle(0, 0, bmp.Width, bmp.Height));

                //add noise
                if (noisy)
                {
                    int i, r, x, y;
                    var pen = new Pen(Color.Yellow);
                    for (i = 1; i < 10; i++)
                    {
                        pen.Color = Color.FromArgb(
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)),
                        (rand.Next(0, 255)));

                        r = rand.Next(0, (130 / 3));
                        x = rand.Next(0, 130);
                        y = rand.Next(0, 30);

                        gfx.DrawEllipse(pen,x-r,y-r,r,r);
                    }
                }

                //add question
                gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Gray, 2, 3);

                //render as Jpeg
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                img = this.File(mem.GetBuffer(), "image/Jpeg");
            }

            return img;
        }
        #endregion

        #region "Changed password"




        #endregion

        #region "Reset Password"
         public ActionResult ForgotPassword()
         {


             return View();
         }
         [HttpPost]
         public ActionResult ForgotPassword(FormCollection collection)
         {

             objCompDB = new CompareAppsBDEntities();
             string strUserName = string.Empty;
             string strEmail = string.Empty;
             strEmail = collection["Email"];
             strUserName = System.Web.Security.Membership.GetUserNameByEmail(strEmail);
             if (! string.IsNullOrEmpty(strUserName))
             {

                 UserMailer.PasswordReset(strUserName, strEmail).Send();
                 ViewData["ErrorMessage"] = "password reset link has been sent to you email ID.";
             }
             else
             {

                 ViewData["ErrorMessage"] = "No user exist in the system associated with this email.";
             
             }

      
             return View();
         }

         public ActionResult ChangePassword()
         {
             ChangePasswordModel objCPM = new ChangePasswordModel();

             var hidden = Convert.ToString(this.Request.QueryString["UserName"]);
             objCPM.hdnUserID = hidden;
             return View(objCPM);
         }
         [HttpPost]
         public ActionResult ChangePassword(FormCollection collection)
         {
             AccountService objAccService = new AccountService();
             string result = string.Empty;
             string strUserName = collection[0];
             string strPWD = collection[1];
             result = objAccService.ResetPassword(strUserName, strPWD);
             if (string.IsNullOrEmpty(result))
             {
                 ViewData["ErrorMessage"] = "Your password has been changed successfully.";
             }
             else
             {
                 ViewData["ErrorMessage"] = result;
             }
             return View();
         }
        #endregion


        #region "Log Off"
         public ActionResult LogOff()
         {
             FormsAuthentication.SignOut();
             return RedirectToAction("List","Application");
         }

        #endregion

        #region "Protected Methods"


        #endregion

        #region "Create Role"

        #endregion
    }

}
