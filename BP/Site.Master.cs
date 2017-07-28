using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using System.Diagnostics;

namespace BP
{
    public class PageMenuHelper
    {
        public int PageID { get; set; }
        public string PageName { get; set; }
        public string PagePath { get; set; }
        public int ParentPageID { get; set; }
        public int PageOrder { get; set; }
        public int MenuID { get; set; }
        public string MenuName { get; set; }
        public string MenuIcon { get; set; }
        public int MenuOrder { get; set; }
    }

    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        MasterUser AuthUser;

        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthUser = (MasterUser)Session["UserData"];
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "onrefLoad", "RefreshSession();", true);

            LoadImageHeader();

            if (!IsPostBack)
            {
                BuildMenu();
            }
        }

        public void LoadImageHeader()
        {
            try
            {
                string src = string.Empty;
                if (AuthUser.Image != null)
                {
                    src = "~/ShowImage.ashx?UserId=" + AuthUser.UserID;

                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "LoadImage", "LoadImage('" + src + "');", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error", "Internal error occurred", ex, true);
            }
        }

        private void BuildMenu()
        {
            List<PageMenuHelper> lstPages = (List<PageMenuHelper>)Session["ListPages"];

            string ActiveClassDashboard = "\"\"";
            string ActiveClassInbox = "\"\"";

            string PATH = HttpContext.Current.Request.Url.AbsolutePath;
            if (PATH.Contains("/Dashboard.aspx"))
            {
                ActiveClassDashboard = "\"active\"";
            }
            if (PATH.Contains("/MailInbox.aspx"))
            {
                ActiveClassInbox = "\"active\"";
            }

            //dashboard
            string DashboardMenu = "<li class=" + ActiveClassDashboard + "><a href=\"" + (HttpContext.Current.Handler as Page).ResolveUrl("~/Dashboard.aspx") + "\"><i class=\"menu-icon fa fa-tachometer\"></i>" +
                "<span class=\"menu-text\"> Dashboard </span></a><b class=\"arrow\"></b></li>";

            //inbox
            string strInbox = (AuthUser.JuncUserRoles.First().RoleID == 3) ? "Event Log" : "Inbox"; //<span class=\"badge\">10</span>
            string InboxMenu = "<li class=" + ActiveClassInbox + "><a href=\"" + (HttpContext.Current.Handler as Page).ResolveUrl("~/MailInbox.aspx") + "\"><i class=\"menu-icon fa fa-envelope\"></i>" +
                "<span class=\"menu-text\"> " + strInbox + " </span></a><b class=\"arrow\"></b></li>";
            
            string MenuBuilder = string.Empty;
            foreach (int MenuId in lstPages.Where(x => x.ParentPageID == 0).OrderBy(x => x.MenuOrder).Select(x => x.MenuID).Distinct())
            {
                PageMenuHelper MenuLVL1 = lstPages.Where(x => x.MenuID == MenuId).FirstOrDefault();

                string ActiveClassMenuLVL1 = "\"\"";
                if (MenuId == lstPages.Where(x => x.PagePath.Contains(Request.Path)).Select(y => y.MenuID).FirstOrDefault())
                {
                    ActiveClassMenuLVL1 = "\"active open\"";
                }

                MenuBuilder = MenuBuilder + "<li class=" + ActiveClassMenuLVL1 + "><a href=\"#\" class=\"dropdown-toggle\"><i class=\"" + MenuLVL1.MenuIcon + "\"></i>";
                MenuBuilder = MenuBuilder + "<span class=\"menu-text\"> " + MenuLVL1.MenuName + " </span><b class=\"arrow fa fa-angle-down\"></b></a>";
                MenuBuilder = MenuBuilder + "<b class=\"arrow\"></b><ul class=\"submenu\">";

                foreach (PageMenuHelper MenuLVL2 in lstPages.Where(x => x.ParentPageID == 0 && x.MenuID == MenuId).OrderBy(x => x.PageOrder))
                {
                    string ActiveClassMenuLVL2 = "\"\"";
                    if (MenuLVL2.PagePath.Contains(Request.Path))
                    {
                        ActiveClassMenuLVL2 = "\"active\"";
                    }
                    MenuBuilder = MenuBuilder + "<li class=" + ActiveClassMenuLVL2 + "><a href=\"" + (HttpContext.Current.Handler as Page).ResolveUrl(MenuLVL2.PagePath) + "\">";
                    MenuBuilder = MenuBuilder + "<i class=\"menu-icon fa fa-caret-right\"></i> " + MenuLVL2.PageName + " </a><b class=\"arrow\"></b></li>";
                }
                MenuBuilder = MenuBuilder + "</ul></li>";
            }

            MENU.InnerHtml = "<ul class=\"nav nav-list\">" + DashboardMenu + InboxMenu + MenuBuilder + "</ul>";
        }

        public void ShowMessage(string Title, string Body)
        {
            lblModalTitle.Text = Title;
            lblModalBody.Text = Body;
            divModalDetail.InnerHtml = string.Empty;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myMsgModal", "$('#myMsgModal').modal();", true);
        }

        public void ShowMessage(string Title, string Body, Exception Exception, bool LogError)
        {
            lblModalTitle.Text = Title;
            lblModalBody.Text = Body;
            divModalDetail.InnerHtml = Exception.Message;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myMsgModal", "$('#myMsgModal').modal();", true);

            if (LogError && !Exception.Message.Contains("Thread was being aborted"))
            {
                try
                {
                    if (AuthUser == null)
                        AuthUser = (MasterUser)Session["UserData"];

                    StackTrace t = new StackTrace();
                    System.Reflection.MethodBase mb = t.GetFrame(1).GetMethod();

                    BPEventLog bpe = new BPEventLog();
                    bpe.Object = mb.ReflectedType.Name;
                    bpe.ObjectName = mb.Name;
                    bpe.ObjectChanges = Exception.Message;
                    bpe.EventMassage = string.Empty;
                    bpe.Status = "E";
                    bpe.CreatedBy = AuthUser.UserID;
                    bpe.CreatedTimeStamp = DateTime.Now;
                    new EventLogDAL().AddEventLog(bpe);
                }
                catch (Exception ex)
                {
                    ShowMessage("Error", "Internal error occurred", ex, true);
                }
            }
        }
    }
}