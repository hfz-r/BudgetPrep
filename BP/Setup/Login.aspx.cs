using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace BP.Setup
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                LoginUser.FailureText = "";
                Session.Clear();
            }
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            if (Membership.ValidateUser(LoginUser.UserName,LoginUser.Password))
            {
                USER user = new UsersDAL().GetValidUser(LoginUser.UserName, LoginUser.Password);
                if (user != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(LoginUser.UserName, LoginUser.RememberMeSet);

                    Session["UserData"] = user;
                    e.Authenticated = true;
                }
                else
                {
                    e.Authenticated = false;
                }
            }
            else
            {
                e.Authenticated = false;
            }
        }

        protected void LoginUser_LoginError(object sender, EventArgs e)
        {
            LoginUser.FailureText = "Your login attempt was not successful. Please try again.";

            MembershipUser usrInfo = Membership.GetUser(LoginUser.UserName);
            if (usrInfo != null)
            {
                if (usrInfo.IsLockedOut)
                {
                    LoginUser.FailureText = "Your account has been locked out because of too many invalid login attempts. Please contact the administrator to have your account unlocked.";
                }
                else if (!usrInfo.IsApproved)
                {
                    LoginUser.FailureText = "Your account has not yet been approved. You cannot login until an administrator has approved your account.";
                }
            }
        }
    }
}