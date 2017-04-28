using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;
using Newtonsoft.Json;
using OBSecurity;

namespace BP.Setup
{
    public class User 
    {
        [JsonProperty("ctl00$MainContent$username")]
        public string username { get; set; }
        [JsonProperty("ctl00$MainContent$password")]
        public string password { get; set; }
        [JsonProperty("ctl00$MainContent$password2")]
        public string password2 { get; set; }
        [JsonProperty("ctl00$MainContent$email")]
        public string email { get; set; }
        [JsonProperty("ctl00$MainContent$question")]
        public string question { get; set; }
        [JsonProperty("ctl00$MainContent$answer")]
        public string answer { get; set; }
        [JsonProperty("ctl00$MainContent$status")]
        public string status { get; set; }
        [JsonProperty("ctl00$MainContent$role")]
        public string role { get; set; }
        [JsonProperty("ctl00$MainContent$fullname")]
        public string fullname { get; set; }
        [JsonProperty("ctl00$MainContent$icno")]
        public string icno { get; set; }
        [JsonProperty("ctl00$MainContent$dept")]
        public string dept { get; set; }
        [JsonProperty("ctl00$MainContent$post")]
        public string post { get; set; }
        [JsonProperty("ctl00$MainContent$phone")]
        public string phone { get; set; }
    }

    public class ReturnValue 
    { 
        public string pageTitle;
        public string pageBody;
    }

