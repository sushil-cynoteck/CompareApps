using Mvc.Mailer;

namespace CompareApps.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}
		
		public virtual MvcMailMessage Welcome()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "Welcome";
				x.ViewName = "Welcome";
				x.To.Add("some-email@example.com");
			});
		}
 
		public virtual MvcMailMessage PasswordReset(string strUserName,string strEmail)
		{
            ViewBag.URL = "http://localhost:3058/FrontEnd/Account/ChangePassword?UserName=" + strUserName;
            ViewBag.UserName = strUserName;
			return Populate(x =>
			{
				x.Subject = "Password Reset";
				x.ViewName = "PasswordReset";
                x.To.Add(strEmail);
			});
		}

        public virtual MvcMailMessage AccountActivation(string strUserName, string strEmail)
        {
            ViewBag.URL = "http://localhost:3058/FrontEnd/Account/VerifiedAccount?UserName=" + strUserName;
            ViewBag.UserName = strUserName;
            return Populate(x =>
            {
                x.Subject = "Account Activation Email";
                x.ViewName = "AccountActivation";
                x.To.Add(strEmail);
            });
        }
 	}
}