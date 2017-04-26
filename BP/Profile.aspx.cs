using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;
using OBSecurity;
using System.Web.UI.HtmlControls;

namespace BP
{
    public partial class Profile : System.Web.UI.Page
    {
        MasterUser AuthUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthUser = (MasterUser)Session["UserData"];
            if (Session["UserData"] == null)
            {
                Response.Redirect("~/Setup/Login.aspx");
            }

            if (!Page.IsPostBack)
            {
                PopulateProfile();
            }
        }

        protected void PopulateProfile()
        {
            string src = "~/assets/images/avatars/profile-pic.jpg";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopulateImage", "PopulateImage('" + src + "');", true);
            
            username.Value = AuthUser.UserName;
            fullname.Value = AuthUser.FullName;

            //build birthdate with ic - start
            string ICno = AuthUser.UserIC;
            int intYear = Convert.ToInt32(ICno.Substring(0, 2));
            if (intYear + 2000 < DateTime.Today.Year) { intYear = 2000 + intYear; } else { intYear = intYear + 1900; }
            string strDate = Convert.ToInt32(ICno.Substring(4, 2)) + "-" + Convert.ToInt32(ICno.Substring(3, 2)) + "-" + intYear;
            birthdate.Value = strDate;
            //build birthdate with ic - end

            email.Value = AuthUser.UserEmail;
            phone.Value = AuthUser.UserPhoneNo;

            question.InnerText = AuthUser.SecQuestion;
            answer.InnerText = (AuthUser.SecAnswer != null && AuthUser.SecAnswer != string.Empty) ? Security.Decrypt(AuthUser.SecAnswer) : string.Empty;
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            try
            {
                Byte[] imgByte = null;

                if (imgUpload.HasFile && imgUpload.PostedFile != null)
                {
                    HttpPostedFile File = imgUpload.PostedFile;
                    imgByte = new Byte[File.ContentLength];
                    File.InputStream.Read(imgByte, 0, File.ContentLength);
                }

                MasterUser objMasterUser = new MasterUser();
                objMasterUser.FullName = fullname.Value.Trim();
                //string strDate = DateTime.Now.ToString("MM/dd/yyyy");

                //objMasterUser.BirthDate = Convert.ToDateTime(birthdate.Value.ToString("dd/MM/yyyy"));

            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }
    }
}