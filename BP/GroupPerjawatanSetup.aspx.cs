using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BP.Classes;
using DAL;
using System.Web.UI.HtmlControls;
using System.Web.Services;

namespace BP
{
    public class GroupPerjawatanTreeHelper
    {
        public string GroupPerjawatanCode { get; set; }
        public string GroupPerjawatanDesc { get; set; }
        public string ParentGroupPerjawatanID { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public int ChildCount { get; set; }
    }

    public class ServiceCodeImport
    {
        public string ServiceCode { get; set; }
        public string ServiceDesc { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string UpperLevel { get; set; }
    }

    public partial class GroupPerjawatanSetup : PageHelper
    {
        List<string> SelectedNodes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["SelectedNodes"] = null;
                Session["SelectedGroupPerjawatan"] = null;

                GetData();
                CreateTreeData();
                LoadDropDown();
            }
        }

        private void GetData()
        {
            try
            {
                List<GroupPerjawatan> data = new GroupPerjawatanDAL().GetGroupPerjawatans().ToList();
                if (data.Count == 0)
                {
                    data = new List<GroupPerjawatan>();
                    data.Add(new GroupPerjawatan() { GroupPerjawatanCode = string.Empty, ParentGroupPerjawatanID = string.Empty });

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
                else
                {
                    btnFileUpload.HRef = "#";
                }

                Session["GroupPerjawatansData"] = data;
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
                SelectedNodes = (List<string>)Session["SelectedNodes"];
                List<GroupPerjawatan> data = (List<GroupPerjawatan>)Session["GroupPerjawatansData"];
                List<GroupPerjawatanTreeHelper> TreeData = new List<GroupPerjawatanTreeHelper>();
                if (data.Count > 0)
                {
                    TreeData = data.Where(x => x.ParentGroupPerjawatanID == string.Empty).OrderBy(x => x.GroupPerjawatanCode).Select(x =>
                            new GroupPerjawatanTreeHelper()
                            {
                                GroupPerjawatanCode = x.GroupPerjawatanCode,
                                GroupPerjawatanDesc = x.GroupPerjawatanDesc,
                                ParentGroupPerjawatanID = x.ParentGroupPerjawatanID,
                                Status = x.Status,
                                Level = 0,
                                ChildCount = data.Where(y => y.ParentGroupPerjawatanID == x.GroupPerjawatanCode).Count()
                            }).ToList();

                    if (SelectedNodes == null || SelectedNodes.Count == 0)
                    {
                        Session["SelectedNodes"] = new List<string>();
                        SelectedNodes = new List<string>();
                    }
                    else
                    {
                        //while(TreeData.Where(x=>x.IsExpanded).Select(x=>x).Count() < SelectedNodes.Count)
                        //{
                        for (int i = 0; i < TreeData.Count; i++)
                        {
                            if (SelectedNodes.Contains(TreeData[i].GroupPerjawatanCode))
                            {
                                //TreeData[i].IsExpanded = true;
                                foreach (GroupPerjawatan sd in data.Where(x => x.ParentGroupPerjawatanID == TreeData[i].GroupPerjawatanCode).OrderByDescending(x => x.GroupPerjawatanCode))
                                {
                                    GroupPerjawatanTreeHelper objSH = new GroupPerjawatanTreeHelper()
                                    {
                                        GroupPerjawatanCode = sd.GroupPerjawatanCode,
                                        GroupPerjawatanDesc = sd.GroupPerjawatanDesc,
                                        ParentGroupPerjawatanID = sd.ParentGroupPerjawatanID,
                                        Status = sd.Status,
                                        Level = TreeData[i].Level + 1,
                                        ChildCount = data.Where(y => y.ParentGroupPerjawatanID == sd.GroupPerjawatanCode).Count()
                                    };
                                    TreeData.Insert(i + 1, objSH);
                                }
                            }
                        }
                        //}
                    }
                }
                Session["GroupPerjawatansTree"] = TreeData;
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
                gvGroupPerjawatans.DataSource = (List<GroupPerjawatanTreeHelper>)Session["GroupPerjawatansTree"];
                gvGroupPerjawatans.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void ChangeSeletedNodeStyle(GridViewRow GridViewRow)
        {
            foreach (GridViewRow gvr in gvGroupPerjawatans.Rows)
                gvr.Style["background-color"] = "";
            GridViewRow.Style["background-color"] = "skyblue";
        }

        private void AssignSelectedNode(string selectedGroupPerjawatanID)
        {
            try
            {
                Session["SelectedGroupPerjawatan"] = ((List<GroupPerjawatan>)Session["GroupPerjawatansData"]).Where(x => x.GroupPerjawatanCode == selectedGroupPerjawatanID).FirstOrDefault();
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
                tbServiceGroup.Text = string.Empty;
                tbServiceDesc.Text = string.Empty;
                ddlServiceStatus.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvGroupPerjawatans.Rows)
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
                GroupPerjawatan objGroupPerjawatan = new GroupPerjawatan();
                objGroupPerjawatan.GroupPerjawatanCode = tbServiceGroup.Text.Trim();
                objGroupPerjawatan.GroupPerjawatanDesc = tbServiceDesc.Text.Trim();
                objGroupPerjawatan.Status = new Helper().GetItemStatusEnumValueByName(ddlServiceStatus.SelectedValue);
                if ((GroupPerjawatan)Session["SelectedGroupPerjawatan"] == null)
                    objGroupPerjawatan.ParentGroupPerjawatanID = string.Empty;
                else
                    objGroupPerjawatan.ParentGroupPerjawatanID = ((GroupPerjawatan)Session["SelectedGroupPerjawatan"]).GroupPerjawatanCode;

                if (((Helper.PageMode)Session["PageMode"]) == Helper.PageMode.New)
                {
                    if (new GroupPerjawatanDAL().GetGroupPerjawatans().Where(x => x.GroupPerjawatanCode.ToUpper().Trim() == tbServiceGroup.Text.ToUpper().Trim()).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "ServiceCode already exists");
                        return;
                    }
                    objGroupPerjawatan.CreatedBy = LoggedInUser.UserID;
                    objGroupPerjawatan.CreatedTimeStamp = DateTime.Now;
                    objGroupPerjawatan.ModifiedBy = LoggedInUser.UserID;
                    objGroupPerjawatan.ModifiedTimeStamp = DateTime.Now;

                    if (new GroupPerjawatanDAL().InsertGroupPerjawatan(objGroupPerjawatan))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Service Group saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Service Group");
                }
                else
                {
                    objGroupPerjawatan.ModifiedBy = LoggedInUser.UserID;
                    objGroupPerjawatan.ModifiedTimeStamp = DateTime.Now;

                    objGroupPerjawatan.GroupPerjawatanCode = ((GroupPerjawatan)Session["SelectedGroupPerjawatan"]).GroupPerjawatanCode;
                    objGroupPerjawatan.ParentGroupPerjawatanID = ((GroupPerjawatan)Session["SelectedGroupPerjawatan"]).ParentGroupPerjawatanID;
                    if (new GroupPerjawatanDAL().UpdateGroupPerjawatan(objGroupPerjawatan))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Service Group updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Service Group");
                }

                ClearPageData();
                Session["SelectedGroupPerjawatan"] = null;
                GetData();
                CreateTreeData();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        [WebMethod(EnableSession = true)]
        public static bool ReloadField()
        {
            string frm = HttpContext.Current.Request.QueryString["f"];
            if (frm == "widgetclosed")
            {
                HttpContext.Current.Session["SelectedGroupPerjawatan"] = null;
            }

            return true;
        }

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    ClearPageData();
        //    Session["SelectedGroupPerjawatan"] = null;
        //}

        protected void gvGroupPerjawatans_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GroupPerjawatanTreeHelper rowItem = (GroupPerjawatanTreeHelper)e.Row.DataItem;
                    //((Label)e.Row.FindControl("lblIndent")).Width = Unit.Pixel(rowItem.Level * 30);
                    //((Label)e.Row.FindControl("lblDetailCode")).Text = rowItem.DetailCode;
                    int width = rowItem.Level * 30;

                    string strHTML = string.Empty;
                    if (rowItem.ChildCount > 0)
                    {
                        if (SelectedNodes.Contains(rowItem.GroupPerjawatanCode))
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-minus-square pull-right\"></i></label> ";
                        else
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-plus-square pull-right\"></i></label> ";
                    }
                    else
                        strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i></i></label> ";

                    LinkButton btnExpand = ((LinkButton)e.Row.FindControl("btnExpand"));
                    btnExpand.Text = "<div>" + strHTML + rowItem.GroupPerjawatanCode + "</div>";

                    if (rowItem.ParentGroupPerjawatanID != string.Empty)
                        ((LinkButton)e.Row.FindControl("lbAddItem")).Visible = false;
                    if (rowItem.ParentGroupPerjawatanID == string.Empty)
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                    if (rowItem.GroupPerjawatanCode == string.Empty)
                    {
                        ((LinkButton)e.Row.FindControl("btnExpand")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbEit")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbDelete")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbCut")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbPaste")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbAddChild")).Visible = false;
                    }

                    if (Session["SelectedGroupPerjawatan"] != null && ((GroupPerjawatan)Session["SelectedGroupPerjawatan"]).GroupPerjawatanCode == rowItem.GroupPerjawatanCode)
                    {
                        e.Row.Style["background-color"] = "skyblue";
                    }

                    var span = ((HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));
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

        protected void gvGroupPerjawatans_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                List<GroupPerjawatanTreeHelper> TreeData = (List<GroupPerjawatanTreeHelper>)Session["GroupPerjawatansTree"];
                GridViewRow selectedRow = gvGroupPerjawatans.Rows[Convert.ToInt32(e.CommandArgument)];
                string GroupPerjawatan = gvGroupPerjawatans.DataKeys[selectedRow.RowIndex]["GroupPerjawatanCode"].ToString();
                if (e.CommandName == "Expand")
                {
                    SelectedNodes = (List<string>)Session["SelectedNodes"];
                    if (!SelectedNodes.Contains(GroupPerjawatan))
                    {
                        if (TreeData.Where(x => x.GroupPerjawatanCode == GroupPerjawatan).FirstOrDefault().ChildCount > 0)
                            SelectedNodes.Add(GroupPerjawatan);
                    }
                    else
                    {
                        SelectedNodes.Remove(GroupPerjawatan);
                    }
                    CreateTreeData();
                }
                else if (e.CommandName == "AddItem")
                {
                    Session["PageMode"] = Helper.PageMode.New;
                    EditForm.Visible = true;
                    tbServiceGroup.Enabled = true;
                }
                else if (e.CommandName == "AddChild")
                {
                    ClearPageData();
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(GroupPerjawatan);
                    Session["PageMode"] = Helper.PageMode.New;
                    EditForm.Visible = true;
                    tbServiceGroup.Enabled = true;
                }
                else if (e.CommandName == "MakeRoot")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(GroupPerjawatan);
                    MakeRoot();
                    ClearPageData();
                }
                else if (e.CommandName == "CmdEdit")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(GroupPerjawatan);
                    CommandEdit();
                    Session["PageMode"] = Helper.PageMode.Edit;
                    EditForm.Visible = true;
                    tbServiceGroup.Enabled = false;
                }
                else if (e.CommandName == "CmdDelete")
                {
                    AssignSelectedNode(GroupPerjawatan);
                    GroupPerjawatan objGroupPerjawatan = (GroupPerjawatan)Session["SelectedGroupPerjawatan"];
                    objGroupPerjawatan.Status = "D";
                    objGroupPerjawatan.ModifiedBy = LoggedInUser.UserID;
                    objGroupPerjawatan.ModifiedTimeStamp = DateTime.Now;
                    if (new GroupPerjawatanDAL().UpdateGroupPerjawatan(objGroupPerjawatan))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Service Group updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Service Group");
                    ClearPageData();
                    Session["SelectedGroupPerjawatan"] = null;
                    GetData();
                    CreateTreeData();
                }
                else if (e.CommandName == "CmdCut")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(GroupPerjawatan);
                }
                else if (e.CommandName == "CmdPaste")
                {
                    CommandPaste(GroupPerjawatan);
                    GetData();
                    CreateTreeData();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CommandPaste(string ParentGroupPerjawatanID)
        {
            try
            {
                GroupPerjawatan cutGroupPerjawatan = (GroupPerjawatan)Session["SelectedGroupPerjawatan"];

                List<GroupPerjawatan> data = (List<GroupPerjawatan>)Session["GroupPerjawatansData"];
                GroupPerjawatan parent = new GroupPerjawatan() { ParentGroupPerjawatanID = ParentGroupPerjawatanID };
                do
                {
                    parent = data.Where(x => x.GroupPerjawatanCode == parent.ParentGroupPerjawatanID).FirstOrDefault();
                    if (parent == null || parent.GroupPerjawatanCode == cutGroupPerjawatan.GroupPerjawatanCode)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "You can not paste a parent under its child");
                        return;
                    }
                } while (parent.ParentGroupPerjawatanID != string.Empty);

                cutGroupPerjawatan.ParentGroupPerjawatanID = ParentGroupPerjawatanID;
                new GroupPerjawatanDAL().UpdateGroupPerjawatan(cutGroupPerjawatan);
                SelectedNodes = (List<string>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(ParentGroupPerjawatanID))
                {
                    if (((List<GroupPerjawatanTreeHelper>)Session["GroupPerjawatansTree"]).Where(x => x.GroupPerjawatanCode == ParentGroupPerjawatanID).FirstOrDefault().ChildCount > 0)
                        SelectedNodes.Add(ParentGroupPerjawatanID);
                }
                Session["SelectedNodes"] = SelectedNodes;
                //GetData();
                //CreateTreeData();
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
                GroupPerjawatan cutGroupPerjawatan = (GroupPerjawatan)Session["SelectedGroupPerjawatan"];
                cutGroupPerjawatan.ParentGroupPerjawatanID = string.Empty;
                new GroupPerjawatanDAL().UpdateGroupPerjawatan(cutGroupPerjawatan);
                SelectedNodes = (List<string>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(cutGroupPerjawatan.GroupPerjawatanCode))
                {
                    if (((List<GroupPerjawatanTreeHelper>)Session["GroupPerjawatansTree"]).Where(x => x.GroupPerjawatanCode == cutGroupPerjawatan.GroupPerjawatanCode).FirstOrDefault().ChildCount > 0)
                        SelectedNodes.Add(cutGroupPerjawatan.GroupPerjawatanCode);
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
                GroupPerjawatan objGroupPerjawatan = (GroupPerjawatan)Session["SelectedGroupPerjawatan"];
                tbServiceGroup.Text = objGroupPerjawatan.GroupPerjawatanCode;
                tbServiceDesc.Text = objGroupPerjawatan.GroupPerjawatanDesc;
                ddlServiceStatus.SelectedIndex = -1;
                ddlServiceStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objGroupPerjawatan.Status))).Selected = true;
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
                ddlServiceStatus.DataSource = Enum.GetValues(typeof(Helper.ItemStatus));
                ddlServiceStatus.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvGroupPerjawatans_PreRender(object sender, EventArgs e)
        {
            if (gvGroupPerjawatans.Rows.Count > 0)
            {
                gvGroupPerjawatans.UseAccessibleHeader = true;
                gvGroupPerjawatans.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvGroupPerjawatans.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

    }
}