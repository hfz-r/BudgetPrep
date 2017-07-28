﻿using System;
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
    public partial class StatusMengurus : PageHelper
    {
        MasterUser AuthUser;

        bool IsPreparer;
        bool IsReviewer;
        bool IsApprover;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AuthUser = (MasterUser)Session["UserData"];

                if (ViewState["IsPreparer"] == null)
                    ViewState["IsPreparer"] = AuthUser.JuncUserRoles.Where(x => x.Status == "A" && x.RoleID == 1).Count() > 0;
                if (ViewState["IsReviewer"] == null)
                    ViewState["IsReviewer"] = AuthUser.JuncUserRoles.Where(x => x.Status == "A" && x.RoleID == 5).Count() > 0;
                if (ViewState["IsApprover"] == null)
                    ViewState["IsApprover"] = AuthUser.JuncUserRoles.Where(x => x.Status == "A" && x.RoleID == 2).Count() > 0;

                IsPreparer = Convert.ToBoolean(ViewState["IsPreparer"]);
                IsReviewer = Convert.ToBoolean(ViewState["IsReviewer"]);
                IsApprover = Convert.ToBoolean(ViewState["IsApprover"]);

                if (!Page.IsPostBack)
                {
                    Session["SelectedNodes"] = null;

                    LoadDropDown();
                    GetData();
                    CreateTreeData();
                }

                if (Request.Params["__EVENTTARGET"] != null && Request.Params["__EVENTTARGET"] == "ctl00$MainContent$btnCancel")
                {
                    ListForm.Visible = false;
                }
                else
                {
                    if (ListForm.Visible)
                    {
                        BuildGrid();
                        BindGrid();
                    }
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
                List<AccountCode> lstAccountCode = new AccountCodeDAL().GetAccountCodes().Where(x => x.Status == "A").ToList();
                List<AccountCode> AccountCodesData = new List<AccountCode>();

                if (AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 3 || AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 4)
                    AccountCodesData = lstAccountCode;
                else
                {
                    AccountCodesData = AuthUser.UserMengurusWorkflows.Where(x => x.Status == "A").Select(x => x.AccountCode1).ToList();
                    if (AccountCodesData.Count == 0)
                    {
                        lstAccountCode = new List<AccountCode>();
                    }
                    else
                    {
                        List<string> lstprntcodes = AccountCodesData.Select(x => x.ParentAccountCode).Distinct().ToList();
                        while (lstprntcodes.Count > 0)
                        {
                            List<AccountCode> lstprnts = lstAccountCode.Where(x => lstprntcodes.Contains(x.AccountCode1)).ToList();
                            foreach (AccountCode o in lstprnts)
                                if (AccountCodesData.Where(x => x.AccountCode1 == o.AccountCode1).Count() == 0)
                                    AccountCodesData.Add(o);
                            lstprntcodes = lstprnts.Select(x => x.ParentAccountCode).Distinct().ToList();
                        }
                    }
                }
                Session["AccountCodesData"] = lstAccountCode;

                List<int> lstperiod = GetSelectedPeriods();
                List<PeriodMenguru> PeriodData = new PeriodMengurusDAL().GetPeriodMengurus().Where(x => x.Status == "A" && lstperiod.Contains(x.PeriodMengurusID))
                    .OrderBy(x => x.MengurusYear).ThenBy(x => x.FieldMenguru.FieldMengurusDesc).ToList();

                List<PeriodMenguru> FixedData = ((List<FieldMenguru>)Session["FixedFieldMengurus"]).Where(x => lstperiod.Contains(x.FieldMengurusID))
                   .Select(x => new PeriodMenguru
                   {
                       PeriodMengurusID = lstperiod.Contains(x.FieldMengurusID) ? x.FieldMengurusID : 0,
                       MengurusYear = DateTime.Now.Year,
                       FieldMenguru = new FieldMenguru
                       {
                           FieldMengurusID = x.FieldMengurusID,
                           FieldMengurusDesc = x.FieldMengurusDesc,
                           Status = x.Status
                       }
                   }).OrderBy(x => x.MengurusYear).ThenBy(x => x.FieldMenguru.FieldMengurusDesc).ToList();

                FixedData.AddRange(PeriodData);
                Session["PeriodData"] = FixedData;

                bool CanEdit = false;

                List<BudgetMenguru> BudgetData = new BudgetMengurusDAL().GetBudgetMengurusStatus(GetSelectedSegmentDetails(), ref CanEdit)
                    .Select(x => new BudgetMenguru
                    {
                        BudgetMengurusID = x.BudgetMengurusID,
                        AccountCode = x.AccountCode,
                        PeriodMengurusID = x.PeriodMengurusID,
                        Status = x.Status,
                        Remarks = x.Remarks,
                        Amount = (AccountCodesData.Where(y => y.ParentAccountCode == x.AccountCode).Count() == 0) ? x.Amount : 0
                    }).ToList();

                Session["BudgetData"] = BudgetData;
                Session["CanEdit"] = CanEdit;

                if (!CanEdit)
                {
                    chkKeterangan.Checked = false;
                    chkPengiraan.Checked = false;
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private Dictionary<int, int> GetSelectedSegmentDetails()
        {
            Dictionary<int, int> DicSegDtls = new Dictionary<int, int>();
            try
            {
                foreach (GridViewRow gvr in gvSegmentDLLs.Rows)
                {
                    TreeView tv = (TreeView)gvr.Cells[0].FindControl("tvSegmentDDL");
                    TextBox tb = (TextBox)gvr.Cells[0].FindControl("tbSegmentDDL");

                    //DicSegDtls.Add(Convert.ToInt32(gvSegmentDLLs.DataKeys[gvr.RowIndex]["SegmentID"].ToString()),
                    //   (tv == null || tb.Text.Trim() == string.Empty) ? 0 : Convert.ToInt32(tv.SelectedValue));
                    if (!string.IsNullOrEmpty(tv.SelectedValue))
                    {
                        DicSegDtls.Add(Convert.ToInt32(gvSegmentDLLs.DataKeys[gvr.RowIndex]["SegmentID"].ToString()), Convert.ToInt32(tv.SelectedValue));
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }

            Session["SegDtlsDictionary"] = DicSegDtls;
            return DicSegDtls;
        }

        private void CreateTreeData()
        {
            try
            {
                List<string> SelectedNodes = (List<string>)Session["SelectedNodes"];
                List<AccountCode> AccountCodesData = (List<AccountCode>)Session["AccountCodesData"];
                List<AccountCodeTreeHelper> TreeData = new List<AccountCodeTreeHelper>();
                if (AccountCodesData.Count > 0)
                {
                    TreeData = AccountCodesData.Where(x => x.ParentAccountCode == string.Empty && x.Status == "A").OrderBy(x => x.AccountCode1).Select(x =>
                            new AccountCodeTreeHelper()
                            {
                                AccountCode = x.AccountCode1,
                                AccountDesc = x.AccountDesc,
                                Keterangan = x.Keterangan,
                                Pengiraan = x.Pengiraan,
                                ParentAccountCode = x.ParentAccountCode,
                                Status = x.Status,
                                Level = 0,
                                ChildCount = AccountCodesData.Where(y => y.ParentAccountCode == x.AccountCode1).Count()
                            }).ToList();

                    if (SelectedNodes == null || SelectedNodes.Count == 0)
                    {
                        Session["SelectedNodes"] = new List<string>();
                        SelectedNodes = new List<string>();
                    }
                    else
                    {
                        for (int i = 0; i < TreeData.Count; i++)
                        {
                            if (SelectedNodes.Contains(TreeData[i].AccountCode))
                            {
                                foreach (AccountCode sd in AccountCodesData.Where(x => x.ParentAccountCode == TreeData[i].AccountCode && x.Status == "A").OrderByDescending(x => x.AccountCode1))
                                {
                                    AccountCodeTreeHelper objSH = new AccountCodeTreeHelper()
                                    {
                                        AccountCode = sd.AccountCode1,
                                        AccountDesc = sd.AccountDesc,
                                        Keterangan = sd.Keterangan,
                                        Pengiraan = sd.Pengiraan,
                                        ParentAccountCode = sd.ParentAccountCode,
                                        Status = sd.Status,
                                        Level = TreeData[i].Level + 1,
                                        ChildCount = AccountCodesData.Where(y => y.ParentAccountCode == sd.AccountCode1).Count()
                                    };
                                    TreeData.Insert(i + 1, objSH);
                                }
                            }
                        }
                    }
                }
                Session["AccountCodesTree"] = TreeData;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void BuildGrid()
        {
            try
            {
                gvAccountCodes.Columns.Clear();
                List<PeriodMenguru> PeriodData = (List<PeriodMenguru>)Session["PeriodData"];
                List<BudgetMenguru> BudgetData = (List<BudgetMenguru>)Session["BudgetData"];
                List<AccountCode> AccountCodesData = (List<AccountCode>)Session["AccountCodesData"];
                List<string> parentcodes = AccountCodesData.Select(x => x.ParentAccountCode).Distinct().ToList();
                List<string> ChildCodes = AccountCodesData.Where(x => !parentcodes.Contains(x.AccountCode1)).Select(x => x.AccountCode1).ToList();

                TemplateField templateField1 = new TemplateField();
                templateField1.HeaderText = "Objek";
                templateField1.ItemTemplate = new GridViewCustomTemplate(0, "", 0);
                templateField1.FooterText = "Jumlah";
                gvAccountCodes.Columns.Add(templateField1);

                BoundField bf1 = new BoundField();
                bf1.HeaderText = "Description";
                bf1.DataField = "AccountDesc";
                gvAccountCodes.Columns.Add(bf1);

                if (chkKeterangan.Checked)
                {
                    BoundField bf2 = new BoundField();
                    bf2.HeaderText = "Keterangan";
                    bf2.DataField = "Keterangan";
                    gvAccountCodes.Columns.Add(bf2);
                }

                if (chkPengiraan.Checked)
                {
                    BoundField bf2 = new BoundField();
                    bf2.HeaderText = "Pengiraan";
                    bf2.DataField = "Pengiraan";
                    gvAccountCodes.Columns.Add(bf2);
                }

                foreach (PeriodMenguru pm in PeriodData)
                {
                    TemplateField templateField = new TemplateField();
                    templateField.HeaderText = pm.MengurusYear.ToString() + " " + pm.FieldMenguru.FieldMengurusDesc;
                    GridViewStatusCustomTemplate objTemp = new GridViewStatusCustomTemplate(0, pm.MengurusYear.ToString() + " " + pm.FieldMenguru.FieldMengurusDesc, pm.PeriodMengurusID);
                    objTemp.OnCustomStatusClicked += new CustomStatusClickedEventHandler(objTemp_OnCustomStatusClicked);
                    templateField.ItemTemplate = objTemp;
                    templateField.FooterText = BudgetData.Where(x => x.PeriodMengurusID == pm.PeriodMengurusID && ChildCodes.Contains(x.AccountCode)).Select(x => x.Amount).Sum().ToString("F");
                    gvAccountCodes.Columns.Add(templateField);
                }
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

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "loadDatatable", "LoadDataTable('" + gvAccountCodes.Columns.Count + "');", true);
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
                ListForm.Visible = false;

                foreach (GridViewRow gvr in gvSegmentDLLs.Rows)
                {
                    TreeView tv = (TreeView)gvr.Cells[0].FindControl("tvSegmentDDL");
                    TextBox tb = (TextBox)gvr.Cells[0].FindControl("tbSegmentDDL");
                    if (tb != null)
                    {
                        if (tv.SelectedNode != null)
                            tv.SelectedNode.Selected = false;
                        tb.Text = string.Empty;
                    }
                }

                chkMedan.Checked = false;

                for (int i = 0; i < gvPeriod.Rows.Count; i++)
                {
                    string stryear = gvPeriod.Rows[i].Cells[1].Text;
                    ((CheckBox)gvPeriod.Rows[i].Cells[0].FindControl("cbPeriodSelect")).Checked = (stryear == (DateTime.Now.Year + 1).ToString());

                    int PeriodMengurusID = Convert.ToInt32(gvPeriod.DataKeys[i]["PeriodMengurusID"].ToString());
                    List<FieldMenguru> obj = ((List<FieldMenguru>)Session["FixedFieldMengurus"]).Where(x => x.FieldMengurusID == PeriodMengurusID).ToList();

                    if (obj.Count() > 0)
                    {
                        ((CheckBox)gvPeriod.Rows[i].Cells[0].FindControl("cbPeriodSelect")).Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void btnSelectedRowClear(object sender, EventArgs e)
        {
            //Get the button that raised the event
            LinkButton btn = (LinkButton)sender;

            //Get the row that contains this button
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;

            TreeView tv = (TreeView)gvr.Cells[0].FindControl("tvSegmentDDL");
            TextBox tb = (TextBox)gvr.Cells[0].FindControl("tbSegmentDDL");
            if (tb != null)
            {
                if (tv.SelectedNode != null)
                    tv.SelectedNode.Selected = false;
                tb.Text = string.Empty;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ListForm.Visible = true;

            GetData();
            //PopulateSearchResult(); (TBA)
            CreateTreeData();
            BuildGrid();
            BindGrid();

            EditForm.Visible = false;
        }

        protected void btnSearchbox_Click(object sender, EventArgs e)
        {
            EditForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearPageData();
            EditForm.Visible = true;
        }

        protected void gvAccountCodes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    List<string> SelectedNodes = (List<string>)Session["SelectedNodes"];
                    List<BudgetMenguru> BudgetData = (List<BudgetMenguru>)Session["BudgetData"];
                    List<AccountCodeTreeHelper> TreeData = (List<AccountCodeTreeHelper>)Session["AccountCodesTree"];
                    List<AccountCode> AccountCodesData = (List<AccountCode>)Session["AccountCodesData"];
                    List<PeriodMenguru> PeriodData = (List<PeriodMenguru>)Session["PeriodData"];

                    AccountCodeTreeHelper rowItem = (AccountCodeTreeHelper)e.Row.DataItem;

                    /*Start Account Code logics*/
                    int width = rowItem.Level * (new Helper().IndentPixels);

                    string strHTML = string.Empty;
                    if (rowItem.ChildCount > 0)
                    {
                        if (SelectedNodes.Contains(rowItem.AccountCode))
                            strHTML = "<label style=\"display: inline-block;width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-minus-square pull-right\"></i></label> ";
                        else
                            strHTML = "<label style=\"display: inline-block;width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i class=\"ace-icon fa fa-plus-square pull-right\"></i></label> ";
                    }
                    else
                        strHTML = "<label style=\"display: inline-block;width:" + (width + 10).ToString() + "px;vertical-align:middle;\"><i></i></label> ";

                    LinkButton btnExpand = ((LinkButton)e.Row.Cells[0].FindControl("btnExpand"));
                    //btnExpand.Text = "<div style=\"max-width:200px;overflow:hidden;white-space:nowrap;\">" + strHTML + Session["PrefixAcountCode"].ToString() + rowItem.AccountCode + "</div>";
                    btnExpand.Text = "<div style=\"max-width:200px;overflow:hidden;white-space:nowrap;\">" + strHTML + rowItem.AccountCode + "</div>";
                    btnExpand.ToolTip = rowItem.AccountDesc;

                    //if (Session["SelectedAccountCode"] != null && ((AccountCode)Session["SelectedAccountCode"]).AccountCode1 == rowItem.AccountCode)
                    //{
                    //    e.Row.Style["background-color"] = "skyblue";
                    //}
                    //if (SelectedNodes.Contains(rowItem.AccountCode))
                    //{
                    //    e.Row.Style["background-color"] = "skyblue";
                    //}
                    /*End Account Code logics*/

                    /*Start Buget logics*/
                    int index = 2;
                    if (chkKeterangan.Checked)
                    {
                        index++;
                    }
                    if (chkPengiraan.Checked)
                    {
                        index++;
                    }

                    bool IsBudgetEditable = Convert.ToBoolean(Session["CanEdit"]);
                    for (int c = index; c < gvAccountCodes.Columns.Count; c++)
                    {
                        int PeriodMenguruID = Convert.ToInt32(((Label)e.Row.Cells[c].Controls[0]).Text);
                        PeriodMenguru pm = PeriodData.Where(x => x.PeriodMengurusID == PeriodMenguruID).FirstOrDefault();
                        BudgetMenguru ObjBudgetMenguru = BudgetData.Where(x => x.AccountCode == rowItem.AccountCode && x.PeriodMengurusID == PeriodMenguruID).FirstOrDefault();

                        Label lbl = ((Label)e.Row.Cells[c].FindControl("lbl_" + PeriodMenguruID));
                        LinkButton btnSaved = ((LinkButton)e.Row.Cells[c].FindControl("btnSaved_" + PeriodMenguruID));
                        LinkButton btnPrepared = ((LinkButton)e.Row.Cells[c].FindControl("btnPrepared_" + PeriodMenguruID));
                        LinkButton btnReviewed = ((LinkButton)e.Row.Cells[c].FindControl("btnReviewed_" + PeriodMenguruID));
                        LinkButton btnApproved = ((LinkButton)e.Row.Cells[c].FindControl("btnApproved_" + PeriodMenguruID));
                        LinkButton btnRevRej = ((LinkButton)e.Row.Cells[c].FindControl("btnRevRej_" + PeriodMenguruID));
                        LinkButton btnAprRej = ((LinkButton)e.Row.Cells[c].FindControl("btnAprRej_" + PeriodMenguruID));

                        if (rowItem.ChildCount == 0 && IsBudgetEditable)
                        {
                            lbl.Text = (ObjBudgetMenguru != null) ? ObjBudgetMenguru.Amount.ToString() : Convert.ToDecimal(0).ToString("F");

                            lbl.Visible = true;
                            btnSaved.Visible = false;
                            btnPrepared.Visible = false;
                            btnReviewed.Visible = false;
                            btnApproved.Visible = false;
                            btnRevRej.Visible = false;
                            btnAprRej.Visible = false;

                            e.Row.Cells[c].BackColor = (ObjBudgetMenguru != null) ? new Helper().GetColorByStatusValue(Convert.ToChar(ObjBudgetMenguru.Status)) : System.Drawing.Color.White;
                        }
                        else
                        {
                            int Scnt = 0;
                            int Pcnt = 0;
                            int Rcnt = 0;
                            int Acnt = 0;
                            int RRcnt = 0;
                            int ARcnt = 0;

                            decimal amount = 0;

                            List<string> ChildIDs = new List<string>() { rowItem.AccountCode };
                            List<string> RefChildIDs = new List<string>();
                            while (ChildIDs.Count > 0)
                            {
                                RefChildIDs.Clear();
                                foreach (AccountCode t in AccountCodesData.Where(x => ChildIDs.Contains(x.AccountCode1)))  //&& x.ChildCount == 0
                                {
                                    amount = amount + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID).Select(x => x.Amount).Sum();
                                    foreach (string s in AccountCodesData.Where(x => x.ParentAccountCode == t.AccountCode1).Select(x => x.AccountCode1).ToList())
                                        RefChildIDs.Add(s);
                                    if (IsBudgetEditable)
                                    {
                                        Scnt = Scnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "S").Select(x => x.BudgetMengurusID).Count();

                                        Pcnt = Pcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "P").Select(x => x.BudgetMengurusID).Count();

                                        Rcnt = Rcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "R").Select(x => x.BudgetMengurusID).Count();

                                        Acnt = Acnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "A").Select(x => x.BudgetMengurusID).Count();

                                        RRcnt = RRcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "X").Select(x => x.BudgetMengurusID).Count();

                                        ARcnt = ARcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "Y").Select(x => x.BudgetMengurusID).Count();
                                    }
                                    else
                                    {
                                        Scnt = Scnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "S").Select(x => x.BudgetMengurusID).Sum();

                                        Pcnt = Pcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "P").Select(x => x.BudgetMengurusID).Sum();

                                        Rcnt = Rcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "R").Select(x => x.BudgetMengurusID).Sum();

                                        Acnt = Acnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "A").Select(x => x.BudgetMengurusID).Sum();

                                        RRcnt = RRcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "X").Select(x => x.BudgetMengurusID).Sum();

                                        ARcnt = ARcnt + BudgetData.Where(x => x.AccountCode == t.AccountCode1 && x.PeriodMengurusID == PeriodMenguruID
                                            && x.Status == "Y").Select(x => x.BudgetMengurusID).Sum();
                                    }
                                }
                                ChildIDs.Clear();
                                foreach (string s in RefChildIDs)
                                    ChildIDs.Add(s);
                                //ChildIDs = TreeData.Where(x => ChildIDs.Contains(x.ParentAccountCode) && x.ChildCount > 0).Select(x => x.AccountCode).ToList();
                            }

                            string Sstr = (Scnt > 0) ? "<span class=\"badge\">" + Scnt.ToString() + "</span>" : string.Empty;
                            string Pstr = (Pcnt > 0) ? "<span class=\"badge\">" + Pcnt.ToString() + "</span>" : string.Empty;
                            string Rstr = (Rcnt > 0) ? "<span class=\"badge\">" + Rcnt.ToString() + "</span>" : string.Empty;
                            string Astr = (Acnt > 0) ? "<span class=\"badge\">" + Acnt.ToString() + "</span>" : string.Empty;
                            string RRstr = (RRcnt > 0) ? "<span class=\"badge\">" + RRcnt.ToString() + "</span>" : string.Empty;
                            string ARstr = (ARcnt > 0) ? "<span class=\"badge\">" + ARcnt.ToString() + "</span>" : string.Empty;

                            btnSaved.Text = Sstr;
                            btnPrepared.Text = Pstr;
                            btnReviewed.Text = Rstr;
                            btnApproved.Text = Astr;
                            btnRevRej.Text = RRstr;
                            btnAprRej.Text = ARstr;

                            lbl.Visible = false;
                            btnSaved.Visible = (Sstr != string.Empty);
                            btnPrepared.Visible = (Pstr != string.Empty);
                            btnReviewed.Visible = (Rstr != string.Empty);
                            btnApproved.Visible = (Astr != string.Empty);
                            btnRevRej.Visible = (RRstr != string.Empty);
                            btnAprRej.Visible = (ARstr != string.Empty);
                        }
                    }
                    /*End Buget logics*/
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
                //GridViewRow selectedRow = gvAccountCodes.Rows[Convert.ToInt32(e.CommandArgument)];
                if (e.CommandName == "Expand")
                {
                    GridViewRow selectedRow = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                    string AccountCode = gvAccountCodes.DataKeys[selectedRow.RowIndex]["AccountCode"].ToString();
                    List<string> SelectedNodes = (List<string>)Session["SelectedNodes"];
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
                    BuildGrid();
                    BindGrid();
                }
                else if (e.CommandName == "Page")
                {
                    gvAccountCodes.PageIndex = Convert.ToInt32(e.CommandArgument) - 1;
                    BuildGrid();
                    BindGrid();
                }
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
                List<Segment> lstSegment = new SegmentDAL().GetSegments().Where(x => x.Status == "A" && x.AccountCodeFlag == false).OrderBy(x => x.SegmentOrder).ToList();

                gvSegmentDLLs.DataSource = lstSegment;
                gvSegmentDLLs.DataBind();

                //GridView: Period
                List<PeriodMenguru> lstPeriodMengurus = new PeriodMengurusDAL().GetPeriodMengurus().Where(x => x.Status == "A" && x.MengurusYear > DateTime.Now.Year)
                    .OrderBy(x => x.MengurusYear).ThenBy(x => x.FieldMenguru.FieldMengurusDesc).ToList();

                List<FieldMenguru> obj = new FieldMenguruDAL().GetFieldMengurus().Where(x => x.Status == "F").ToList();
                Session["FixedFieldMengurus"] = obj;

                foreach (FieldMenguru item in obj)
                {
                    PeriodMenguru pm = new PeriodMenguru();

                    pm.PeriodMengurusID = item.FieldMengurusID;
                    pm.MengurusYear = DateTime.Now.Year;
                    pm.FieldMenguru = new FieldMenguru { FieldMengurusDesc = item.FieldMengurusDesc };

                    lstPeriodMengurus.Add(pm);
                }

                gvPeriod.DataSource = lstPeriodMengurus.Select(x => new
                {
                    x.PeriodMengurusID,
                    x.MengurusYear,
                    x.FieldMenguru.FieldMengurusDesc
                });
                gvPeriod.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvSegmentDLLs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Segment rowItem = (Segment)e.Row.DataItem;

                    List<SegmentDetail> lst = new SegmentDetailsDAL().GetSegmentDetails().ToList();
                    List<SegmentDetail> lstSD = new List<SegmentDetail>();

                    if (AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 3 || AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 4)
                        lstSD = lst.Where(x => x.Status == "A" && x.SegmentID == rowItem.SegmentID).ToList();
                    else
                    {
                        lstSD = AuthUser.UserSegDtlWorkflows.Where(x => x.Status == "A" && x.SegmentDetail.SegmentID == rowItem.SegmentID).Select(x => x.SegmentDetail).ToList();
                        List<int> parntids = lstSD.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();

                        if ((lstSD.Count() > 0) == false)
                        {
                            e.Row.Visible = false;
                        }

                        while (parntids.Count > 0)
                        {
                            List<SegmentDetail> lstprnts = lst.Where(x => parntids.Contains(x.SegmentDetailID)).ToList();
                            foreach (SegmentDetail o in lstprnts)
                            {
                                if (lstSD.Where(x => x.SegmentDetailID == o.SegmentDetailID).Count() == 0)
                                    lstSD.Add(o);
                            }
                            parntids = lstprnts.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                        }
                    }

                    lstSD = lstSD.OrderBy(x => x.ParentDetailID).ThenBy(x => x.DetailCode).ToList();

                    TreeNode tn = new TreeNode();
                    CreateNode(lstSD, ref tn, 0);

                    TreeView tvSegmentDDL = ((TreeView)e.Row.Cells[1].FindControl("tvSegmentDDL"));
                    for (int i = tn.ChildNodes.Count - 1; i >= 0; i--)
                    {
                        tvSegmentDDL.Nodes.Add(tn.ChildNodes[0]);
                    }

                    tvSegmentDDL.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvPeriod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string stryear = e.Row.Cells[1].Text;
                    ((CheckBox)e.Row.Cells[0].FindControl("cbPeriodSelect")).Checked = (stryear == (DateTime.Now.Year + 1).ToString());

                    int PeriodMengurusID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PeriodMengurusID"));
                    List<FieldMenguru> obj = ((List<FieldMenguru>)Session["FixedFieldMengurus"]).Where(x => x.FieldMengurusID == PeriodMengurusID).ToList();

                    if (obj.Count() > 0)
                    {
                        ((CheckBox)e.Row.Cells[0].FindControl("cbPeriodSelect")).Checked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void CreateNode(List<SegmentDetail> ListSegDtls, ref TreeNode Node, int ParentID)
        {
            try
            {
                foreach (SegmentDetail sd in ListSegDtls.Where(x => x.Status == "A" && x.ParentDetailID == ParentID))
                {
                    TreeNode tn = new TreeNode();
                    tn.Text = sd.DetailCode + " - " + sd.DetailDesc;
                    tn.Value = sd.SegmentDetailID.ToString();
                    tn.SelectAction = TreeNodeSelectAction.Select;
                    CreateNode(ListSegDtls.ToList(), ref tn, Convert.ToInt32(sd.SegmentDetailID));
                    Node.ChildNodes.Add(tn);
                }
                //return TreeNodes;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void objTemp_OnCustomStatusClicked(object sender, CustomEvenArgs e)
        {
            try
            {
                lblDecisionModalPeriodID.Text = e.PeriodID.ToString(); //hidden field
                lblDecisionModalAccountCode.Text = e.Code; //hidden field
                lblDecisionModalAccount.Text = e.Code;
                lblDecisionModalPeriod.Text = e.Period;

                LinkButton btn = (LinkButton)sender;
                string status = (btn.ID == "btnSaved_" + e.PeriodID.ToString()) ? "S" :
                                    ((btn.ID == "btnPrepared_" + e.PeriodID.ToString()) ? "P" :
                                        ((btn.ID == "btnReviewed_" + e.PeriodID.ToString()) ? "R" :
                                            ((btn.ID == "btnApproved_" + e.PeriodID.ToString()) ? "A" :
                                                ((btn.ID == "btnRevRej_" + e.PeriodID.ToString()) ? "X" :
                                                    ((btn.ID == "btnAprRej_" + e.PeriodID.ToString()) ? "Y" : string.Empty)))));

                ColorHeader.Style.Add("background-color", new Helper().GetColorByStatusValue(Convert.ToChar(status)).Name);
                //ColorHeader.Style.Add("width", "100%");
                ColorHeader.InnerText = "Budget Mengurus - " + new Helper().GetBudgetStatusEnumName(Convert.ToChar(status));

                //Grid
                List<SegmentDetail> segdtls = new SegmentDetailsDAL().GetSegmentDetails().ToList();

                //int seglength = new SegmentDAL().GetSegments().Where(x => x.Status == "A" && x.AccountCodeFlag == false).Count();
                int seglength = ((Dictionary<int, int>)Session["SegDtlsDictionary"]).Count();

                List<int> segdtlids = new List<int>();
                foreach (KeyValuePair<int, int> pair in (Dictionary<int, int>)Session["SegDtlsDictionary"])
                {
                    List<int> parntsegdtlids = new List<int>();
                    parntsegdtlids = segdtls.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                    if (pair.Value == 0)
                    {
                        foreach (int i in segdtls.Where(x => !parntsegdtlids.Contains(x.SegmentDetailID) && x.Status == "A" && x.SegmentID == pair.Key)
                                        .Select(x => x.SegmentDetailID).ToList())
                            segdtlids.Add(i);
                    }
                    else
                    {
                        if (!parntsegdtlids.Contains(pair.Value))
                            segdtlids.Add(pair.Value);
                        else
                        {
                            List<SegmentDetail> lstsubdtl = segdtls.Where(x => x.ParentDetailID == pair.Value).ToList();
                            for (int i = 0; i < lstsubdtl.Count; i++)
                            {
                                if (!parntsegdtlids.Contains(lstsubdtl[i].SegmentDetailID))
                                    segdtlids.Add(lstsubdtl[i].SegmentDetailID);
                                else
                                {
                                    foreach (SegmentDetail o in segdtls.Where(x => x.ParentDetailID == lstsubdtl[i].SegmentDetailID))
                                        lstsubdtl.Add(o);
                                }
                            }
                        }
                    }
                }

                IQueryable<AccountCode> lstacccode = new AccountCodeDAL().GetAccountCodes();
                List<string> accntcodes = new List<string>();
                List<string> parntacntcodes = lstacccode.Select(x => x.ParentAccountCode).Distinct().ToList();
                if (!parntacntcodes.Contains(e.Code))
                    accntcodes.Add(e.Code);
                else
                {
                    List<AccountCode> lstsubacccode = lstacccode.Where(x => x.ParentAccountCode == e.Code).ToList();
                    for (int i = 0; i < lstsubacccode.Count; i++)
                    {
                        if (!parntacntcodes.Contains(lstsubacccode[i].AccountCode1))
                            accntcodes.Add(lstsubacccode[i].AccountCode1);
                        else
                        {
                            string ac = lstsubacccode[i].AccountCode1;
                            foreach (AccountCode o in lstacccode.Where(x => x.ParentAccountCode == ac))
                                lstsubacccode.Add(o);
                        }
                    }
                }

                List<int> budids = new BudgetMengurusDAL().GetMengurusSegDtls()
                    .Where(x => segdtlids.Contains(x.SegmentDetailID))
                    .GroupBy(x => new
                    {
                        x.BudgetMengurusID
                    })
                    .Select(x => new
                    {
                        BudgetMengurusID = x.Key.BudgetMengurusID,
                        JuncBgtMengurusSegDtlID = x.Count()
                    })
                    .Where(x => x.JuncBgtMengurusSegDtlID == seglength)
                    .Select(x => x.BudgetMengurusID).ToList();

                List<BudgetMenguru> budData = new BudgetMengurusDAL().GetBudgetMengurus()
                    .Where(x => x.Status == status && accntcodes.Contains(x.AccountCode) && x.PeriodMengurusID == e.PeriodID && budids.Contains(x.BudgetMengurusID)).ToList();

                budids = budData.Select(y => y.BudgetMengurusID).ToList();
                var jundata = new BudgetMengurusDAL().GetMengurusSegDtls()
                    .AsEnumerable()
                    .Where(x => budids.Contains(x.BudgetMengurusID))
                    .OrderBy(x => x.BudgetMengurusID)
                    .ThenBy(x => x.SegmentDetail.Segment.SegmentOrder)
                    .GroupBy(x => new
                    {
                        x.BudgetMengurusID
                    })
                    .Select(x => new
                    {
                        BudgetMengurusID = x.Key.BudgetMengurusID,
                        //AccountCode = String.Join("-", x.Select(y => y.SegmentDetail.DetailCode)) 
                        AccountCode = x.Select(y => y.SegmentDetail.DetailCode).Aggregate((a, b) => a + "-" + b)
                        //AccountCode = x.Aggregate(string.Empty, (a, b) => a + "," + i.text)
                    })
                    .ToList();

                if (status == "S" || status == "P" || status == "R" || status == "A")
                {
                    gvModelDetails.DataSource =
                        (from x in budData
                         join y in jundata on x.BudgetMengurusID equals y.BudgetMengurusID
                         join z in new UsersDAL().GetUsers() on x.ModifiedBy equals z.UserID
                         select new
                         {
                             AccountCode = y.AccountCode + "-" + x.AccountCode,
                             Amount = x.Amount,
                             Username = z.UserName,
                             Email = z.UserEmail,
                             Contact = z.UserPhoneNo,
                             DateTime = x.ModifiedTimeStamp
                         }).ToList();
                }
                else
                {
                    gvModelDetails.DataSource =
                        (from x in budData
                         join y in jundata on x.BudgetMengurusID equals y.BudgetMengurusID
                         join z in new UsersDAL().GetUsers() on x.ModifiedBy equals z.UserID
                         select new
                         {
                             AccountCode = y.AccountCode + "-" + x.AccountCode,
                             Amount = x.Amount,
                             Remarks = x.Remarks,
                             Username = z.UserName,
                             Email = z.UserEmail,
                             Contact = z.UserPhoneNo,
                             DateTime = x.ModifiedTimeStamp
                         }).ToList();
                }
                gvModelDetails.DataBind();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "myModal", "$('#myModal').modal();", true);
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private List<int> GetSelectedPeriods()
        {
            List<int> lst = new List<int>();
            try
            {
                foreach (GridViewRow gvr in gvPeriod.Rows)
                {
                    if (((CheckBox)gvr.Cells[0].FindControl("cbPeriodSelect")).Checked)
                        lst.Add(Convert.ToInt32(gvPeriod.DataKeys[gvr.RowIndex]["PeriodMengurusID"].ToString()));
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
            return lst;
        }

        protected void tvSegmentDDL_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)((GridViewRow)((TreeView)sender).NamingContainer).FindControl("tbSegmentDDL")).Text = ((TreeView)sender).SelectedNode.Text;

                bool CanEdit = true;
                foreach (GridViewRow gvr in gvSegmentDLLs.Rows)
                {
                    TreeView tv = (TreeView)gvr.Cells[0].FindControl("tvSegmentDDL");

                    //CanEdit = (tv != null && tv.SelectedNode != null && tv.SelectedNode.ChildNodes.Count == 0 && CanEdit);
                    if (tv != null && tv.SelectedNode != null)
                    {
                        if ((tv.SelectedNode.ChildNodes.Count == 0) == false)
                        {
                            CanEdit = false;
                            break;
                        }
                    }
                }

                chkKeterangan.Checked = (CanEdit) ? chkKeterangan.Checked : false;
                chkPengiraan.Checked = (CanEdit) ? chkPengiraan.Checked : false;
                chkKeterangan.Enabled = CanEdit;
                chkPengiraan.Enabled = CanEdit;
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void chkMedan_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gvPeriod.Rows.Count; i++)
            {
                string stryear = gvPeriod.Rows[i].Cells[1].Text;
                ((CheckBox)gvPeriod.Rows[i].Cells[0].FindControl("cbPeriodSelect")).Checked = (chkMedan.Checked || (stryear == (DateTime.Now.Year + 1).ToString()));
            }
        }

        #region Manual Printing - disable as will be use jqueryDT
        private List<AccountCodeTreeHelper> CreateExportData()
        {
            List<AccountCodeTreeHelper> TreeData = new List<AccountCodeTreeHelper>();
            try
            {
                List<AccountCode> data = (List<AccountCode>)Session["AccountCodesData"];
                if (data.Count > 0)
                {
                    TreeData = data.Where(x => x.ParentAccountCode == string.Empty).OrderBy(x => x.AccountCode1).Select(x =>
                            new AccountCodeTreeHelper()
                            {
                                AccountCode = x.AccountCode1,
                                AccountDesc = x.AccountDesc,
                                Keterangan = x.Keterangan,
                                Pengiraan = x.Pengiraan,
                                ParentAccountCode = x.ParentAccountCode,
                                Status = x.Status,
                                Level = 0,
                                ChildCount = data.Where(y => y.ParentAccountCode == x.AccountCode1).Count()
                            }).ToList();

                    for (int i = 0; i < TreeData.Count; i++)
                    {
                        foreach (AccountCode sd in data.Where(x => x.ParentAccountCode == TreeData[i].AccountCode).OrderByDescending(x => x.AccountCode1))
                        {
                            AccountCodeTreeHelper objSH = new AccountCodeTreeHelper()
                            {
                                AccountCode = sd.AccountCode1,
                                AccountDesc = sd.AccountDesc,
                                Keterangan = sd.Keterangan,
                                Pengiraan = sd.Pengiraan,
                                ParentAccountCode = sd.ParentAccountCode,
                                Status = sd.Status,
                                Level = TreeData[i].Level + 1,
                                ChildCount = data.Where(y => y.ParentAccountCode == sd.AccountCode1).Count()
                            };
                            TreeData.Insert(i + 1, objSH);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
            return TreeData;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                System.Web.HttpResponse Response = System.Web.HttpContext.Current.Response;
                DataSet ds = new DataSet();
                ds.Tables.Add(GetMengurusDataTable());
                string filename = Session["PrefixAcountCode"].ToString().Substring(0, Session["PrefixAcountCode"].ToString().Length - 1);
                new ReportHelper().ToExcel(ds, "BudgetMengurus_" + filename + ".xls", ref Response);
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Failure", ex.ToString());
            }
        }

        private DataTable GetMengurusDataTable()
        {
            DataTable dt = new DataTable();
            try
            {
                List<AccountCodeTreeHelper> accountcodes = CreateExportData();
                List<PeriodMenguru> PeriodData = (List<PeriodMenguru>)Session["PeriodData"];
                List<BudgetMenguru> BudgetData = ((List<BudgetMenguru>)Session["BudgetData"]).Where(x => x.Status == "A").ToList();

                //Start Build DataTable
                DataColumn dc = new DataColumn("AccountCode");
                dt.Columns.Add(dc);
                dc = new DataColumn("Objeck");
                dt.Columns.Add(dc);
                dc = new DataColumn("Description");
                dt.Columns.Add(dc);
                if (chkKeterangan.Checked)
                {
                    dc = new DataColumn("Keterangan");
                    dt.Columns.Add(dc);
                }
                if (chkPengiraan.Checked)
                {
                    dc = new DataColumn("Pengiraan");
                    dt.Columns.Add(dc);
                }
                foreach (PeriodMenguru pm in PeriodData)
                {
                    int count = dt.Columns.Cast<DataColumn>().Where(x => x.ColumnName == pm.MengurusYear.ToString() + "_" + pm.FieldMenguru.FieldMengurusDesc).Count();
                    string colname = pm.MengurusYear.ToString() + "_" + pm.FieldMenguru.FieldMengurusDesc;
                    colname = (count == 0) ? colname : colname + "_" + (count + 1).ToString();
                    dc = new DataColumn(colname);
                    dt.Columns.Add(dc);
                }
                //End Build DataTable

                //Start pushing data into DataTable
                foreach (AccountCodeTreeHelper acchelp in accountcodes)
                {
                    int c = 0;
                    DataRow dr = dt.NewRow();

                    dr[c] = new Helper().GetLevelString(acchelp.AccountCode, acchelp.Level);
                    c++;
                    dr[c] = Session["PrefixAcountCode"].ToString() + acchelp.AccountCode;
                    c++;
                    dr[c] = acchelp.AccountDesc;
                    if (chkKeterangan.Checked)
                    {
                        c++;
                        dr[c] = acchelp.Keterangan;
                    }
                    if (chkPengiraan.Checked)
                    {
                        c++;
                        dr[c] = acchelp.Pengiraan;
                    }

                    foreach (PeriodMenguru pm in PeriodData)
                    {
                        BudgetMenguru ObjBudgetMenguru = BudgetData.Where(x => x.AccountCode == acchelp.AccountCode && x.PeriodMengurusID == pm.PeriodMengurusID).FirstOrDefault();
                        decimal amount = Convert.ToDecimal(0);
                        if (acchelp.ChildCount == 0)
                        {
                            amount = (ObjBudgetMenguru != null) ? ObjBudgetMenguru.Amount : Convert.ToDecimal(0);
                        }
                        else
                        {
                            List<string> ChildIDs = new List<string>() { acchelp.AccountCode };
                            List<string> RefChildIDs = new List<string>();
                            while (ChildIDs.Count > 0)
                            {
                                RefChildIDs.Clear();
                                foreach (AccountCodeTreeHelper t in accountcodes.Where(x => ChildIDs.Contains(x.AccountCode)))
                                {
                                    amount = amount + BudgetData.Where(x => x.AccountCode == t.AccountCode && x.PeriodMengurusID == pm.PeriodMengurusID).Select(x => x.Amount).Sum();
                                    foreach (string s in accountcodes.Where(x => x.ParentAccountCode == t.AccountCode).Select(x => x.AccountCode).ToList())
                                        RefChildIDs.Add(s);
                                }
                                ChildIDs.Clear();
                                foreach (string s in RefChildIDs)
                                    ChildIDs.Add(s);
                            }
                        }

                        c++;
                        dr[c] = amount.ToString("F");
                    }
                    dt.Rows.Add(dr);
                }
                //End pushing data into DataTable
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
            return dt;
        }
        #endregion

        protected void gvAccountCodes_PreRender(object sender, EventArgs e)
        {
            if (gvAccountCodes.Rows.Count > 0)
            {
                gvAccountCodes.UseAccessibleHeader = true;
                gvAccountCodes.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvAccountCodes.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        #region Populate Search Result - TBA
        protected void PopulateSearchResult()
        {
            tvSearchResult.Nodes.Clear();

            //populate Search Result - Segment Details - START
            List<SegmentDetail> ListSegDtls = new List<SegmentDetail>();

            if (AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 3 || AuthUser.JuncUserRoles.FirstOrDefault().RoleID == 4)
                ListSegDtls = new SegmentDetailsDAL().GetSegmentDetails().ToList().Where(x => x.Status == "A").ToList();
            else
            {
                ListSegDtls = AuthUser.UserSegDtlWorkflows.Where(x => x.Status == "A").Select(x => x.SegmentDetail).ToList();
                List<int> ParentIDs = ListSegDtls.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();

                while (ParentIDs.Count > 0)
                {
                    List<SegmentDetail> lstParObj = new SegmentDetailsDAL().GetSegmentDetails().ToList().Where(x => ParentIDs.Contains(x.SegmentDetailID)).ToList();
                    foreach (SegmentDetail o in lstParObj)
                    {
                        if (ListSegDtls.Where(x => x.SegmentDetailID == o.SegmentDetailID).Count() == 0)
                            ListSegDtls.Add(o);
                    }
                    ParentIDs = lstParObj.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                }
            }

            ListSegDtls = ListSegDtls.OrderBy(x => x.ParentDetailID).ThenBy(x => x.DetailCode).ToList();

            foreach (Segment segment in new SegmentDAL().GetSegments().Where(w => ListSegDtls.Select(y => y.Segment.SegmentID).Contains(w.SegmentID))
                .OrderBy(x => x.SegmentOrder).ToList())
            {
                TreeNode parentNode = new TreeNode(segment.SegmentName, segment.SegmentID.ToString());
                parentNode.SelectAction = TreeNodeSelectAction.None;

                //find deep level nodes
                CreateSearhResultNode(ListSegDtls, ref parentNode, segment.SegmentID, 0);

                tvSearchResult.Nodes.Add(parentNode);
                tvSearchResult.ExpandAll();
            }

            foreach (TreeNode parentNode in tvSearchResult.Nodes)
            {
                FindAllNodes(parentNode);
            }
            //populate Search Result - Segment Details - END

            //populate Search Result - Selected Period - START
            BuildSelectedPeriodList();
            //populate Search Result - Selected Period - END
        }

        protected void CreateSearhResultNode(List<SegmentDetail> ListSegDtls, ref TreeNode parent, int SegmentID, int ParentID)
        {
            foreach (SegmentDetail sd in ListSegDtls.Where(x => x.Status == "A" && x.SegmentID == SegmentID && x.ParentDetailID == ParentID))
            {
                TreeNode childNode = new TreeNode(sd.DetailCode + " - " + sd.DetailDesc, sd.SegmentDetailID.ToString());
                childNode.SelectAction = TreeNodeSelectAction.None;

                //recursively constructing nodes
                CreateSearhResultNode(ListSegDtls, ref childNode, Convert.ToInt32(sd.SegmentID), Convert.ToInt32(sd.SegmentDetailID));

                parent.ChildNodes.Add(childNode);
            }
        }

        public void FindAllNodes(TreeNode parentNode)
        {
            List<int> SelectedSegDtls = new SegmentDetailsDAL().AllLeafDetails(GetSelectedSegmentDetails());

            foreach (TreeNode childNode in parentNode.ChildNodes)
            {
                if (SelectedSegDtls.Contains(Convert.ToInt32(childNode.Value)))
                {
                    childNode.ShowCheckBox = true;
                    childNode.Checked = true;
                    childNode.Text = "<span class=\"label label-white middle\">" + childNode.Text + "</span>";
                }

                FindAllNodes(childNode);
            }
        }

        protected void BuildSelectedPeriodList()
        {
            List<PeriodMenguru> PeriodData = (List<PeriodMenguru>)Session["PeriodData"];

            //generate random color - start
            List<string> lstItemColor = new List<string>();
            lstItemColor.Add("item-orange");
            lstItemColor.Add("item-red");
            lstItemColor.Add("item-default");
            lstItemColor.Add("item-blue");
            lstItemColor.Add("item-grey");
            lstItemColor.Add("item-green");
            lstItemColor.Add("item-pink");

            Random rnd = new Random();
            //generate random color - end

            //start build innerhtml
            SelectedPeriod.InnerHtml = "<ul id=\"tasks\" class=\"item-list\">";

            foreach (PeriodMenguru pm in PeriodData)
            {
                int r = rnd.Next(lstItemColor.Count);
                string str = "<b>" + pm.MengurusYear.ToString() + "</b>" + " - " + pm.FieldMenguru.FieldMengurusDesc;

                SelectedPeriod.InnerHtml += "<li class=\" " + (string)lstItemColor[r] + " clearfix\">";
                SelectedPeriod.InnerHtml += "<label class=\"inline\"><input type=\"checkbox\" checked=\"checked\" class=\"ace\" /> " +
                                  "<span class=\"lbl\"> " + str + "</span>" +
                                  "</label></li>";
            }

            SelectedPeriod.InnerHtml += "</ul>";
        }
        #endregion

    }
}