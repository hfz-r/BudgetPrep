
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DAL;

namespace BP.Classes
{
    public class PageHelper : System.Web.UI.Page
    {
<<<<<<< HEAD
=======
        //public MasterUser LoggedInUser = new MasterUser();
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
        public MasterUser LoggedInUser { get; set; }
        public PageHelper()
        {
            LoggedInUser = new MasterUser();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["UserData"] == null)
            {
                ClearSession();
            }
            else
            {
                LoggedInUser = (MasterUser)Session["UserData"];
                if (LoggedInUser.SecQuestion == null || LoggedInUser.SecAnswer == null)
<<<<<<< HEAD
                    Response.Redirect("~/Setup/Login.aspx");
                if (LoggedInUser.SecQuestion.Trim() == string.Empty || LoggedInUser.SecAnswer.Trim() == string.Empty)
                    Response.Redirect("~/Setup/Login.aspx");
            }

            List<PageMenuHelper> lstPages = (List<PageMenuHelper>)Session["ListPages"];
            if (!Request.Path.ToUpper().Contains(("/Dashboard.aspx").ToUpper()) && !Request.Path.ToUpper().Contains(("/MailInBox.aspx").ToUpper()))
            {
                if (lstPages.Where(x => Request.Path.ToUpper().Contains(x.PagePath.Replace("~/", "").Trim().ToUpper())).Count() == 0)
                {
                    ClearSession();
                }
            }
=======
                    Response.Redirect("~/UserSettings.aspx");
                if (LoggedInUser.SecQuestion.Trim() == string.Empty || LoggedInUser.SecAnswer.Trim() == string.Empty)
                    Response.Redirect("~/UserSettings.aspx");
            }

            //List<PageMenuHelper> lstPages = (List<PageMenuHelper>)Session["ListPages"];
            //if (!Request.Path.ToUpper().Contains(("Default.aspx").ToUpper()) && !Request.Path.ToUpper().Contains(("MailInBox.aspx").ToUpper()))
            //{
            //    if (lstPages.Where(x => Request.Path.ToUpper().Contains(x.PagePath.Replace("./", "").Trim().ToUpper())).Count() == 0)
            //    {
            //        ClearSession();
            //    }
            //}
>>>>>>> fa2a2893ae1d7e783d8591f454ef428f3a40756b
        }

        private void ClearSession()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Response.Redirect("~/Setup/Login.aspx");
        }
    }
}