using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;

namespace BP.Setup
{
    public class User 
    {
        public string username { get; set; }
        public string password { get; set; }
        public string password2 { get; set; }
        public string email { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string fullname { get; set; }
        public string ic { get; set; }
        public string dept { get; set; }
        public string post { get; set; }
        public string phone { get; set; }
        public string agree { get; set; }
    }

    public partial class UserSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
            }
        }

        [WebMethod]
        public static string FormValues(string obj)
        {
            string msg = string.Empty;
            JavaScriptSerializer js = new JavaScriptSerializer();
            User _Users = js.Deserialize<User>(obj);

            MembershipCreateStatus result = new MembershipCreateStatus();
            try
            {
                MembershipUser newUser = Membership.CreateUser(_Users.username,_Users.password,_Users.email,_Users.question,
                    _Users.answer,true,out result);

                if (result.ToString() == "Success")
                {
                    MembershipUser _MembershipUser = Membership.GetUser(newUser.UserName);
                    Guid UserId = (Guid)_MembershipUser.ProviderUserKey;

                    USER _obj = new USER();
                    _obj.UserId = UserId;
                    _obj.UserName = _Users.username;
                    _obj.UserPassword = _Users.password;
                    _obj.FullName = _Users.fullname;
                    _obj.UserEmail = _Users.email;
                    _obj.UserIC = _Users.ic;
                    _obj.Department = _Users.dept;
                    _obj.Position = _Users.post;
                    _obj.UserPhoneNo = _Users.phone;

                    if (new UsersDAL().InsertUsers(_obj))
                    {
                        msg = "Your information was successfully saved!";
                    }
                    else
                    {
                        msg = "<Fail> Your information was unsuccessfully to save!";
                    }
                }
                else
                {
                    //Membership.DeleteUser(_Users.username);
                    msg = "<Fail> " + GetErrorMessage(result);
                }
            }
            catch
            {
                //Membership.DeleteUser(_Users.username);
                msg = "<Fail> " + GetErrorMessage(result);
            }

            return msg;
        }

        public static string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

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
    }
}