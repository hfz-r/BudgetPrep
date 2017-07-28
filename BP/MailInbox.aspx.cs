using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Classes;
using DAL;
using System.Web.UI.HtmlControls;
using System.Data;

namespace BP
{
    public class Mailbox
    {
        public string Object { get; set; }
        public string Time { get; set; }
        public string TimeInDetails { get; set; }
        public int NoCount { get; set; }
        public string Title { get; set; }
    }

    public partial class MailInbox : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AuthUser"] = LoggedInUser;

            LoadInboxImageHeader();

            if (!Page.IsPostBack)
            {
                BindListView();
            }

            BindDummyItem();
        }

        protected void BindListView()
        {
            List<Mailbox> mb = new EventLogDAL().GetInboxList(LoggedInUser)
                .Select(obj => new Mailbox
                {
                    Object = obj.Object,
                    Time = (obj.LastModDateTime.Date == DateTime.Today ?
                        obj.LastModDateTime.ToString("t") : (obj.LastModDateTime.Date == DateTime.Today.AddDays(-1) ?
                            "Yesterday" : obj.LastModDateTime.ToString("d MMM yyyy"))),
                    TimeInDetails = (obj.LastModDateTime.Date == DateTime.Today ?
                        "Today, " + obj.LastModDateTime.ToString("t") : (obj.LastModDateTime.Date == DateTime.Today.AddDays(-1) ?
                            "Yesterday, " + obj.LastModDateTime.ToString("t") : obj.LastModDateTime.ToString("dd MMM yyyy, HH:mm:ss"))),
                    NoCount = obj.NoCount,
                    Title = obj.Title
                }).ToList();

            ListView1.DataSource = mb;
            ListView1.DataBind();
            //total records
            TotalInbox1.InnerText = mb.Count().ToString();
            TotalInbox2.InnerText = mb.Count().ToString();
        }

        public void BindDummyItem()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] 
            { 
                new DataColumn("Title"), 
                new DataColumn("Object"),
                new DataColumn("Details"),
                new DataColumn("Modified By"),
                new DataColumn("Last Modified")
            });
            dt.Rows.Add();

            gvDetail.DataSource = dt;
            gvDetail.DataBind();
        }

        protected void ListView1_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            //set current page startindex, max rows and rebind to false
            DataPager1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            
            //rebind List View
            BindListView();
        }

        protected void ListView1_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                Mailbox obj = (Mailbox)dataItem.DataItem;

                //obj.TimeInDetails;
            }
        }

        public void LoadInboxImageHeader()
        {
            try
            {
                string src = string.Empty;
                if (LoggedInUser.Image != null)
                {
                    src = "~/ShowImage.ashx?UserId=" + LoggedInUser.UserID;

                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "InboxImage", "LoadInboxImage('" + src + "');", true);
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "Internal error occurred", ex, true);
            }
        }

        [System.Web.Services.WebMethod]
        public static InboxHelper[] PopulateInboxDetails(string title, string details)
        {
            return (new EventLogDAL().GetMailDetails(
                title.Trim(),
                details.Trim(),
                (MasterUser)HttpContext.Current.Session["AuthUser"])).ToArray();
        }
    }
}