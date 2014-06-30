using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CompareApps.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {

        [Required(ErrorMessage = "*")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class LoginUserModel
    {

        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }




    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }


    public class RegisterUserModel
    {

        [Required(ErrorMessage = "*")]
        [Display(Name = "UserName*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email*")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Country*")]
        public string Country { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "State*")]
        public string State { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Date of Birth*")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender*")]
        public string Gender { get; set; }
        [Display(Name = "Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }
        [Display(Name = "Phone Make/Model")]
        public string PhoneMakeOrModel { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Terms of Service*")]
        public bool TermsofService { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "How much is ?")]
        public string Captcha { get; set; }
    }

    public class RegisterDeveloperModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "CompareApps URL*")]
        public string CompareAppsUrl { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "UserName/CompanyName*")]
        public string UserNameOrCompanyName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Country*")]
        public string Country { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "State*")]
        public string State { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Web*")]
        public string Web { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Address Line1*")]
        public string AddressLine1 { get; set; }
        [Display(Name = "Address Line2")]
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "City*")]
        public string City { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Postal Code*")]
        public string PostalCode { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number*")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Terms of Service*")]
        public bool TermsofService { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "How much is ?")]
        public string Captcha { get; set; }
       
    }

    public class AccountModel
    {

        [Required(ErrorMessage = "*")]
        [Display(Name = "UserName*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name*")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name*")]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email*")]
        public string Email { get; set; }
     
        [Display(Name = "Country")]
        public string Country { get; set; }
      
        [Display(Name = "State")]
        public string State { get; set; }
       
        [Display(Name = "City")]
        public string City { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Date of Birth*")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Gender*")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Role*")]
        public string Role { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsActive { get; set; }
    }

    public class AccountListModel
    {
        public List<AccountModel> Accounts { get; set; }
        public string UserName { get; set; }
    }
    public class AccountMainModel
    {
        public RegisterUserModel RegisterUserModel { get; set; }
        public RegisterDeveloperModel RegisterDeveloperModel { get; set; }
        public string HiddenFieldVal1 { get; set; }
        public string HiddenFieldVal2 { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Account Type")]
        public int UserTypeID { get; set; }
      
    }

    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Please enter a valid email ID")]
        [Display(Name = "Email*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class ChangePasswordModel
    {
        public string hdnUserID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password*")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Confirm Password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string ConfPassword { get; set; }
    }

    public class VerifiedAccountModel
    {
        public string hdnUserID { get; set; }
    
    }
}
