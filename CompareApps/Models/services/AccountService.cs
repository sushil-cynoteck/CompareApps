using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;
using CompareApps.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Transactions;
using CompareApps.Mailers;
using Mvc.Mailer;
namespace CompareApps.Models.services
{
    public class AccountService
    {
        CompareAppsBDEntities objComDBEnt;
        #region "General"


        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }


        public bool ValidateUser(string username, string password)
        {


            return username != null ? System.Web.Security.Membership.ValidateUser(username, password) : false;

        }




        public string getUserRole(string UserName)
        {

            return Roles.GetRolesForUser(UserName).FirstOrDefault();

        }


        public string[] getUserAllRoles(string UserName)
        {
            return Roles.GetRolesForUser(UserName);

        }

        public string[] getAllRoles()
        {

            return Roles.GetAllRoles();

        }

        public bool CheckEmailIsRegistered(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            return System.Web.Security.Membership.FindUsersByEmail(email).Count > 0;
        }
        public bool CheckEmailIsValid(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            string pattern = @"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$";
            Match match = Regex.Match(email.Trim(), pattern, RegexOptions.IgnoreCase);
            if (match.Success)
                return true;
            else
                return false;

        }

        public Guid getUserID(string UserName)
        {

            return (Guid)System.Web.Security.Membership.GetUser(UserName).ProviderUserKey;

        }
        #endregion

        #region Frontend

        public string RegisterUser(RegisterUserModel model)
        {
            string result = string.Empty;
            try
            {
                MembershipCreateStatus status;

                var confirm = Guid.NewGuid().ToString();

                if (!CheckEmailIsRegistered(model.Email))
                {
                    var user = System.Web.Security.Membership.CreateUser(model.UserName, model.Password, model.Email, confirm, confirm, false, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(model.UserName, "User");
                        result = SaveUserProfile(model);

                        if (string.IsNullOrEmpty(result))
                        {

                            //mail sending code to user

                            UserMailer.AccountActivation(model.UserName, model.Email).Send();

                        }
                        else
                        {
                            System.Web.Security.Membership.DeleteUser(model.UserName);

                        }


                    }
                    else
                    {

                        result = ErrorCodeToString(status);
                        System.Web.Security.Membership.DeleteUser(model.UserName);
                    }
                }
                else
                {
                    result = ErrorCodeToString(MembershipCreateStatus.DuplicateEmail);

                }
            }
            catch (Exception ex)
            {
                System.Web.Security.Membership.DeleteUser(model.UserName);
                result = "Error Occured.";

            }
            return result;

        }





        private string SaveUserProfile(RegisterUserModel model)
        {
            string result = string.Empty;
            try
            {
                RegisteredUser objRegUser = new RegisteredUser();
                using (objComDBEnt = new CompareAppsBDEntities())
                {
                    objComDBEnt.Database.Connection.Open();


                    objRegUser.UserID = getUserID(model.UserName);
                    objRegUser.FirstName = model.FirstName;
                    objRegUser.LastName = model.LastName;
                    objRegUser.Email = model.Email;
                    objRegUser.CountryID = Convert.ToInt32(model.Country);
                    objRegUser.StateID = Convert.ToInt32(model.State);
                    objRegUser.DateOfBirth = model.DateOfBirth;
                    objRegUser.Gender = Convert.ToInt32(model.Gender);
                    objRegUser.MobileNumber = model.MobileNumber;
                    objRegUser.PhoneMakeOrModel = model.PhoneMakeOrModel;
                    objComDBEnt.RegisteredUsers.Add(objRegUser);
                    objComDBEnt.SaveChanges();


                }
            }
            catch (Exception ex)
            {

                result = model.UserName;
            }
            return result;
        }
        private string SaveProfile(AccountModel model)
        {
            string result = string.Empty;
            try
            {
                if (model.Role.Equals("User"))
                {
                    RegisteredUser objRegUser = new RegisteredUser();
                    using (objComDBEnt = new CompareAppsBDEntities())
                    {
                        objComDBEnt.Database.Connection.Open();


                        objRegUser.UserID = getUserID(model.UserName);
                        objRegUser.FirstName = model.FirstName;
                        objRegUser.LastName = model.LastName;
                        objRegUser.Email = model.Email;
                        objRegUser.CountryID = Convert.ToInt32(model.Country);
                        objRegUser.StateID = Convert.ToInt32(model.State);
                        objRegUser.DateOfBirth = model.DateOfBirth;
                        objRegUser.Gender = Convert.ToInt32(model.Gender);
                        objComDBEnt.RegisteredUsers.Add(objRegUser);
                        objComDBEnt.SaveChanges();


                    }
                }
                else if (model.Role.Equals("Developer"))
                {
                    AppDeveloper objAppDeveloper = new AppDeveloper();
                    using (objComDBEnt = new CompareAppsBDEntities())
                    {
                        objComDBEnt.Database.Connection.Open();

                        objAppDeveloper.UserID = getUserID(model.UserName);
                        objAppDeveloper.UserNameORCompanyName = model.UserName;
                        objAppDeveloper.FirstName = model.FirstName;
                        objAppDeveloper.LastName = model.LastName;
                        objAppDeveloper.Email = model.Email;

                        objAppDeveloper.CountryID = Convert.ToInt32(model.Country);
                        objAppDeveloper.StateID = Convert.ToInt32(model.State);
                        objAppDeveloper.CityID = Convert.ToInt32(model.City);

                        objComDBEnt.AppDevelopers.Add(objAppDeveloper);
                        objComDBEnt.SaveChanges();

                    }

                }
                else if (model.Role.Equals("ContentManager"))
                {


                }
                else if (model.Role.Equals("Admin"))
                {


                }
            }
            catch (Exception ex)
            {

                result = model.UserName;
            }
            return result;
        }

