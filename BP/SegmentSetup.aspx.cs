using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Classes;
using DAL;
using System.Data;

namespace BP
{
    public partial class SegmentSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                cdaccflag.Checked = false;
                GetData();
                Session["SegmentPageMode"] = Helper.PageMode.New;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                ChangePageMode(Helper.PageMode.New);
                ClearPageData();
                EditForm.Visible = true;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if ((Helper.PageMode)Session["SegmentPageMode"] == Helper.PageMode.New)
                {
                    if (new SegmentDAL().GetSegments().Where(x => x.SegmentName.ToUpper().Trim() == tbSegName.Text.ToUpper().Trim()).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "Segment already exists");
                        return;
                    }
                    if (new SegmentDAL().GetSegments().Where(x => x.SegmentOrder == Convert.ToInt32(tbSegOrder.Text.Trim())).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "Please change Segment order");
                        return;
                    }
                    Segment objSegment = new Segment();
                    objSegment.SegmentName = tbSegName.Text.Trim();
                    objSegment.ShapeFormat = string.Empty.PadRight(tbSegFormat.Text.Trim().Length, '?');
                    objSegment.SegmentOrder = Convert.ToInt32(tbSegOrder.Text.Trim());
                    objSegment.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objSegment.CreatedBy = LoggedInUser.UserID;
                    objSegment.CreatedTimeStamp = DateTime.Now;
                    objSegment.ModifiedBy = LoggedInUser.UserID;
                    objSegment.ModifiedTimeStamp = DateTime.Now;
                    objSegment.AccountCodeFlag = (cdaccflag.Checked) ? true : false;

                    if (new SegmentDAL().InsertSegment(objSegment))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Segment saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Segment");
                }
                else if ((Helper.PageMode)Session["SegmentPageMode"] == Helper.PageMode.Edit)
                {
                    Segment objSegment = (Segment)Session["SelectedSegment"];

                    Segment seg = new SegmentDAL().GetSegments().Where(x => x.SegmentOrder == Convert.ToInt32(tbSegOrder.Text.Trim())).FirstOrDefault();
                    if (seg != null)
                    {
                        if (seg.SegmentID != objSegment.SegmentID)
                        {
                            ((SiteMaster)this.Master).ShowMessage("Failure", "Please change Segment order");
                            return;
                        }
                    }
                    objSegment.SegmentName = tbSegName.Text.Trim();
                    objSegment.ShapeFormat = string.Empty.PadRight(tbSegFormat.Text.Trim().Length, '?');
                    objSegment.SegmentOrder = Convert.ToInt32(tbSegOrder.Text.Trim());
                    objSegment.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                    objSegment.ModifiedBy = LoggedInUser.UserID;
                    objSegment.ModifiedTimeStamp = DateTime.Now;
                    objSegment.AccountCodeFlag = (cdaccflag.Checked) ? true : false;

                    if (new SegmentDAL().UpdateSegment(objSegment))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Segment updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Segment");
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

        protected void gvSegmentSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();
                    GridViewRow selectedRow = gvSegmentSetup.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    Segment objSegment = new Segment();
                    objSegment.SegmentID = Convert.ToInt32(gvSegmentSetup.DataKeys[selectedRow.RowIndex]["SegmentID"]);
                    objSegment.SegmentName = selectedRow.Cells[1].Text;
                    objSegment.ShapeFormat = selectedRow.Cells[2].Text;
                    objSegment.SegmentOrder = Convert.ToInt32(selectedRow.Cells[3].Text);
                    objSegment.Status = new SegmentDAL().GetSegments().Where(x => x.SegmentID == objSegment.SegmentID).Select(y => y.Status).FirstOrDefault();
                    objSegment.AccountCodeFlag = new SegmentDAL().GetSegments().Where(x => x.SegmentID == objSegment.SegmentID)
                        .Select(y => y.AccountCodeFlag).FirstOrDefault() ?? false;
                    //objSegment.Status = selectedRow.Cells[4].Text;

                    Session["SelectedSegment"] = objSegment;

                    tbSegName.Text = objSegment.SegmentName;
                    tbSegFormat.Attributes.Add("value", objSegment.ShapeFormat);
                    tbSegOrder.Text = objSegment.SegmentOrder.ToString();
                    ddlStatus.SelectedIndex = -1;
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objSegment.Status))).Selected = true;
                    cdaccflag.Checked = (bool)objSegment.AccountCodeFlag;

                    ChangePageMode(Helper.PageMode.Edit);
                    EditForm.Visible = true;
                }

                if (e.CommandName == "EditDetails")
                {
                    GridViewRow selectedRow = gvSegmentSetup.Rows[Convert.ToInt32(e.CommandArgument)];

                    Segment objSegment = new Segment();
                    objSegment.SegmentID = Convert.ToInt32(gvSegmentSetup.DataKeys[selectedRow.RowIndex]["SegmentID"]);
                    objSegment.SegmentName = selectedRow.Cells[1].Text;
                    objSegment.ShapeFormat = selectedRow.Cells[2].Text;
                    objSegment.SegmentOrder = Convert.ToInt32(selectedRow.Cells[3].Text);
                    objSegment.Status = selectedRow.Cells[4].Text;
                    objSegment.AccountCodeFlag = new SegmentDAL().GetSegments().Where(x => x.SegmentID == objSegment.SegmentID)
                        .Select(y => y.AccountCodeFlag).FirstOrDefault() ?? false;

                    Session["SelectedSegment"] = objSegment;

                    Response.Redirect("~/SegmentDetails.aspx");
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
                List<Segment> data = new SegmentDAL().GetSegments();
                Session["SegmentData"] = data.OrderBy(x => x.SegmentOrder).ThenBy(x => x.SegmentName).ToList();
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
                gvSegmentSetup.DataSource = (List<Segment>)Session["SegmentData"];
                gvSegmentSetup.DataBind();
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
                    tbSegName.ReadOnly = false;
                    widget_title.InnerText = "Segment - New";
                    break;
                case Helper.PageMode.Edit:
                    tbSegName.ReadOnly = true;
                    widget_title.InnerText = "Segment - Edit";
                    break;
            }
            Session["SegmentPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            tbSegName.Text = string.Empty;
            tbSegFormat.Text = string.Empty;
            tbSegFormat.Attributes.Add("value", "");
            tbSegOrder.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            cdaccflag.Checked = false;

            foreach (GridViewRow gvr in gvSegmentSetup.Rows)
                gvr.Style["background-color"] = "";
        }

        protected void gvSegmentSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<Segment> data = (List<Segment>)Session["SegmentData"];
                var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[4].FindControl("CustomStatus"));

                int segmentid = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SegmentID"));
                string SegmentStatus = data.Where(x => x.SegmentID == segmentid).Select(y => y.Status).FirstOrDefault();

                if (SegmentStatus == "A")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                }
                else if (SegmentStatus == "D")
                {
                    span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                    "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                }
            }
        }

        protected void gvSegmentSetup_PreRender(object sender, EventArgs e)
        {
            if (gvSegmentSetup.Rows.Count > 0)
            {
                gvSegmentSetup.UseAccessibleHeader = true;
                gvSegmentSetup.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvSegmentSetup.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}