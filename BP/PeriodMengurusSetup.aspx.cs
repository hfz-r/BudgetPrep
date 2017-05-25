using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Classes;
using DAL;

namespace BP
{
    public class PeriodMengurusGridHelper
    {
        public int PeriodMengurusID { get; set; }
        public int FieldMengurusID { get; set; }
        public int MengurusYear { get; set; }
        public string FieldMengurus { get; set; }
        public string Status { get; set; }
    }

    public partial class PeriodMengurusSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetData();
                Session["PeriodMenguruPageMode"] = Helper.PageMode.New;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ChangePageMode(Helper.PageMode.New);
            ClearPageData();
            EditForm.Visible = true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int FieldID = new FieldMenguruDAL().GetFieldMengurus().Where(x => x.FieldMengurusDesc.Contains("Anggaran Dipohon") && 
                    x.Status == "A").Select(y => y.FieldMengurusID).FirstOrDefault();

                if ((Helper.PageMode)Session["PeriodMenguruPageMode"] == Helper.PageMode.New)
                {
                    if (new PeriodMengurusDAL().GetPeriodMengurus().Where(x => x.FieldMengurusID == FieldID && x.MengurusYear == Convert.ToInt32(ddlYear.SelectedValue)).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "Period Mengurus already exists");
                        return;
                    }

                    PeriodMenguru objPeriodMenguru = new PeriodMenguru();
                    objPeriodMenguru.FieldMengurusID = FieldID;
                    objPeriodMenguru.MengurusYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objPeriodMenguru.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objPeriodMenguru.CreatedBy = LoggedInUser.UserID;
                    objPeriodMenguru.CreatedTimeStamp = DateTime.Now;
                    objPeriodMenguru.ModifiedBy = LoggedInUser.UserID;
                    objPeriodMenguru.ModifiedTimeStamp = DateTime.Now;

                    if (new PeriodMengurusDAL().InsertPeriodMenguru(objPeriodMenguru))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Period Mengurus saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Period Mengurus");
                }
                else if ((Helper.PageMode)Session["PeriodMenguruPageMode"] == Helper.PageMode.Edit)
                {
                    PeriodMenguru objPeriodMenguru = (PeriodMenguru)Session["SelectedPeriodMenguru"];

                    PeriodMenguru pm = new PeriodMengurusDAL().GetPeriodMengurus().Where(x => x.FieldMengurusID == FieldID && x.MengurusYear == Convert.ToInt32(ddlYear.SelectedValue)).FirstOrDefault();
                    if (pm != null)
                    {
                        if (pm.PeriodMengurusID != objPeriodMenguru.PeriodMengurusID)
                        {
                            ((SiteMaster)this.Master).ShowMessage("Failure", "Period Mengurus already exists");
                            return;
                        }
                    }

                    objPeriodMenguru.FieldMengurusID = FieldID;
                    objPeriodMenguru.MengurusYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objPeriodMenguru.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objPeriodMenguru.ModifiedBy = LoggedInUser.UserID;
                    objPeriodMenguru.ModifiedTimeStamp = DateTime.Now;

                    if (new PeriodMengurusDAL().UpdatePeriodMenguru(objPeriodMenguru))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Period Mengurus updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Period Mengurus");
                }

                GetData();
                ClearPageData();
                EditForm.Visible = false;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    ChangePageMode(Helper.PageMode.New);
        //    ClearPageData();
        //    EditBox.Visible = false;
        //}

        protected void gvPeriodMenguruSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();
                    GridViewRow selectedRow = gvPeriodMenguruSetup.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    PeriodMenguru objPeriodMenguru = new PeriodMenguru();
                    objPeriodMenguru.PeriodMengurusID = Convert.ToInt32(gvPeriodMenguruSetup.DataKeys[selectedRow.RowIndex]["PeriodMengurusID"]);
                    objPeriodMenguru.FieldMengurusID = Convert.ToInt32(gvPeriodMenguruSetup.DataKeys[selectedRow.RowIndex]["FieldMengurusID"]);
                    objPeriodMenguru.MengurusYear = Convert.ToInt32(selectedRow.Cells[1].Text);
                    objPeriodMenguru.Status = new PeriodMengurusDAL().GetPeriodMengurus().Where(x => x.PeriodMengurusID == objPeriodMenguru.PeriodMengurusID)
                        .Select(y => y.Status).FirstOrDefault();

                    Session["SelectedPeriodMenguru"] = objPeriodMenguru;

                    ddlYear.SelectedIndex = -1;
                    ddlYear.Items.FindByValue(objPeriodMenguru.MengurusYear.ToString()).Selected = true;
                    ddlStatus.SelectedIndex = -1;
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objPeriodMenguru.Status))).Selected = true;

                    ChangePageMode(Helper.PageMode.Edit);
                    EditForm.Visible = true;
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void GetData()
        {
            try
            {
                List<PeriodMengurusGridHelper> data = new PeriodMengurusDAL().GetPeriodMengurus()
                    .Select(x => new PeriodMengurusGridHelper()
                    {
                        PeriodMengurusID = x.PeriodMengurusID,
                        FieldMengurusID = x.FieldMengurusID,
                        FieldMengurus = x.FieldMenguru.FieldMengurusDesc,
                        MengurusYear = x.MengurusYear,
                        Status = ((x.Status == "A") ? "A" : "D")
                    }).OrderBy(x => x.MengurusYear).ToList();

                Session["PeriodMenguruData"] = data;
                BindGrid();
                LoadDropDown();
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
                gvPeriodMenguruSetup.DataSource = (List<PeriodMengurusGridHelper>)Session["PeriodMenguruData"];
                gvPeriodMenguruSetup.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void LoadDropDown()
        {
            try
            {
                ddlStatus.DataSource = Enum.GetValues(typeof(Helper.ItemStatus));
                ddlStatus.DataBind();

                for (int i = DateTime.Now.Year; i <= DateTime.Now.Year + 10; i++)
                {
                    ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }

                List<FieldMenguru> obj = new FieldMenguruDAL().GetFieldMengurus().Where(x => x.FieldMengurusDesc.Contains("Anggaran Dipohon") && x.Status == "A").ToList();
                foreach (var item in obj)
                {
                    tbFieldMengurus.Text = string.Format("{0}", item.FieldMengurusDesc);
                }
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
                    widget_title.InnerText = "Period Mengurus - New";
                    break;
                case Helper.PageMode.Edit:
                    widget_title.InnerText = "Period Mengurus - Edit";
                    break;
            }
            Session["PeriodMenguruPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            try
            {
                ddlYear.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvPeriodMenguruSetup.Rows)
                    gvr.Style["background-color"] = "";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvPeriodMenguruSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<PeriodMengurusGridHelper> data = (List<PeriodMengurusGridHelper>)Session["PeriodMenguruData"];
                var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));

                int periodmengurusid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PeriodMengurusID"));
                int fieldmengurusid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FieldMengurusID"));
                string PeriodStatus = data.Where(x => x.PeriodMengurusID == periodmengurusid && x.FieldMengurusID == fieldmengurusid).Select(y => y.Status).FirstOrDefault();

                if (PeriodStatus == "A")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                }
                else if (PeriodStatus == "D")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                }
            }
        }

        protected void gvPeriodMenguruSetup_PreRender(object sender, EventArgs e)
        {
            if (gvPeriodMenguruSetup .Rows.Count > 0)
            {
                gvPeriodMenguruSetup.UseAccessibleHeader = true;
                gvPeriodMenguruSetup.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPeriodMenguruSetup.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}