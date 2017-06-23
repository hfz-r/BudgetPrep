using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using OBSecurity;
using BP.Classes;
using System.Web.Services;
using Newtonsoft.Json;

namespace BP.Setup
{
    public class VerifyHelper
    {
        public string Response;
        public string Username;
        public string Question;
    }

    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Membership.EnablePasswordReset)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (!Page.IsPostBack)
            {
                FormsAuthentication.SignOut();
                LoginUser.FailureText = "";
                Session.Clear();
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.Cookies["myCookie"] != null)
                {
                    HttpCookie cookie = Request.Cookies.Get("myCookie");
                    LoginUser.UserName = cookie.Values["username"];

                    LoginUser.RememberMeSet = (!String.IsNullOrEmpty(LoginUser.UserName));
                }
            }
            Response.Cache.SetNoStore();
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            try
            {
                if (Membership.ValidateUser(LoginUser.UserName, LoginUser.Password))
                {
                    MasterUser user = new UsersDAL().GetValidUser(LoginUser.UserName, LoginUser.Password);
                    if (user != null)
                    {
                        FormsAuthentication.RedirectFromLoginPage(LoginUser.UserName, LoginUser.RememberMeSet);

                        HttpCookie myCookie = new HttpCookie("myCookie");
                        Boolean remember = LoginUser.RememberMeSet;

                        if (remember)
                        {
                            Int32 persistDays = 15;
                            myCookie.Values.Add("username", LoginUser.UserName);
                            myCookie.Expires = DateTime.Now.AddDays(persistDays);
                        }
                        else
                        {
                            myCookie.Values.Add("username", string.Empty);
                            myCookie.Expires = DateTime.Now.AddMinutes(5);
                        }

                        Response.Cookies.Add(myCookie);

                        Session["UserData"] = user;
                        CreateMenu(user);
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
            catch (Exception ex)
            {
                throw ex;
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

        private void CreateMenu(MasterUser AuthUser)
        {
            List<PageMenuHelper> lstPages = new List<PageMenuHelper>();
            foreach (JuncUserRole jr in AuthUser.JuncUserRoles)
            {
                foreach (JuncRolePage rp in jr.MasterRole.JuncRolePages.Where(x => x.Status == "A"))
                {
                    if (lstPages.Where(x => x.PageID == rp.PageID).Count() == 0)
                    {
                        lstPages.Add(new PageMenuHelper()
                        {
                            PageID = (int)rp.PageID,
                            PageName = rp.MasterPage.PageName,
                            PagePath = rp.MasterPage.PagePath,
                            ParentPageID = (rp.MasterPage.ParentPageID != null) ? (int)rp.MasterPage.ParentPageID : 0,
                            PageOrder = rp.MasterPage.PageOrder,
                            MenuID = (int)rp.MasterPage.MenuID,
                            MenuName = rp.MasterPage.MasterMenu.MenuName,
                            MenuIcon = rp.MasterPage.MasterMenu.MenuIcon,
                            MenuOrder = rp.MasterPage.MasterMenu.MenuOrder
                        });
                    }
                }
            }

            Session["ListPages"] = lstPages;
        }

        [WebMethod]
        public static VerifyHelper GetVerification(string email)
        {
            var dt = new VerifyHelper();

            try
            {
                MembershipUser u = Membership.GetUser(Membership.GetUserNameByEmail(email));
                if (u == null)
                {
                    string myString = HttpUtility.HtmlEncode(email);
                    dt.Response = "Email " + myString + " is not valid. Please check and re-enter.";
                }
                else
                {

                    MasterUser _MasterUser = DAL.UsersDAL.StaticUserId(0, u.UserName);
                    if (_MasterUser.UUID == (Guid)u.ProviderUserKey)
                    {
                        if (_MasterUser.SecQuestion == null)
                            dt.Response = "Invalid Email";
                        else if (_MasterUser.SecQuestion == string.Empty)
                            dt.Response = "Security question was not set. Contact your admin";
                        else
                        {
                            dt.Response = "";
                            dt.Username = _MasterUser.UserName;
                            dt.Question = _MasterUser.SecQuestion;
                        }
                    }
                    else
                    {
                        dt.Response = "Invalid Email";
                    }
                }
            }
            catch
            {
                dt.Response = "Invalid Email";
            }
            return dt;
        }

        [WebMethod]
        public static string ResetPassword(string username, string answer)
        {
            string json;
            var ReturnObj = new { status = "", result = "" };

            try
            {
                MasterUser user = new UsersDAL().VerifyAnswer(username, answer);
                if (user != null)
                {
                    string newPassword;
                    MembershipUser u = Membership.GetUser(username);
                    if (u == null)
                    {
                        string myString = HttpUtility.HtmlEncode(username);
                        throw new Exception("Username " + myString + " not found. Please check the value and re-enter.");
                    }
                    //verify at Membership side
                    newPassword = u.ResetPassword(answer);

                    if (newPassword != null)
                    {
                        //verify at DB side
                        if (new UsersDAL().ResetPassword(username, u.GetPassword(answer)))
                        {
                            bool mail = MailHelper.SendMail(user, u.GetPassword(answer));
                            //bool mail = MailHelper.NewPasswordMail(user.UserEmail, u.GetPassword());
                            ReturnObj = new
                            {
                                status = "Success",
                                result = "Password successfully reset. Your new password will be sent to your email id : "
                                    + new Helper().EmailClipper(user.UserEmail)
                            };
                        }
                        else
                        {
                            throw new Exception("Password reset failed. Please re-enter your values and try again.");
                        }
                    }
                    else
                    {
                        throw new Exception("Password reset failed. Please re-enter your values and try again.");
                    }
                }
                else
                {
                    throw new Exception("Wrong Answer.");
                }
            }
            catch (Exception ex)
            {
                ReturnObj = new { status = "Fail", result = "An error occurred. Error Message: " + ex.Message };
            }

            return json = JsonConvert.SerializeObject(ReturnObj, Formatting.Indented);
        }
    }
}