    public partial class UserSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetData();
                Session["UsersPageMode"] = Helper.PageMode.New;
            }
        }

        protected void gvUsers_PreRender(object sender, EventArgs e)
        {
            if (gvUsers.Rows.Count > 0)
            {
                gvUsers.UseAccessibleHeader = true;
                gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvUsers.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        private void GetData()
        {
            try
            {
                List<MasterUser> data = new UsersDAL().GetUsers().ToList();

                if (data != null)
                {
                    Session["MasterUserData"] = data;
                    BindGrid();
                    LoadDropDown();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void BindGrid()
        {
            try
            {
                gvUsers.DataSource = (List<MasterUser>)Session["MasterUserData"];
                gvUsers.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();

                    GridViewRow selectedRow = gvUsers.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    MasterUser objMasterUser = new UsersDAL().GetUsers().Where(x => x.UserID == Convert.ToInt32(gvUsers.DataKeys[selectedRow.RowIndex]["UserID"])).ToList().FirstOrDefault();
                    MembershipUser _MembershipUser = Membership.GetUser(objMasterUser.UserName);

                    if ((Guid)_MembershipUser.ProviderUserKey == objMasterUser.UUID)
                    {
                        Session["SelectedMasterUser"] = objMasterUser;

                        username.Value = objMasterUser.UserName;
                        email.Value = objMasterUser.UserEmail;
                        question.Value = objMasterUser.SecQuestion;
                        answer.Value = objMasterUser.SecAnswer;
                        fullname.Value = objMasterUser.FullName;
                        icno.Value = objMasterUser.UserIC;
                        dept.Value = objMasterUser.Department;
                        post.Value = objMasterUser.Position;
                        phone.Value = objMasterUser.UserPhoneNo;
                        agree.Checked = false;

                        status.SelectedIndex = -1;
                        status.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objMasterUser.UserStatus))).Selected = true;

                        role.SelectedIndex = -1;
                        role.Items.FindByValue(new UsersRoleDAL().ListUserRole().Where(x => x.UserID == objMasterUser.UserID)
                            .Select(y=>Convert.ToString(y.RoleID)).FirstOrDefault()).Selected = true;

                        ChangePageMode(Helper.PageMode.Edit);
                        form_Wiz.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        [WebMethod]
        public static ReturnValue FormValues(string obj)
        {
            var dt = new ReturnValue();
            MembershipCreateStatus result = new MembershipCreateStatus();

            User _Users = JsonConvert.DeserializeObject<User>(obj);

            try
            {
                if ((Helper.PageMode)HttpContext.Current.Session["UsersPageMode"] == Helper.PageMode.New)
                {
                    MembershipUser newUser = Membership.CreateUser(_Users.username, _Users.password, _Users.email, _Users.question,
                   _Users.answer, true, out result);

                    if (result.ToString() == "Success")
                    {
                        if (new UsersDAL().GetUsers().Where(x => x.UserName.ToUpper().Trim() == _Users.username.ToUpper().Trim()).Count() > 0)
                        {
                            dt.pageTitle = "Failure";
                            dt.pageBody = "Username already exists";
                            return dt;
                        }

                        MembershipUser _MembershipUser = Membership.GetUser(newUser.UserName);
                        Guid UserId = (Guid)_MembershipUser.ProviderUserKey;

                        MasterUser objMasterUser = new MasterUser();
                        objMasterUser.UUID = UserId;
                        objMasterUser.UserName = _Users.username.Trim();
                        //objMasterUser.UserPassword = _Users.password.Trim();
                        objMasterUser.UserPassword = _MembershipUser.GetPassword(_Users.answer.Trim());
                        objMasterUser.FullName = _Users.fullname.Trim();
                        objMasterUser.UserEmail = _Users.email.Trim();
                        objMasterUser.UserIC = _Users.icno.Trim();
                        objMasterUser.Department = _Users.dept.Trim();
                        objMasterUser.Position = _Users.post.Trim();
                        objMasterUser.UserPhoneNo = _Users.phone.Trim();
                        objMasterUser.SecQuestion = _Users.question.Trim();
                        objMasterUser.SecAnswer = _Users.answer.Trim();
                        objMasterUser.UserStatus = new Helper().GetItemStatusEnumValueByName(_Users.status.Trim());
                        objMasterUser.CreatedBy = new UsersDAL().GetUserID(HttpContext.Current.User.Identity.Name);
                        objMasterUser.CreatedTimeStamp = DateTime.Now;
                        objMasterUser.ModifiedBy = new UsersDAL().GetUserID(HttpContext.Current.User.Identity.Name);
                        objMasterUser.ModifiedTimeStamp = DateTime.Now;

                        if (new UserSetup().AddUserRole(objMasterUser,_Users.role) == false)
                        {
                            dt.pageTitle = "Failure";
                            dt.pageBody = "An error occurred while creating User";
                        }

                        if (new UsersDAL().InsertUsers(objMasterUser))
                        {
                            bool mail = MailHelper.SendMail(objMasterUser, _MembershipUser.GetPassword(_Users.answer.Trim()));
                            //bool mail = MailHelper.NewPasswordMail(objMasterUser.UserEmail, _Users.password);

                            dt.pageTitle = "Success";
                            dt.pageBody = "User created successfully, " + ((mail) ? "Mail Sent" : "Error Sending Mail");
                        }
                        else
                        {
                            dt.pageTitle = "Failure";
                            dt.pageBody = "An error occurred while creating User";
                        }
                    }
                    else
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = GetErrorMessage(result);
                        return dt;
                    }
                }
                else if ((Helper.PageMode)HttpContext.Current.Session["UsersPageMode"] == Helper.PageMode.Edit)
                {
                    MasterUser objMasterUser = (MasterUser)HttpContext.Current.Session["SelectedMasterUser"];
                    objMasterUser.UserName = _Users.username.Trim();
                    objMasterUser.FullName = _Users.fullname.Trim();
                    objMasterUser.UserEmail = _Users.email.Trim();
                    objMasterUser.UserIC = _Users.icno.Trim();
                    objMasterUser.Department = _Users.dept.Trim();
                    objMasterUser.Position = _Users.post.Trim();
                    objMasterUser.UserPhoneNo = _Users.phone.Trim();
                    objMasterUser.SecQuestion = _Users.question.Trim();
                    //objMasterUser.SecAnswer = _Users.answer.Trim();
                    objMasterUser.UserStatus = new Helper().GetItemStatusEnumValueByName(_Users.status.Trim());
                    objMasterUser.ModifiedBy = new UsersDAL().GetUserID(HttpContext.Current.User.Identity.Name);
                    objMasterUser.ModifiedTimeStamp = DateTime.Now;

                    if (new UserSetup().AddUserRole(objMasterUser, _Users.role) == false)
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = "An error occurred while creating User";
                    }

                    if (new UsersDAL().UpdateUsers(objMasterUser))
                    {
                        MembershipUser u = Membership.GetUser(objMasterUser.UserName);
                        u.Email = objMasterUser.UserEmail;
                        Membership.UpdateUser(u);

                        dt.pageTitle = "Success";
                        dt.pageBody = "User updated successfully";
                    }
                    else
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = "An error occurred while updating User";
                    }
                }
            }
            catch
            {
                //rollback - start
                Membership.DeleteUser(_Users.username);
                new UsersDAL().DeleteUsers(_Users.username);
                //rollback - end

                dt.pageTitle = "Failure";
                dt.pageBody = GetErrorMessage(result);
            }

            return dt;
        }

        protected bool AddUserRole(MasterUser objMasterUser, string role)
        {
            MasterRole objMasterRole = new UsersRoleDAL().GetRoles().Where(x => x.RoleID == Convert.ToInt32(role)).FirstOrDefault();
            if (objMasterRole != null)
            {
                //back-end level;membership checking
                if (!Roles.IsUserInRole(objMasterUser.UserName, objMasterRole.RoleName))
                {
                    string[] getRoles = Roles.GetRolesForUser(objMasterUser.UserName);
                    if (getRoles.Count() > 0)
                    {
                        Roles.RemoveUserFromRoles(objMasterUser.UserName, getRoles);
                    }
                    Roles.AddUserToRole(objMasterUser.UserName, objMasterRole.RoleName);
                }

                //local level checking
                JuncUserRole objUserRole = new JuncUserRole();
                objUserRole.RoleID = Convert.ToInt32(role);
                objUserRole.UserID = objMasterUser.UserID;
                objUserRole.Status = "A";

                if (new UsersRoleDAL().UserRoleFunc(objUserRole))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ChangePageMode(Helper.PageMode.New);
            ClearPageData();
            form_Wiz.Visible = true;
            //Response.Redirect(Request.RawUrl);
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

        private void LoadDropDown()
        {
            try
            {
                status.DataSource = Enum.GetValues(typeof(Helper.ItemStatus));
                status.DataBind();

                role.DataSource = new UsersRoleDAL().GetRoles();
                role.DataTextField = "RoleName";
                role.DataValueField = "RoleID";
                role.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void ChangePageMode(Helper.PageMode pagemode)
        {
            switch (pagemode)
            {
                case Helper.PageMode.New:
                    username.Attributes.Remove("readonly");
                    question.Attributes.Remove("readonly");
                    answer.Attributes.Remove("readonly");
                    widget_title.InnerText = "User Setup - New";
                    pwdDiv.Visible = true;
                    pwd2Div.Visible = true;
                    SecAns.Visible = true;
                    break;
                case Helper.PageMode.Edit:
                    username.Attributes.Add("readonly", "readonly");
                    question.Attributes.Add("readonly", "readonly");
                    answer.Attributes.Add("readonly", "readonly");
                    widget_title.InnerText = "User Setup - Edit";
                    pwdDiv.Visible = false;
                    pwd2Div.Visible = false;
                    SecAns.Visible = false;
                    break;
            }
            Session["UsersPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            try
            {
                username.Value = string.Empty;
                password.Value = string.Empty;
                password2.Value = string.Empty;
                email.Value = string.Empty;
                question.Value = string.Empty;
                answer.Value = string.Empty;

                status.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvUsers.Rows)
                    gvr.Style["background-color"] = "";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<MasterUser> data = (List<MasterUser>)Session["MasterUserData"];
                var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[4].FindControl("CustomStatus"));
                
                int UserId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "UserID"));
                string UserStatus = data.Where(x => x.UserID==UserId).Select(y => y.UserStatus).FirstOrDefault();
                bool LockedOut = Membership.GetUser(e.Row.Cells[1].Text).IsLockedOut;

                if (LockedOut == true)
                {
                    span.InnerHtml = "<span class=\"label label-sm label-warning arrowed-in arrowed-in-right tooltip-warning\" data-rel=\"tooltip\" " +
                                     "data-placement=\"right\" title=\"Account has been locked out because of too many invalid login attempts.\">Locked</span>";
                }
                else
                {
                    if (UserStatus == "A")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                    }
                    else if (UserStatus == "D")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                    }
                }
            }
        }
    }
}