        public string UpdateRegistrationByAdmin(string strUserName, AccountModel model)
        {
            string result = string.Empty;
            Guid currUserID = Guid.Empty;
            objComDBEnt = new CompareAppsBDEntities();
            string strCurrRole = Roles.GetRolesForUser(strUserName).FirstOrDefault();
            System.Web.Security.MembershipUser objUser = System.Web.Security.Membership.GetUser(strUserName);
            currUserID = (Guid)objUser.ProviderUserKey;
            if (!CheckEmailIsRegistered(model.Email))
            {
                objUser.Email = model.Email;
                System.Web.Security.Membership.UpdateUser(objUser);
                if (strCurrRole.Equals(model.Role))
                {

                    if (model.Role.Equals("User"))
                    {
                        var User = objComDBEnt.RegisteredUsers.Where(x => x.UserID == currUserID).FirstOrDefault();
                        User.FirstName = model.FirstName;
                        User.LastName = model.LastName;
                        User.Email = model.Email;
                        User.CountryID = Convert.ToInt32(model.Country);
                        User.StateID = Convert.ToInt32(model.State);
                        User.DateOfBirth = model.DateOfBirth;
                        User.Gender = Convert.ToInt32(model.Gender);
                        objComDBEnt.SaveChanges();

                    }
                    else if (model.Role.Equals("Developer"))
                    {
                        var Developer = objComDBEnt.AppDevelopers.Where(x => x.UserID == currUserID).FirstOrDefault();
                        Developer.FirstName = model.FirstName;
                        Developer.LastName = model.LastName;
                        Developer.Email = model.Email;
                        Developer.CountryID = Convert.ToInt32(model.Country);
                        Developer.StateID = Convert.ToInt32(model.State);
                        Developer.CityID = Convert.ToInt32(model.City);
                        Developer.Gender = Convert.ToInt32(model.Gender);
                        objComDBEnt.SaveChanges();



                    }
                    else if (model.Role.Equals("ContentManager"))
                    {





                    }
                    else if (model.Role.Equals("Admin"))
                    {





                    }
                }
                else
                {
                    Roles.RemoveUserFromRole(strUserName, strCurrRole);
                    Roles.AddUserToRole(strUserName, model.Role);
                    //deleting old user 
                    if (strCurrRole.Equals("User"))
                    {
                        var User = objComDBEnt.RegisteredUsers.Where(x => x.UserID == currUserID).FirstOrDefault();
                        objComDBEnt.RegisteredUsers.Remove(User);
                        objComDBEnt.SaveChanges();
                    }
                    else if (strCurrRole.Equals("Developer"))
                    {
                        var Developer = objComDBEnt.AppDevelopers.Where(x => x.UserID == currUserID).FirstOrDefault();
                        objComDBEnt.AppDevelopers.Remove(Developer);
                        objComDBEnt.SaveChanges();
                    }
                    else
                    {



                    }
                    //creating new user
                    if (model.Role.Equals("User"))
                    {
                        RegisteredUser objRegisterUser = new RegisteredUser();
                        objRegisterUser.FirstName = model.FirstName;
                        objRegisterUser.LastName = model.LastName;
                        objRegisterUser.Email = model.Email;
                        objRegisterUser.CountryID = Convert.ToInt32(model.Country);
                        objRegisterUser.StateID = Convert.ToInt32(model.State);
                        objRegisterUser.DateOfBirth = model.DateOfBirth;
                        objRegisterUser.Gender = Convert.ToInt32(model.Gender);
                        objComDBEnt.RegisteredUsers.Add(objRegisterUser);
                        objComDBEnt.SaveChanges();


                    }
                    else if (model.Role.Equals("Developer"))
                    {
                        AppDeveloper objAppDeveloper = new AppDeveloper();
                        objAppDeveloper.FirstName = model.FirstName;
                        objAppDeveloper.LastName = model.LastName;
                        objAppDeveloper.Email = model.Email;
                        objAppDeveloper.CountryID = Convert.ToInt32(model.Country);
                        objAppDeveloper.StateID = Convert.ToInt32(model.State);
                        objAppDeveloper.CityID = Convert.ToInt32(model.City);
                        objAppDeveloper.Gender = Convert.ToInt32(model.Gender);
                        objComDBEnt.AppDevelopers.Add(objAppDeveloper);

                        objComDBEnt.SaveChanges();

                    }
                    else
                    {



                    }



                }
            }
            else
            {
                result = ErrorCodeToString(MembershipCreateStatus.DuplicateEmail);

            }

            return result;
        }


