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
    public class PeruntukanAsalTreeHelper
    {
        public string BudgetAccount { get; set; }
        public string Description { get; set; }
        public string BudgetType { get; set; }
        public double BudgetAccKey { get; set; }
        public double BudgetLedgerKey { get; set; }
        public double BudgetYear { get; set; }
        public string Type { get; set; }
        public double BudgetAmount { get; set; }
    }

    public class PeruntukanAsalImport
    {
        public string BudgetAccount { get; set; }
        public string Description { get; set; }
        public string BudgetType { get; set; }
        public double BudgetAccKey { get; set; }
        public double BudgetLedgerKey { get; set; }
        public double BudgetYear { get; set; }
        public string Type { get; set; }
        public double BudgetAmount { get; set; }

    }

    public partial class PeruntukanAsalSetup : PageHelper
    {
        List<string> SelectedNodes;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["SelectedNodes"] = null;
                Session["SelectedAccountCode"] = null;

                GetData();
                CreateTreeData();
                LoadDropDown();
            }
        }

        private void GetData()
        {
            try
            {
                List<PeruntukanAsal> data = new PeruntukanAsalDAL().GetAccountCodes().ToList();
                if (data.Count == 0)
                {
                    data = new List<PeruntukanAsal>();
                    data.Add(new PeruntukanAsal() { BudgetAccount = string.Empty});

                    //List<DAL.PeruntukanAsal> GetData = new PeruntukanAsalDAL().GetAccountCodes();
                    //DAL.YearUploadSetup curryear = GetData.Where(x => x.BudgetYear == DateTime.Now.Year).FirstOrDefault();

                    //if (curryear.ToString().Count() > 0)
                    //{
                    //    if (!GetData.Where(y => y.BudgetYear == curryear.BudgetYear).Select(z => z.Status.Contains("A")).FirstOrDefault())
                    //    {
                    //        btnFileUpload.HRef = "#";
                    //    }
                    //}
                }

                Session["PeruntukanData"] = data;
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
                List<PeruntukanAsal> data = (List<PeruntukanAsal>)Session["PeruntukanData"];
                List<PeruntukanAsalTreeHelper> TreeData = new List<PeruntukanAsalTreeHelper>();
                if (data.Count > 0)
                {
                    TreeData = data.OrderBy(x => x.BudgetAccount).Select(x =>
                            new PeruntukanAsalTreeHelper()
                            {
                                BudgetAccount = x.BudgetAccount,
                                Description = x.Description,
                                BudgetAccKey = Convert.ToDouble(x.BudgetAccKey),
                                BudgetLedgerKey = Convert.ToDouble(x.BudgetLedgerKey),
                                BudgetType = x.BudgetType,
                                BudgetAmount = Convert.ToDouble(x.BudgetAmount),
                                Type = x.Type,
                                BudgetYear = Convert.ToDouble(x.BudgetYear),
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
                            if (SelectedNodes.Contains(TreeData[i].BudgetAccount))
                            {
                                //TreeData[i].IsExpanded = true;
                                foreach (PeruntukanAsal sd in data.Where(x => x.BudgetAccount == TreeData[i].BudgetAccount).OrderByDescending(x => x.BudgetAccount))
                                {
                                    PeruntukanAsalTreeHelper objSH = new PeruntukanAsalTreeHelper()
                                    {
                                        BudgetAccount = sd.BudgetAccount,
                                        Description = sd.Description,
                                        BudgetAccKey = Convert.ToDouble(sd.BudgetAccKey),
                                        BudgetLedgerKey = Convert.ToDouble(sd.BudgetLedgerKey),
                                        BudgetType = sd.BudgetType,
                                        Type = sd.Type,
                                        BudgetAmount = Convert.ToDouble(sd.BudgetAmount),
                                        BudgetYear = Convert.ToDouble(sd.BudgetYear),
                                    };
                                    TreeData.Insert(i + 1, objSH);
                                }
                            }
                        }
                        //}
                    }
                }
                Session["PeruntukanDataTree"] = TreeData;
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
                gvOpenbudget.DataSource = (List<PeruntukanAsalTreeHelper>)Session["PeruntukanDataTree"];
                gvOpenbudget.DataBind();
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
                foreach (GridViewRow gvr in gvOpenbudget.Rows)
                    gvr.Style["background-color"] = "";
                GridViewRow.Style["background-color"] = "skyblue";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void AssignSelectedNode(string selectedAccountCodeID)
        {
            try
            {
                Session["SelectedAccountCode"] = ((List<AccountCode>)Session["PeruntukanData"]).Where(x => x.AccountCode1 == selectedAccountCodeID).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void ClearPageData()
        {
            tbCode.Text = string.Empty;
            tbDesc.Text = string.Empty;
            //tbKeterangan.Text = string.Empty;
            //tbPengiraan.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;

            foreach (GridViewRow gvr in gvOpenbudget.Rows)
                gvr.Style["background-color"] = "";

            EditForm.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                AccountCode objAccountCode = new AccountCode();
                objAccountCode.AccountCode1 = tbCode.Text.Trim();
                objAccountCode.AccountDesc = tbDesc.Text.Trim();
                //objAccountCode.Keterangan = tbKeterangan.Text.Trim();
                //objAccountCode.Pengiraan = tbPengiraan.Text.Trim();
                objAccountCode.Status = new Helper().GetItemStatusEnumValueByName(ddlStatus.SelectedValue);
                if ((AccountCode)Session["SelectedAccountCode"] == null)
                    objAccountCode.ParentAccountCode = string.Empty;
                else
                    objAccountCode.ParentAccountCode = ((AccountCode)Session["SelectedAccountCode"]).AccountCode1;

                if (((Helper.PageMode)Session["PageMode"]) == Helper.PageMode.New)
                {
                    if (new AccountCodeDAL().GetAccountCodes().Where(x => x.AccountCode1.ToUpper().Trim() == tbCode.Text.ToUpper().Trim()).Count() > 0)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "AccountCode already exists");
                        return;
                    }
                    objAccountCode.CreatedBy = LoggedInUser.UserID;
                    objAccountCode.CreatedTimeStamp = DateTime.Now;
                    objAccountCode.ModifiedBy = LoggedInUser.UserID;
                    objAccountCode.ModifiedTimeStamp = DateTime.Now;

                    if (new AccountCodeDAL().InsertAccountCode(objAccountCode))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Account Code saved successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving Account Code");
                }
                else
                {
                    objAccountCode.ModifiedBy = LoggedInUser.UserID;
                    objAccountCode.ModifiedTimeStamp = DateTime.Now;

                    objAccountCode.AccountCode1 = ((AccountCode)Session["SelectedAccountCode"]).AccountCode1;
                    objAccountCode.ParentAccountCode = ((AccountCode)Session["SelectedAccountCode"]).ParentAccountCode;
                    if (new AccountCodeDAL().UpdateAccountCode(objAccountCode))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Account Code updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Account Code");
                }

                ClearPageData();
                Session["SelectedAccountCode"] = null;
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
                HttpContext.Current.Session["SelectedAccountCode"] = null;
            }

            return true;
        }

        //protected void btnCancel_Click(object sender, EventArgs e)
        //{
        //    ClearPageData();
        //    Session["SelectedAccountCode"] = null;
        //}

        protected void gvOpenbudget_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    PeruntukanAsalTreeHelper rowItem = (PeruntukanAsalTreeHelper)e.Row.DataItem;
                    //((Label)e.Row.FindControl("lblIndent")).Width = Unit.Pixel(rowItem.Level * 30);
                    //((Label)e.Row.FindControl("lblDetailCode")).Text = rowItem.DetailCode;
                    //int width = rowItem.BudgetAccKey * 30;

                    string strHTML = string.Empty;

                    //if (rowItem.BudgetAccKey > 0)
                    //{
                    //    if (SelectedNodes.Contains(rowItem.BudgetAccount))
                    //        strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-minus-square pull-right\"></i></label> ";
                    //    else
                    //        strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-plus-square pull-right\"></i></label> ";
                    //}
                    //else
                    //    strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i></i></label> ";

                    LinkButton btnExpand = ((LinkButton)e.Row.FindControl("btnExpand"));
                    btnExpand.Text = "<div>" + strHTML + rowItem.BudgetAccount + "</div>";

                    if (rowItem.BudgetAccount != string.Empty)
                        ((LinkButton)e.Row.FindControl("lbAddItem")).Visible = false;
                    if (rowItem.BudgetAccount == string.Empty)
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                    if (rowItem.BudgetAccount == string.Empty)
                    {
                        ((LinkButton)e.Row.FindControl("btnExpand")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbEit")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbDelete")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbCut")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbPaste")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbAddChild")).Visible = false;
                    }

                    if (Session["SelectedAccountCode"] != null && ((AccountCode)Session["SelectedAccountCode"]).AccountCode1 == rowItem.BudgetAccount)
                    {
                        e.Row.Style["background-color"] = "skyblue";
                    }

                    var span = ((HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));
                    //if (rowItem.Status == "A")
                    //{
                    //    //span.Attributes["class"] = "label label-success";
                    //    //span.InnerHtml = "<i class=\"fa fa-flag green bigger-150 tooltip-success\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Active\"></i>";
                    //    span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                    //        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                    //}
                    //else if (rowItem.Status == "D")
                    //{
                    //    //span.InnerHtml = "<i class=\"fa fa-flag red bigger-150 tooltip-error\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive\"></i>";
                    //    span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                    //        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                    //}
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvOpenbudget_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                List<PeruntukanAsalTreeHelper> TreeData = (List<PeruntukanAsalTreeHelper>)Session["PeruntukanDataTree"];
                GridViewRow selectedRow = gvOpenbudget.Rows[Convert.ToInt32(e.CommandArgument)];
                string AccountCode = gvOpenbudget.DataKeys[selectedRow.RowIndex]["BudgetAccount"].ToString();
                if (e.CommandName == "Expand")
                {
                    SelectedNodes = (List<string>)Session["SelectedNodes"];
                    if (!SelectedNodes.Contains(AccountCode))
                    {
                        if (TreeData.Where(x => x.BudgetAccount == AccountCode).FirstOrDefault().BudgetAccKey > 0)
                            SelectedNodes.Add(AccountCode);
                    }
                    else
                    {
                        SelectedNodes.Remove(AccountCode);
                    }
                    CreateTreeData();
                }
                else if (e.CommandName == "AddItem")
                {
                    //ChangeSeletedNodeStyle(selectedRow);
                    Session["PageMode"] = Helper.PageMode.New;
                    tbCode.Enabled = true;
                    EditForm.Visible = true;
                }
                else if (e.CommandName == "AddChild")
                {
                    ClearPageData();
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(AccountCode);
                    Session["PageMode"] = Helper.PageMode.New;
                    tbCode.Enabled = true;
                    EditForm.Visible = true;
                }
                else if (e.CommandName == "MakeRoot")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(AccountCode);
                    MakeRoot();
                    ClearPageData();
                    Session["SelectedAccountCode"] = null;
                }
                else if (e.CommandName == "CmdEdit")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(AccountCode);
                    CommandEdit();
                    Session["PageMode"] = Helper.PageMode.Edit;
                    EditForm.Visible = true;
                    tbCode.Enabled = false;
                }
                else if (e.CommandName == "CmdDelete")
                {
                    AssignSelectedNode(AccountCode);
                    AccountCode objAccountCode = (AccountCode)Session["SelectedAccountCode"];
                    objAccountCode.Status = "D";
                    objAccountCode.ModifiedBy = LoggedInUser.UserID;
                    objAccountCode.ModifiedTimeStamp = DateTime.Now;
                    if (new AccountCodeDAL().UpdateAccountCode(objAccountCode))
                        ((SiteMaster)this.Master).ShowMessage("Success", "Account Code updated successfully");
                    else
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while updating Account Code");
                    ClearPageData();
                    Session["SelectedAccountCode"] = null;
                    GetData();
                    CreateTreeData();
                    //Session["PageMode"] = Helper.PageMode.Edit;
                }
                else if (e.CommandName == "CmdCut")
                {
                    ChangeSeletedNodeStyle(selectedRow);
                    AssignSelectedNode(AccountCode);
                }
                else if (e.CommandName == "CmdPaste")
                {
                    CommandPaste(AccountCode);
                    GetData();
                    CreateTreeData();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CommandPaste(string ParentAccountCodeID)
        {
            try
            {
                AccountCode cutAccountCode = (AccountCode)Session["SelectedAccountCode"];

                List<AccountCode> data = (List<AccountCode>)Session["PeruntukanData"];
                AccountCode parent = new AccountCode() { ParentAccountCode = ParentAccountCodeID };
                do
                {
                    parent = data.Where(x => x.AccountCode1 == parent.ParentAccountCode).FirstOrDefault();
                    if (parent == null || parent.AccountCode1 == cutAccountCode.AccountCode1)
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "You can not paste a parent under its child");
                        return;
                    }
                } while (parent.ParentAccountCode != string.Empty);

                cutAccountCode.ParentAccountCode = ParentAccountCodeID;
                new AccountCodeDAL().UpdateAccountCode(cutAccountCode);
                SelectedNodes = (List<string>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(ParentAccountCodeID))
                {
                    if (((List<PeruntukanAsalTreeHelper>)Session["PeruntukanDataTree"]).Where(x => x.BudgetAccount == ParentAccountCodeID).FirstOrDefault().BudgetAccKey > 0)
                        SelectedNodes.Add(ParentAccountCodeID);
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
                AccountCode cutAccountCode = (AccountCode)Session["SelectedAccountCode"];
                cutAccountCode.ParentAccountCode = string.Empty;
                new AccountCodeDAL().UpdateAccountCode(cutAccountCode);
                SelectedNodes = (List<string>)Session["SelectedNodes"];
                if (!SelectedNodes.Contains(cutAccountCode.AccountCode1))
                {
                    if (((List<PeruntukanAsalTreeHelper>)Session["PeruntukanDataTree"]).Where(x => x.BudgetAccount == cutAccountCode.AccountCode1).FirstOrDefault().BudgetAccKey > 0)
                        SelectedNodes.Add(cutAccountCode.AccountCode1);
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
                AccountCode objAccountCode = (AccountCode)Session["SelectedAccountCode"];
                tbCode.Text = objAccountCode.AccountCode1;
                tbDesc.Text = objAccountCode.AccountDesc;
                //tbKeterangan.Text = objAccountCode.Keterangan;
                //tbPengiraan.Text = objAccountCode.Pengiraan;
                ddlStatus.SelectedIndex = -1;
                ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objAccountCode.Status))).Selected = true;
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

        protected void gvOpenbudget_PreRender(object sender, EventArgs e)
        {
            if (gvOpenbudget.Rows.Count > 0)
            {
                gvOpenbudget.UseAccessibleHeader = true;
                gvOpenbudget.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvOpenbudget.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}