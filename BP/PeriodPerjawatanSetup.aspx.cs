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
    public class PeriodPerjawatanGridHelper
    {
        public int PeriodPerjawatanID { get; set; }
        public int FieldPerjawatanID { get; set; }
        public int PerjawatanYear { get; set; }
        public string FieldPerjawatan { get; set; }
        public string Status { get; set; }
    }

    public partial class PeriodPerjawatanSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetData();
                Session["PeriodPerjawatanPageMode"] = Helper.PageMode.New;
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
                int FieldID = new FieldPerjawatanDAL().GetFieldPerjawatans().Where(x => x.FieldPerjawatanDesc.Contains("Bil Perjawatan") &&
                    x.Status == "A").Select(y => y.FieldPerjawatanID).FirstOrDefault();

                if ((Helper.PageMode)Session["PeriodPerjawatanPageMode"] == Helper.PageMode.New)
                {
                    if (new PeriodPerjawatanDAL().GetPeriodPerjawatans().Where(x => x.FieldPerjawatanID == FieldID && x.PerjawatanYear == Convert.ToInt32(ddlYear.SelectedValue)).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "Period Perjawatan already exists");
                        return;
                    }

                    PeriodPerjawatan objPeriodPerjawatan = new PeriodPerjawatan();
                    objPeriodPerjawatan.FieldPerjawatanID = FieldID;
                    objPeriodPerjawatan.PerjawatanYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objPeriodPerjawatan.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objPeriodPerjawatan.CreatedBy = LoggedInUser.UserID;
                    objPeriodPerjawatan.CreatedTimeStamp = DateTime.Now;
                    objPeriodPerjawatan.ModifiedBy = LoggedInUser.UserID;
                    objPeriodPerjawatan.ModifiedTimeStamp = DateTime.Now;

                    if (new PeriodPerjawatanDAL().InsertPeriodPerjawatan(objPeriodPerjawatan))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Period Perjawatan saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Period Perjawatan");
                }
                else if ((Helper.PageMode)Session["PeriodPerjawatanPageMode"] == Helper.PageMode.Edit)
                {
                    PeriodPerjawatan objPeriodPerjawatan = (PeriodPerjawatan)Session["SelectedPeriodPerjawatan"];

                    PeriodPerjawatan pp = new PeriodPerjawatanDAL().GetPeriodPerjawatans().Where(x => x.FieldPerjawatanID == FieldID && x.PerjawatanYear == Convert.ToInt32(ddlYear.SelectedValue)).FirstOrDefault();
                    if (pp != null)
                    {
                        if (pp.PeriodPerjawatanID != objPeriodPerjawatan.PeriodPerjawatanID)
                        {
                            ((SiteMaster)this.Master).ShowMessage("Failure", "Period Perjawatan already exists");
                            return;
                        }
                    }

                    objPeriodPerjawatan.FieldPerjawatanID = FieldID;
                    objPeriodPerjawatan.PerjawatanYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objPeriodPerjawatan.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objPeriodPerjawatan.ModifiedBy = LoggedInUser.UserID;
                    objPeriodPerjawatan.ModifiedTimeStamp = DateTime.Now;

                    if (new PeriodPerjawatanDAL().UpdatePeriodPerjawatan(objPeriodPerjawatan))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Period Perjawatan updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Period Perjawatan");
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

        protected void gvPeriodPerjawatanSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();
                    GridViewRow selectedRow = gvPeriodPerjawatanSetup.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    PeriodPerjawatan objPeriodPerjawatan = new PeriodPerjawatan();
                    objPeriodPerjawatan.PeriodPerjawatanID = Convert.ToInt32(gvPeriodPerjawatanSetup.DataKeys[selectedRow.RowIndex]["PeriodPerjawatanID"]);
                    objPeriodPerjawatan.FieldPerjawatanID = Convert.ToInt32(gvPeriodPerjawatanSetup.DataKeys[selectedRow.RowIndex]["FieldPerjawatanID"]);
                    objPeriodPerjawatan.PerjawatanYear = Convert.ToInt32(selectedRow.Cells[1].Text);
                    objPeriodPerjawatan.Status = new PeriodPerjawatanDAL().GetPeriodPerjawatans().Where(x => x.PeriodPerjawatanID == objPeriodPerjawatan.PeriodPerjawatanID)
                        .Select(y => y.Status).FirstOrDefault();

                    Session["SelectedPeriodPerjawatan"] = objPeriodPerjawatan;

                    ddlYear.SelectedIndex = -1;
                    ddlYear.Items.FindByValue(objPeriodPerjawatan.PerjawatanYear.ToString()).Selected = true;
                    ddlStatus.SelectedIndex = -1;
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objPeriodPerjawatan.Status))).Selected = true;

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
                List<PeriodPerjawatanGridHelper> data = new PeriodPerjawatanDAL().GetPeriodPerjawatans()
                    .Select(x => new PeriodPerjawatanGridHelper()
                    {
                        PeriodPerjawatanID = x.PeriodPerjawatanID,
                        FieldPerjawatanID = x.FieldPerjawatanID,
                        FieldPerjawatan = x.FieldPerjawatan.FieldPerjawatanDesc,
                        PerjawatanYear = x.PerjawatanYear,
                        Status = ((x.Status == "A") ? "A" : "D")
                    }).OrderBy(x => x.PerjawatanYear).ToList();

                Session["PeriodPerjawatanData"] = data;
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
                gvPeriodPerjawatanSetup.DataSource = (List<PeriodPerjawatanGridHelper>)Session["PeriodPerjawatanData"];
                gvPeriodPerjawatanSetup.DataBind();
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

                List<FieldPerjawatan> obj = new FieldPerjawatanDAL().GetFieldPerjawatans().Where(x => x.FieldPerjawatanDesc.Contains("Bil Perjawatan") && x.Status == "A").ToList();
                foreach (var item in obj)
                {
                    tbFieldPerjawatan.Text = string.Format("{0}", item.FieldPerjawatanDesc);
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
                    widget_title.InnerText = "Period Perjawatan - New";
                    break;
                case Helper.PageMode.Edit:
                    widget_title.InnerText = "Period Perjawatan - Edit";
                    break;
            }
            Session["PeriodPerjawatanPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            try
            {
                ddlYear.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvPeriodPerjawatanSetup.Rows)
                    gvr.Style["background-color"] = "";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvPeriodPerjawatanSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<PeriodPerjawatanGridHelper> data = (List<PeriodPerjawatanGridHelper>)Session["PeriodPerjawatanData"];
                var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));

                int periodperjawatanid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PeriodPerjawatanID"));
                int fieldperjawatanid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "FieldPerjawatanID"));
                string PeriodStatus = data.Where(x => x.PeriodPerjawatanID == periodperjawatanid && x.FieldPerjawatanID == fieldperjawatanid).Select(y => y.Status).FirstOrDefault();

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

        protected void gvPeriodPerjawatanSetup_PreRender(object sender, EventArgs e)
        {
            if (gvPeriodPerjawatanSetup.Rows.Count > 0)
            {
                gvPeriodPerjawatanSetup.UseAccessibleHeader = true;
                gvPeriodPerjawatanSetup.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPeriodPerjawatanSetup.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}