        public string RegisterByAdmin(AccountModel model)
        {
            string result = string.Empty;
            try
            {
                MembershipCreateStatus status;

                var confirm = Guid.NewGuid().ToString();

                if (!CheckEmailIsRegistered(model.Email))
                {
                    var user = System.Web.Security.Membership.CreateUser(model.UserName, model.Password, model.Email, confirm, confirm, false, out status);
                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(model.UserName, model.Role);


                        result = SaveProfile(model);

                        if (string.IsNullOrEmpty(result))
                        {

                            //mail sending code to user
                            //UserMailer.AccountActivation(model.UserNameOrCompanyName, model.Email).Send();

                        }
                        else
                        {
                            System.Web.Security.Membership.DeleteUser(model.UserName);

                        }



                    }
                    else
                    {

                        result = ErrorCodeToString(status);
                        System.Web.Security.Membership.DeleteUser(model.UserName);
                    }


                }
                else
                {
                    result = ErrorCodeToString(MembershipCreateStatus.DuplicateEmail);

                }
            }
            catch (Exception ex)
            {
                System.Web.Security.Membership.DeleteUser(model.UserName);
                result = "Error Occured.";

            }
            return result;
        }



        public string RegisterDeveloper(RegisterDeveloperModel model)
        {


            string result = string.Empty;
            try
            {
                MembershipCreateStatus status;

                var confirm = Guid.NewGuid().ToString();

                if (!CheckEmailIsRegistered(model.Email))
                {
                    var user = System.Web.Security.Membership.CreateUser(model.UserNameOrCompanyName, model.Password, model.Email, confirm, confirm, false, out status);

                    if (status == MembershipCreateStatus.Success)
                    {
                        Roles.AddUserToRole(model.UserNameOrCompanyName, "Developer");
                        result = SaveDeveloperProfile(model);

                        if (string.IsNullOrEmpty(result))
                        {

                            //mail sending code to user
                            UserMailer.AccountActivation(model.UserNameOrCompanyName, model.Email).Send();

                        }
                        else
                        {
                            System.Web.Security.Membership.DeleteUser(model.UserNameOrCompanyName);

                        }


                    }
                    else
                    {

                        result = ErrorCodeToString(status);
                        System.Web.Security.Membership.DeleteUser(model.UserNameOrCompanyName);
                    }
                }
                else
                {
                    result = ErrorCodeToString(MembershipCreateStatus.DuplicateEmail);

                }
            }
            catch (Exception ex)
            {
                System.Web.Security.Membership.DeleteUser(model.UserNameOrCompanyName);
                result = "Error Occured.";

            }
            return result;

        }

        private string SaveDeveloperProfile(RegisterDeveloperModel model)
        {
            string result = string.Empty;
            try
            {

                AppDeveloper objAppDeveloper = new AppDeveloper();
                using (objComDBEnt = new CompareAppsBDEntities())
                {
                    objComDBEnt.Database.Connection.Open();

                    objAppDeveloper.UserID = getUserID(model.UserNameOrCompanyName);
                    objAppDeveloper.UserNameORCompanyName = model.UserNameOrCompanyName;
                    objAppDeveloper.FirstName = model.FirstName;
                    objAppDeveloper.LastName = model.LastName;
                    objAppDeveloper.Email = model.Email;
                    objAppDeveloper.CompareAppsURL = model.CompareAppsUrl;
                    objAppDeveloper.PhoneNumber = model.PhoneNumber;
                    objAppDeveloper.Web = model.Web;
                    objAppDeveloper.AddressLine1 = model.AddressLine1;
                    objAppDeveloper.AddressLine2 = model.AddressLine2;
                    objAppDeveloper.CountryID = Convert.ToInt32(model.Country);
                    objAppDeveloper.StateID = Convert.ToInt32(model.State);
                    objAppDeveloper.CityID = Convert.ToInt32(model.City);
                    objAppDeveloper.PostalCode = model.PostalCode;
                    objComDBEnt.AppDevelopers.Add(objAppDeveloper);
                    objComDBEnt.SaveChanges();

                }
            }
            catch (Exception ex)
            {

                result = model.UserNameOrCompanyName;
            }
            return result;
        }

        public string ResetPassword(string UserName, string newPWD)
        {
            string result = string.Empty;
            string OldPWD = string.Empty;
            System.Web.Security.MembershipUser mUser;
            try
            {
                mUser = System.Web.Security.Membership.GetUser(UserName);

                if (mUser != null)
                {
                    OldPWD = mUser.GetPassword();
                    mUser.ChangePassword(OldPWD, newPWD);
                }
                else
                {
                    result = "User does not exist in the system.";

                }
            }
            catch (Exception ex)
            {
                result = "Some error occured.";

            }

            return result;
        }
        #endregion


        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}