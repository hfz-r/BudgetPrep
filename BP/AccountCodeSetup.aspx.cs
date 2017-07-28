﻿using System;
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
    public class AccountCodeTreeHelper
    {
        public string AccountCode { get; set; }
        public string AccountDesc { get; set; }
        public string Keterangan { get; set; }
        public string Pengiraan { get; set; }
        public string ParentAccountCode { get; set; }
        public string Status { get; set; }
        public int Level { get; set; }
        public int ChildCount { get; set; }
    }

    public class AccountCodeImport
    {
        public string AccountCode { get; set; }
        public string AccountDesc { get; set; }
        public string Status { get; set; }
        public string Action { get; set; }
        public string UpperLevel { get; set; }
    }

    public partial class AccountCodeSetup : PageHelper
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
                List<AccountCode> data = new AccountCodeDAL().GetAccountCodes().ToList();
                if (data.Count == 0)
                {
                    data = new List<AccountCode>();
                    data.Add(new AccountCode() { AccountCode1 = string.Empty, ParentAccountCode = string.Empty });

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

                Session["AccountCodesData"] = data;
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
                List<AccountCode> data = (List<AccountCode>)Session["AccountCodesData"];
                List<AccountCodeTreeHelper> TreeData = new List<AccountCodeTreeHelper>();
                if (data.Count > 0)
                {
                    TreeData = data.Where(x => x.ParentAccountCode == string.Empty).OrderBy(x => x.AccountCode1).Select(x =>
                            new AccountCodeTreeHelper()
                            {
                                AccountCode = x.AccountCode1,
                                AccountDesc = x.AccountDesc,
                                //Keterangan = x.Keterangan,
                                //Pengiraan = x.Pengiraan,
                                ParentAccountCode = x.ParentAccountCode,
                                Status = x.Status,
                                Level = 0,
                                ChildCount = data.Where(y => y.ParentAccountCode == x.AccountCode1).Count()
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
                            if (SelectedNodes.Contains(TreeData[i].AccountCode))
                            {
                                //TreeData[i].IsExpanded = true;
                                foreach (AccountCode sd in data.Where(x => x.ParentAccountCode == TreeData[i].AccountCode).OrderByDescending(x => x.AccountCode1))
                                {
                                    AccountCodeTreeHelper objSH = new AccountCodeTreeHelper()
                                    {
                                        AccountCode = sd.AccountCode1,
                                        AccountDesc = sd.AccountDesc,
                                        //Keterangan = sd.Keterangan,
                                        //Pengiraan = sd.Pengiraan,
                                        ParentAccountCode = sd.ParentAccountCode,
                                        Status = sd.Status,
                                        Level = TreeData[i].Level + 1,
                                        ChildCount = data.Where(y => y.ParentAccountCode == sd.AccountCode1).Count()
                                    };
                                    TreeData.Insert(i + 1, objSH);
                                }
                            }
                        }
                        //}
                    }
                }
                Session["AccountCodesTree"] = TreeData;
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
                gvAccountCodes.DataSource = (List<AccountCodeTreeHelper>)Session["AccountCodesTree"];
                gvAccountCodes.DataBind();
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
                foreach (GridViewRow gvr in gvAccountCodes.Rows)
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
                Session["SelectedAccountCode"] = ((List<AccountCode>)Session["AccountCodesData"]).Where(x => x.AccountCode1 == selectedAccountCodeID).FirstOrDefault();
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

            foreach (GridViewRow gvr in gvAccountCodes.Rows)
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

        protected void gvAccountCodes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    AccountCodeTreeHelper rowItem = (AccountCodeTreeHelper)e.Row.DataItem;
                    //((Label)e.Row.FindControl("lblIndent")).Width = Unit.Pixel(rowItem.Level * 30);
                    //((Label)e.Row.FindControl("lblDetailCode")).Text = rowItem.DetailCode;
                    int width = rowItem.Level * 30;

                    string strHTML = string.Empty;

                    if (rowItem.ChildCount > 0)
                    {
                        if (SelectedNodes.Contains(rowItem.AccountCode))
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-minus-square pull-right\"></i></label> ";
                        else
                            strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-plus-square pull-right\"></i></label> ";
                    }
                    else
                        strHTML = "<label style=\"width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i></i></label> ";

                    LinkButton btnExpand = ((LinkButton)e.Row.FindControl("btnExpand"));
                    btnExpand.Text = "<div>" + strHTML + rowItem.AccountCode + "</div>";

                    if (rowItem.ParentAccountCode != string.Empty)
                        ((LinkButton)e.Row.FindControl("lbAddItem")).Visible = false;
                    if (rowItem.ParentAccountCode == string.Empty)
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                    if (rowItem.AccountCode == string.Empty)
                    {
                        ((LinkButton)e.Row.FindControl("btnExpand")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbEit")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbDelete")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbCut")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbPaste")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbMakeRoot")).Visible = false;
                        ((LinkButton)e.Row.FindControl("lbAddChild")).Visible = false;
                    }

                    if (Session["SelectedAccountCode"] != null && ((AccountCode)Session["SelectedAccountCode"]).AccountCode1 == rowItem.AccountCode)
                    {
                        e.Row.Style["background-color"] = "skyblue";
                    }

                    var span = ((HtmlGenericControl)e.Row.Cells[2].FindControl("CustomStatus"));
                    if (rowItem.Status == "A")
                    {
                        //span.Attributes["class"] = "label label-success";
                        //span.InnerHtml = "<i class=\"fa fa-flag green bigger-150 tooltip-success\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Active\"></i>";
                        span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                            "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                    }
                    else if (rowItem.Status == "D")
                    {
                        //span.InnerHtml = "<i class=\"fa fa-flag red bigger-150 tooltip-error\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive\"></i>";
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

        protected void gvAccountCodes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                List<AccountCodeTreeHelper> TreeData = (List<AccountCodeTreeHelper>)Session["AccountCodesTree"];
                GridViewRow selectedRow = gvAccountCodes.Rows[Convert.ToInt32(e.CommandArgument)];
                string AccountCode = gvAccountCodes.DataKeys[selectedRow.RowIndex]["AccountCode"].ToString();
                if (e.CommandName == "Expand")
                {
                    SelectedNodes = (List<string>)Session["SelectedNodes"];
                    if (!SelectedNodes.Contains(AccountCode))
                    {
                        if (TreeData.Where(x => x.AccountCode == AccountCode).FirstOrDefault().ChildCount > 0)
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

                List<AccountCode> data = (List<AccountCode>)Session["AccountCodesData"];
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
                    if (((List<AccountCodeTreeHelper>)Session["AccountCodesTree"]).Where(x => x.AccountCode == ParentAccountCodeID).FirstOrDefault().ChildCount > 0)
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
                    if (((List<AccountCodeTreeHelper>)Session["AccountCodesTree"]).Where(x => x.AccountCode == cutAccountCode.AccountCode1).FirstOrDefault().ChildCount > 0)
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

        protected void gvAccountCodes_PreRender(object sender, EventArgs e)
        {
            if (gvAccountCodes.Rows.Count > 0)
            {
                gvAccountCodes.UseAccessibleHeader = true;
                gvAccountCodes.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvAccountCodes.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
    }
}