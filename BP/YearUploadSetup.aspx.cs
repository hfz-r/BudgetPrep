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
    public partial class YearUploadSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetData();
                Session["YearUploadPageMode"] = Helper.PageMode.New;
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
                if ((Helper.PageMode)Session["YearUploadPageMode"] == Helper.PageMode.New)
                {
                    if (new YearUploadDAL().GetYearUpload().Where(x => x.BudgetYear == Convert.ToInt32(ddlYear.SelectedValue)).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "Budget Year already exists");
                        return;
                    }

                    DAL.YearUploadSetup objYearUpload = new DAL.YearUploadSetup();
                    objYearUpload.BudgetYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objYearUpload.BudgetYearDesc = tbDesc.Text;
                    objYearUpload.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objYearUpload.CreatedBy = LoggedInUser.UserID;
                    objYearUpload.CreatedTimeStamp = DateTime.Now;
                    objYearUpload.ModifiedBy = LoggedInUser.UserID;
                    objYearUpload.ModifiedTimeStamp = DateTime.Now;

                    if (new YearUploadDAL().InsertYearUpload(objYearUpload))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Budget Year saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while processing Budget Year");
                }
                else if ((Helper.PageMode)Session["YearUploadPageMode"] == Helper.PageMode.Edit)
                {
                    DAL.YearUploadSetup objYearUpload = (DAL.YearUploadSetup)Session["SelectedYearUpload"];

                    DAL.YearUploadSetup pm = new YearUploadDAL().GetYearUpload()
                        .Where(x => x.BudgetYear == Convert.ToInt32(ddlYear.SelectedValue)).FirstOrDefault();

                    if (pm != null)
                    {
                        if (pm.BudgetYearID != objYearUpload.BudgetYearID)
                        {
                            ((SiteMaster)this.Master).ShowMessage("Failure", "Budget Year already exists");
                            return;
                        }
                    }

                    objYearUpload.BudgetYear = Convert.ToInt32(ddlYear.SelectedValue);
                    objYearUpload.BudgetYearDesc = tbDesc.Text;
                    objYearUpload.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objYearUpload.ModifiedBy = LoggedInUser.UserID;
                    objYearUpload.ModifiedTimeStamp = DateTime.Now;

                    if (new YearUploadDAL().UpdateYearUpload(objYearUpload))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Budget Year updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Budget Year");
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
        //    EditForm.Visible = false;
        //}

        protected void gvYearUploadSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();
                    GridViewRow selectedRow = gvYearUploadSetup.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    int budgetyearid = Convert.ToInt32(gvYearUploadSetup.DataKeys[selectedRow.RowIndex]["BudgetYearID"]);
                    DAL.YearUploadSetup objYearUpload = ((List<DAL.YearUploadSetup>)Session["YearUploadData"]).Where(x => x.BudgetYearID == budgetyearid).FirstOrDefault();

                    Session["SelectedYearUpload"] = objYearUpload;

                    ddlYear.SelectedIndex = -1;
                    ddlYear.Items.FindByValue(objYearUpload.BudgetYear.ToString()).Selected = true;

                    ddlStatus.SelectedIndex = -1;
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objYearUpload.Status))).Selected = true;

                    tbDesc.Text = objYearUpload.BudgetYearDesc;

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
                List<DAL.YearUploadSetup> data = new YearUploadDAL().GetYearUpload();
                Session["YearUploadData"] = data;
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
                gvYearUploadSetup.DataSource = ((List<DAL.YearUploadSetup>)Session["YearUploadData"])
                                        .Select(x => new DAL.YearUploadSetup()
                                        {
                                            BudgetYearID = x.BudgetYearID,
                                            BudgetYear = x.BudgetYear,
                                            BudgetYearDesc = x.BudgetYearDesc,
                                            Status = ((x.Status == "A") ? "A" : "D")
                                        }).OrderBy(x => x.BudgetYear).ThenBy(x => x.BudgetYearDesc).ThenByDescending(x => x.Status).ToList();
                gvYearUploadSetup.DataBind();
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
                    ddlYear.Enabled = true;
                    ddlStatus.Enabled = true;
                    tbDesc.Text = String.Empty;
                    widget_title.InnerText = "User Setup - New";
                    break;
                case Helper.PageMode.Edit:
                    ddlYear.Enabled = false;
                    widget_title.InnerText = "User Setup - Edit";
                    break;
            }
            Session["YearUploadPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            try
            {
                ddlYear.SelectedIndex = 0;
                ddlStatus.SelectedIndex = 0;
                tbDesc.Text = String.Empty;

                foreach (GridViewRow gvr in gvYearUploadSetup.Rows)
                    gvr.Style["background-color"] = "";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvYearUploadSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<DAL.YearUploadSetup> data = (List<DAL.YearUploadSetup>)Session["YearUploadData"];
                var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));

                int budgetyearid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BudgetYearID"));
                string BudgetStatus = data.Where(x => x.BudgetYearID == budgetyearid).Select(y => y.Status).FirstOrDefault();

                if (BudgetStatus == "A")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                }
                else if (BudgetStatus == "D")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                }
            }
        }

        protected void gvYearUploadSetup_PreRender(object sender, EventArgs e)
        {
            if (gvYearUploadSetup.Rows.Count > 0)
            {
                gvYearUploadSetup.UseAccessibleHeader = true;
                gvYearUploadSetup.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvYearUploadSetup.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}