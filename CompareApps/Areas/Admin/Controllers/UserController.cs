using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompareApps.Models;
using CompareApps.Models.services;
using System.Web.Security;
using CompareApps.Data;
namespace CompareApps.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Admin/User/
        CompareAppsBDEntities objCompDB;
        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            MembershipUserCollection objUser = System.Web.Security.Membership.GetAllUsers();
            List<AccountModel> objAM = new List<AccountModel>();
           
            foreach (MembershipUser user in objUser)
            {
                AccountModel objModel = new AccountModel();
               
                    objModel.UserName = user.UserName;
                    objModel.Email = user.Email;
                    objModel.Role = Roles.GetRolesForUser(user.UserName).FirstOrDefault();
                    objModel.IsActive = user.IsApproved;
                    objModel.IsLockedOut = user.IsLockedOut;
                    objAM.Add(objModel);
                
            }



            return View(objAM);
        }

        //
        // GET: /Admin/User/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Admin/User/Create

        public ActionResult Create()
        {

            ViewBag.Roles = getRoleList();
            ViewBag.GenderList = getGenderList();
            ViewBag.CountryList = getCountryList();
            return View();
        }

        private SelectList getRoleList()
        {
            objCompDB = new CompareAppsBDEntities();
           IEnumerable<SelectListItem> RoleList = (from m in objCompDB.Roles select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.RoleName, Value = m.RoleName.ToString() });
            return new SelectList(RoleList, "Value", "Text");
        
        }

        private SelectList getGenderList()
        {

            objCompDB = new CompareAppsBDEntities();

            IEnumerable<SelectListItem> GenderList = (from m in objCompDB.GenderTypes select m).AsEnumerable().Select(m => new SelectListItem() { Text = m.Gender, Value = m.GenderID.ToString() });
            return new SelectList(GenderList, "Value", "Text");
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

        //
        // POST: /Admin/User/Create

        [HttpPost]
        public ActionResult Create(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                //validate captcha
                string strResult = string.Empty;
                AccountService objAccService = new AccountService();
                try
                {
                   strResult = objAccService.RegisterByAdmin(model);
                   if (string.IsNullOrEmpty(strResult))
                   {
                       ViewData["ErrorMessage"] = "Profile has been created successfully.";
                       return RedirectToAction("Index");
                   }
                   else
                   {
                       ViewData["ErrorMessage"] = strResult;
                       return View(model);
                   
                   }
                }
                catch
                {
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        //
        // GET: /Admin/User/Edit/5

        public ActionResult Edit(string strUserName)
        {
             CompareAppsBDEntities objCADB = new CompareAppsBDEntities();
            AccountModel objAccountModel = new AccountModel();
            ViewBag.Roles = getRoleList();
            ViewBag.GenderList = getGenderList();
            ViewBag.CountryList = getCountryList();

            //get role from on basis of username 

            var CurrRole = Roles.GetRolesForUser(strUserName).FirstOrDefault();

            System.Web.Security.MembershipUser objUser = System.Web.Security.Membership.GetUser(strUserName);
            Guid CUserID = (Guid)objUser.ProviderUserKey;
            objAccountModel.Role = CurrRole;
            if (CurrRole.Equals("User"))
            {
                var user = objCADB.RegisteredUsers.Where(x => x.UserID == CUserID).FirstOrDefault();
                objAccountModel.FirstName = user.FirstName;
                objAccountModel.LastName = user.LastName;
                objAccountModel.Email = user.Email;
                objAccountModel.DateOfBirth = Convert.ToDateTime(user.DateOfBirth);
                objAccountModel.Country = Convert.ToString(user.CountryID);
                objAccountModel.State =  Convert.ToString(user.StateID);
                objAccountModel.Gender = Convert.ToString( user.Gender);
            }
            else if (CurrRole.Equals("Developer"))
            {
                var developer = objCADB.AppDevelopers.Where(x => x.UserID == CUserID).FirstOrDefault();
                objAccountModel.FirstName = developer.FirstName;
                objAccountModel.LastName = developer.LastName;
                objAccountModel.Email = developer.Email;
                objAccountModel.Country =  Convert.ToString(developer.CountryID);
                objAccountModel.State =  Convert.ToString(developer.StateID);
               objAccountModel.Gender =  Convert.ToString(developer.Gender);
            }



            return View(objAccountModel);
        }

        //
        // POST: /Admin/User/Edit/5

        [HttpPost]
        public ActionResult Edit(string strUserName,AccountModel model)
        {
            string result = string.Empty;
              AccountService objAccService = new AccountService();
            try
            {
                // TODO: Add update logic here
                //find out current role of user
                result = objAccService.UpdateRegistrationByAdmin(strUserName, model);

                if (string.IsNullOrEmpty(result))
                {
                    ViewData["ErrorMessage"] = "Profile has been updated successfully.";
                    return RedirectToAction("Index");


                }
                else
                {
                    ViewData["ErrorMessage"] = result;
                    return View(model);
                
                }

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Admin/User/Delete/5

        public ActionResult Delete(string strUserName)
        {
            CompareAppsBDEntities objCADB = new CompareAppsBDEntities();
            MembershipUser objUser = System.Web.Security.Membership.GetUser(strUserName);

            if (objUser != null)
            {
                Guid objUserID = (Guid)objUser.ProviderUserKey;
                string strRole = Roles.GetRolesForUser(strUserName).FirstOrDefault();
                if (strRole == "User")
                {
                    var User = objCADB.RegisteredUsers.Where(x => x.UserID == objUserID).FirstOrDefault();
                    if (User != null)
                    {
                        objCADB.RegisteredUsers.Remove(User);
                        objCADB.SaveChanges();
                       
                    }
                    System.Web.Security.Membership.DeleteUser(strUserName, true);
                }
                else if (strRole == "Developer")
                {
                    var Developer = objCADB.AppDevelopers.Where(x => x.UserID == objUserID).FirstOrDefault();
                    if (Developer != null)
                    {
                        objCADB.AppDevelopers.Remove(Developer);
                        objCADB.SaveChanges();
                       
                    }
                    System.Web.Security.Membership.DeleteUser(strUserName, true);
                }
                else if (strRole == "Admin")
                {



                }
                else
                {
                    System.Web.Security.Membership.DeleteUser(strUserName, true);
                }
              
                    objCADB.SaveChanges();
                    //objUser.IsLockedOut = false;
             

            }


            return RedirectToAction("Index");
        }

        //
        // POST: /Admin/User/Delete/5

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

        public ActionResult Activate(string strUserName)
        {

            MembershipUser objUser = System.Web.Security.Membership.GetUser(strUserName);
            if (objUser != null)
            {
                if (objUser.IsApproved)
                {
                    objUser.IsApproved = false;
                }

                else
                {
                    objUser.IsApproved = true;
                
                }
                System.Web.Security.Membership.UpdateUser(objUser);
                
            }


            return RedirectToAction("Index");
        }

        public ActionResult LockedOut(string strUserName)
        {

            CompareAppsBDEntities objCADB = new CompareAppsBDEntities();
            MembershipUser objUser = System.Web.Security.Membership.GetUser(strUserName);
            
            if (objUser != null)
            {
                Guid objUserID = (Guid)objUser.ProviderUserKey;
                if (objUser.IsLockedOut)
                {
                    objUser.UnlockUser();
                    System.Web.Security.Membership.UpdateUser(objUser);
                }

                else
                {
                    var objMebership = objCADB.Memberships.Where(x => x.UserId == objUserID).FirstOrDefault();
                    objMebership.IsLockedOut = true;
                    objCADB.SaveChanges();
                    //objUser.IsLockedOut = false;
                }
               
                
            }


            return RedirectToAction("Index");
        }


    }
}
