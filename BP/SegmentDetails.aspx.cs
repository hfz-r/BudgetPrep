using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;

namespace BP
{
    public class SegmentDetailTreeHelper
    {
        public int SegmentDetailID { get; set; }
        public int SegmentID { get; set; }
        public string DetailCode { get; set; }
        public string DetailDesc { get; set; }
        public int ParentDetailID { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public int ChildCount { get; set; }
    }

    public class SegmentDetailImport
    {
        public string DetailCode { get; set; }
        public string DetailDesc { get; set; }
        public string Status { get; set; }
    }

    public partial class SegmentDetails : PageHelper
    {
        Segment selectedSegment;
        List<int> SelectedNodes;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                selectedSegment = (Segment)Session["SelectedSegment"];
                if (selectedSegment == null)
                {
                    Response.Redirect("~/SegmentSetup.aspx");
                }

                RegExpValidatorCode.ValidationExpression = "^[\\s\\S]{1," + selectedSegment.ShapeFormat.Length.ToString() + "}$";

                if (!Page.IsPostBack)
                {
                    Session["SelectedNodes"] = null;
                    Session["SelectedSegmentDetail"] = null;
                    
                    ChangePageMode(Helper.PageMode.New);
                    GetData();
                    CreateTreeData();
                    LoadDropDown();
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
                selectedSegment = (Segment)Session["SelectedSegment"];
                List<SegmentDetail> data = new SegmentDetailsDAL().GetSegmentDetails(selectedSegment.SegmentID);
                if (data.Count == 0)
                {
                    data = new List<SegmentDetail>();
                    data.Add(new SegmentDetail() { SegmentID = 0, SegmentDetailID = 0, ParentDetailID = 0 });

                    List<DAL.YearUploadSetup> GetData = new YearUploadDAL().GetYearUpload();
                    DAL.YearUploadSetup curryear = GetData.Where(x => x.BudgetYear == DateTime.Now.Year).FirstOrDefault();

                    if (curryear.ToString().Count() > 0)
                    {
                        if (!GetData.Where(y => y.BudgetYear == curryear.BudgetYear).Select(z => z.Status.Contains("A")).FirstOrDefault())
                        {
                            btnFileUpload.HRef = "#";
                        }
                    }
                }

                Session["SegmentDetailsData"] = data;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CreateTreeData()
        {
            try
            {
                SelectedNodes = (List<int>)Session["SelectedNodes"];
                List<SegmentDetail> data = (List<SegmentDetail>)Session["SegmentDetailsData"];
                List<SegmentDetailTreeHelper> TreeData = new List<SegmentDetailTreeHelper>();

                if (data.Count > 0)
                {
                    TreeData = data.Where(x => x.ParentDetailID == 0).OrderBy(x => x.DetailCode).Select(x =>
                            new SegmentDetailTreeHelper()
                            {
                                SegmentDetailID = x.SegmentDetailID,
                                SegmentID = Convert.ToInt32(x.SegmentID),
                                DetailCode = x.DetailCode,
                                DetailDesc = x.DetailDesc,
                                ParentDetailID = Convert.ToInt32(x.ParentDetailID),
                                Status = x.Status,
                                Level = 0,
                                ChildCount = data.Where(y => y.ParentDetailID == x.SegmentDetailID).Count()
                            }).ToList();

                    if (SelectedNodes == null || SelectedNodes.Count == 0)
                    {
                        Session["SelectedNodes"] = new List<int>();
                        SelectedNodes = new List<int>();
                    }
                    else
                    {
                        for (int i = 0; i < TreeData.Count; i++)
                        {
                            if (SelectedNodes.Contains(TreeData[i].SegmentDetailID))
                            {
                                foreach (SegmentDetail sd in data.Where(x => x.ParentDetailID == TreeData[i].SegmentDetailID).OrderByDescending(x => x.DetailCode))
                                {
                                    SegmentDetailTreeHelper objSH = new SegmentDetailTreeHelper()
                                    {
                                        SegmentDetailID = sd.SegmentDetailID,
                                        SegmentID = Convert.ToInt32(sd.SegmentID),
                                        DetailCode = sd.DetailCode,
                                        DetailDesc = sd.DetailDesc,
                                        ParentDetailID = Convert.ToInt32(sd.ParentDetailID),
                                        Status = sd.Status,
                                        Level = TreeData[i].Level + 1,
                                        ChildCount = data.Where(y => y.ParentDetailID == sd.SegmentDetailID).Count()
                                    };
                                    TreeData.Insert(i + 1, objSH);
                                }
                            }
                        }
                    }
                }

                Session["SegmentDetailsTree"] = TreeData;
                BindGrid();
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
                gvSegmentDetails.DataSource = (List<SegmentDetailTreeHelper>)Session["SegmentDetailsTree"];
                gvSegmentDetails.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void ChangeSeletedNodeStyle(GridViewRow GridViewRow)
        {
            try
            {
                foreach (GridViewRow gvr in gvSegmentDetails.Rows)
                    gvr.Style["background-color"] = "";
                GridViewRow.Style["background-color"] = "skyblue";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void AssignSelectedNode(int selectedSegmentDetailID)
        {
            try
            {
                Session["SelectedSegmentDetail"] = ((List<SegmentDetail>)Session["SegmentDetailsData"]).Where(x => x.SegmentDetailID == selectedSegmentDetailID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void ClearPageData()
        {
            try
            {
                tbCode.Text = string.Empty;
                tbDesc.Text = string.Empty;
                ddlStatus.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvSegmentDetails.Rows)
                    gvr.Style["background-color"] = "";

                EditForm.Visible = false;
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
                SegmentDetail objSegmentDetail = new SegmentDetail();
                objSegmentDetail.SegmentID = ((Segment)Session["SelectedSegment"]).SegmentID;
                objSegmentDetail.DetailCode = tbCode.Text.Trim();
                objSegmentDetail.DetailDesc = tbDesc.Text.Trim();
                objSegmentDetail.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                if ((SegmentDetail)Session["SelectedSegmentDetail"] == null)
                    objSegmentDetail.ParentDetailID = 0;
                else
                    objSegmentDetail.ParentDetailID = ((SegmentDetail)Session["SelectedSegmentDetail"]).SegmentDetailID;

                if (((Helper.PageMode)Session["PageMode"]) == Helper.PageMode.New)
                {
                    if (new SegmentDetailsDAL().GetSegmentDetails(((Segment)Session["SelectedSegment"]).SegmentID)
                        .Where(x => x.DetailCode.ToUpper().Trim() == tbCode.Text.ToUpper().Trim()).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "SegmentDetail already exists");
                        return;
                    }

                    objSegmentDetail.CreatedBy = LoggedInUser.UserID;
                    objSegmentDetail.CreatedTimeStamp = DateTime.Now;
                    objSegmentDetail.ModifiedBy = LoggedInUser.UserID;
                    objSegmentDetail.ModifiedTimeStamp = DateTime.Now;

                    if (new SegmentDetailsDAL().InsertSegmentDetail(objSegmentDetail))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Segment Detail saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Segment Detail");
                }
                else if (((Helper.PageMode)Session["PageMode"]) == Helper.PageMode.Edit)
                {
                    objSegmentDetail.ModifiedBy = LoggedInUser.UserID;
                    objSegmentDetail.ModifiedTimeStamp = DateTime.Now;

                    objSegmentDetail.SegmentDetailID = ((SegmentDetail)Session["SelectedSegmentDetail"]).SegmentDetailID;
                    objSegmentDetail.ParentDetailID = ((SegmentDetail)Session["SelectedSegmentDetail"]).ParentDetailID;
                    
                    if (new SegmentDetailsDAL().UpdateSegmentDetail(objSegmentDetail))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Segment Detail updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Segment Detail");
                }

                ClearPageData();
                Session["SelectedSegmentDetail"] = null;
                GetData();
                CreateTreeData();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static bool ReloadField()
        {
            string frm = HttpContext.Current.Request.QueryString["f"];
            if (frm == "widgetclosed")
            {
                HttpContext.Current.Session["SelectedSegmentDetail"] = null;
            }

            return true;
        }

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    ClearPageData();
        //    Session["SelectedSegmentDetail"] = null;
        //}

        protected void gvSegmentDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    SegmentDetailTreeHelper rowItem = (SegmentDetailTreeHelper)e.Row.DataItem;
                    //((Label)e.Row.FindControl("lblIndent")).Width = Unit.Pixel(rowItem.Level * 30);
                    //((Label)e.Row.FindControl("lblDetailCode")).Text = rowItem.DetailCode;
                    int width = rowItem.Level * 30;

                    string strHTML = string.Empty;
                    if (rowItem.ChildCount > 0)
                    {
                        if (SelectedNodes.Contains(rowItem.SegmentDetailID))
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-minus-square pull-right\"></i></label> ";
                        else
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-plus-square pull-right\"></i></label> ";
                    }
                    else
                        strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i></i></label> ";

                    LinkButton btnExpand = ((LinkButton)e.Row.FindControl("btnExpand"));
                    btnExpand.Text = "<div>" + strHTML + rowItem.DetailCode + "</div>";

                    if (rowItem.ParentDetailID != 0)
                        ((LinkButton)e.Row.FindControl("lbAddItem")).Visible = false;
                    if (rowItem.ParentDetailID == 0)
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                    if (rowItem.SegmentDetailID == 0)
                    {
                        ((LinkButton)e.Row.FindControl("btnExpand")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbEdit")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbDelete")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbCut")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbPaste")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbAddChild")).Visible = false;
                    }

                    if (Session["SelectedSegmentDetail"] != null && ((SegmentDetail)Session["SelectedSegmentDetail"]).SegmentDetailID == rowItem.SegmentDetailID)
                    {
                        e.Row.Style["background-color"] = "skyblue";
                    }

                    var span = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));
                    if (rowItem.Status == "A")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                            "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                    }
                    else if (rowItem.Status == "D")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                            "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvSegmentDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                List<SegmentDetailTreeHelper> TreeData = (List<SegmentDetailTreeHelper>)Session["SegmentDetailsTree"];
                GridViewRow selectedRow = gvSegmentDetails.Rows[Convert.ToInt32(e.CommandArgument)];
                int SegmentDetailID = Convert.ToInt32(gvSegmentDetails.DataKeys[selectedRow.RowIndex]["SegmentDetailID"]);
                if (e.CommandName == "Expand")
                {
                    SelectedNodes = (List<int>)Session["SelectedNodes"];
                    if (!SelectedNodes.Contains(SegmentDetailID))
                    {
                        if (TreeData.Where(x => x.SegmentDetailID == SegmentDetailID).FirstOrDefault().ChildCount > 0)
                            SelectedNodes.Add(SegmentDetailID);
                    }
                    else
                    {
                        SelectedNodes.Remove(SegmentDetailID);
                    }
                    CreateTreeData();
                }
                else if (e.CommandName == "AddItem")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    ChangePageMode(Helper.PageMode.New);
                    EditForm.Visible = true;
                    tbCode.Enabled = true;
                }
                else if (e.CommandName == "AddChild")
                {
                    ClearPageData();
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(SegmentDetailID);
                    ChangePageMode(Helper.PageMode.New);
                    EditForm.Visible = true;
                    tbCode.Enabled = true;
                }
                else if (e.CommandName == "MakeRoot")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(SegmentDetailID);
                    MakeRoot();
                    ClearPageData();
                }
                else if (e.CommandName == "CmdEdit")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(SegmentDetailID);
                    CommandEdit();
                    ChangePageMode(Helper.PageMode.Edit);
                    EditForm.Visible = true;
                    tbCode.Enabled = false;
                }
                else if (e.CommandName == "CmdDelete")
                {
                    AssignSelectedNode(SegmentDetailID);
                    SegmentDetail objSegmentDetail = (SegmentDetail)Session["SelectedSegmentDetail"];
                    objSegmentDetail.Status = "D";
                    objSegmentDetail.ModifiedBy = LoggedInUser.UserID;
                    objSegmentDetail.ModifiedTimeStamp = DateTime.Now;

                    if (new SegmentDetailsDAL().UpdateSegmentDetail(objSegmentDetail))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Segment Detail updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Segment Detail");
                    ClearPageData();
                    Session["SelectedSegmentDetail"] = null;
                    GetData();
                    CreateTreeData();
                }
                else if (e.CommandName == "CmdCut")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(SegmentDetailID);
                }
                else if (e.CommandName == "CmdPaste")
                {
                    CommandPaste(SegmentDetailID);
                    GetData();
                    CreateTreeData();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CommandPaste(int ParentSegmentDetailID)
        {
            try
            {
                SegmentDetail cutSegmentDetail = (SegmentDetail)Session["SelectedSegmentDetail"];

                List<SegmentDetail> data = (List<SegmentDetail>)Session["SegmentDetailsData"];
                SegmentDetail parent = new SegmentDetail() { ParentDetailID = ParentSegmentDetailID };
                do
                {
                    parent = data.Where(x => x.SegmentDetailID == parent.ParentDetailID).FirstOrDefault();
                    if (parent == null || parent.SegmentDetailID == cutSegmentDetail.SegmentDetailID)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "You can not paste a parent under its child");
                        return;
                    }
                } while (parent.ParentDetailID != 0);

                cutSegmentDetail.ParentDetailID = ParentSegmentDetailID;
                new SegmentDetailsDAL().UpdateSegmentDetail(cutSegmentDetail);
                SelectedNodes = (List<int>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(ParentSegmentDetailID))
                {
                    if (((List<SegmentDetailTreeHelper>)Session["SegmentDetailsTree"]).Where(x => x.SegmentDetailID == ParentSegmentDetailID).FirstOrDefault().ChildCount > 0)
                        SelectedNodes.Add(ParentSegmentDetailID);
                }
                Session["SelectedNodes"] = SelectedNodes;
                GetData();
                CreateTreeData();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void MakeRoot()
        {
            try
            {
                SegmentDetail cutSegmentDetail = (SegmentDetail)Session["SelectedSegmentDetail"];
                cutSegmentDetail.ParentDetailID = 0;
                new SegmentDetailsDAL().UpdateSegmentDetail(cutSegmentDetail);
                SelectedNodes = (List<int>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(cutSegmentDetail.SegmentDetailID))
                {
                    if (((List<SegmentDetailTreeHelper>)Session["SegmentDetailsTree"]).Where(x => x.SegmentDetailID == cutSegmentDetail.SegmentDetailID).FirstOrDefault().ChildCount > 0)
                        SelectedNodes.Add(cutSegmentDetail.SegmentDetailID);
                }
                Session["SelectedNodes"] = SelectedNodes;
                GetData();
                CreateTreeData();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CommandEdit()
        {
            try
            {
                SegmentDetail objSegmentDetail = (SegmentDetail)Session["SelectedSegmentDetail"];
                tbCode.Text = objSegmentDetail.DetailCode;
                tbDesc.Text = objSegmentDetail.DetailDesc;
                ddlStatus.SelectedIndex = -1;
                if (objSegmentDetail.Status != null)
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objSegmentDetail.Status))).Selected = true;
                else
                    ddlStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void LoadDropDown()
        {
            ddlStatus.DataSource = Enum.GetValues(typeof(Helper.ItemStatus));
            ddlStatus.DataBind();
        }

        private void ChangePageMode(Helper.PageMode pagemode)
        {
            Segment obj = (Segment)Session["SelectedSegment"];
            string title_edit_new = "<a href=\"javascript: history.go(-1);\" class=\"red\" style=\"text-decoration:none\">[" + obj.SegmentName + "]</a>" + 
                " > " + "Segment Details - New";
            string title_edit_edit = "<a href=\"javascript: history.go(-1);\" class=\"red\" style=\"text-decoration:none\">[" + obj.SegmentName + "]</a>" +
                " > " + "Segment Details - Edit";
            string title_list = "<a href=\"javascript: history.go(-1);\" class=\"red\" title=\"**Format: "+ obj.ShapeFormat + " **\" style=\"text-decoration:none\">[" + obj.SegmentName + "]</a>" +
                " > " + "Segment Details - List";

            switch (pagemode)
            {
                case Helper.PageMode.New:
                    tbCode.ReadOnly = false;
                    widget_title_edit.InnerHtml = title_edit_new;
                    widget_title_list.InnerHtml = title_list;
                    break;
                case Helper.PageMode.Edit:
                    tbCode.ReadOnly = true;
                    widget_title_edit.InnerHtml = title_edit_edit;
                    widget_title_list.InnerHtml = title_list;
                    break;
            }
            Session["PageMode"] = pagemode;
        }

        protected void gvSegmentDetails_PreRender(object sender, EventArgs e)
        {
            if (gvSegmentDetails.Rows.Count > 0)
            {
                gvSegmentDetails.UseAccessibleHeader = true;
                gvSegmentDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvSegmentDetails.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}