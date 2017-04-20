using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Membership.OpenAuth;
using DAL;

namespace BP.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            try
            {
                MembershipUser _MembershipUser = Membership.GetUser(RegisterUser.UserName);
                Guid UserId = (Guid)_MembershipUser.ProviderUserKey;

                //Users objUser = new Users();
                //objUser.UserId = UserId;
                //objUser.UserName = _MembershipUser.UserName;
                //objUser.UserPassword = RegisterUser.Password;
                //objUser.UserEmail = _MembershipUser.Email;

                //if (new UserDAL().Insert(objUser))
                //{
                //    FormsAuthentication.SetAuthCookie(RegisterUser.UserName, createPersistentCookie: false);

                //    string continueUrl = RegisterUser.ContinueDestinationPageUrl;
                //    if (!OpenAuth.IsLocalUrl(continueUrl))
                //    {
                //        continueUrl = "~/";
                //    }

                //    Response.Redirect(continueUrl,false);
                //}
            }
            catch (Exception ex)
            {
                Membership.DeleteUser(RegisterUser.UserName);

                throw ex;
            }
        }
    }
}