using Mvc.Mailer;

namespace CompareApps.Mailers
{ 
    public interface IUserMailer
    {
			MvcMailMessage Welcome();
            MvcMailMessage PasswordReset(string strUserName, string strEmail);
            MvcMailMessage AccountActivation(string strUserName, string strEmail);
	}
}