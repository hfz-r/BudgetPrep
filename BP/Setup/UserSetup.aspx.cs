﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;
using Newtonsoft.Json;
using OBSecurity;
using System.Web.UI.HtmlControls;

namespace BP.Setup
{
    public class User 
    {
        [JsonProperty("ctl00$MainContent$username")]
        public string username { get; set; }
        [JsonProperty("ctl00$MainContent$password")]
        public string password { get; set; }
        [JsonProperty("ctl00$MainContent$password2")]
        public string password2 { get; set; }
        [JsonProperty("ctl00$MainContent$email")]
        public string email { get; set; }
        [JsonProperty("ctl00$MainContent$question")]
        public string question { get; set; }
        [JsonProperty("ctl00$MainContent$answer")]
        public string answer { get; set; }
        [JsonProperty("ctl00$MainContent$status")]
        public string status { get; set; }
        [JsonProperty("ctl00$MainContent$role")]
        public string role { get; set; }
        [JsonProperty("ctl00$MainContent$fullname")]
        public string fullname { get; set; }
        [JsonProperty("ctl00$MainContent$icno")]
        public string icno { get; set; }
        [JsonProperty("ctl00$MainContent$title")]
        public string title { get; set; }
        [JsonProperty("ctl00$MainContent$phone")]
        public string phone { get; set; }
        [JsonProperty("ctl00$MainContent$fax")]
        public string fax { get; set; }
        [JsonProperty("ctl00$MainContent$designation")]
        public string designation { get; set; }
        [JsonProperty("ctl00$MainContent$dept")]
        public string dept { get; set; }
        [JsonProperty("ctl00$MainContent$grade")]
        public string grade { get; set; }
        [JsonProperty("ctl00$MainContent$period")]
        public string period { get; set; }
        [JsonProperty("ctl00$MainContent$offaddress")]
        public string offaddress { get; set; }
    }

    public class ReturnValue 
    { 
        public string pageTitle;
        public string pageBody;
    }

    public partial class UserSetup : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tvSegmentDetails.Attributes.Add("onclick", "postBackByObject(event)");
                tvMengurus.Attributes.Add("onclick", "postBackByObject(event)");

                GetData();
                Session["UsersPageMode"] = Helper.PageMode.New;
            }
        }

        #region Gridview Pre-Render

        protected void gvUsers_PreRender(object sender, EventArgs e)
        {
            if (gvUsers.Rows.Count > 0)
            {
                gvUsers.UseAccessibleHeader = true;
                gvUsers.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvUsers.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void gvPerjawatanWorkFlow_PreRender(object sender, EventArgs e)
        {
            if (gvPerjawatanWorkFlow.Rows.Count > 0)
            {
                gvPerjawatanWorkFlow.UseAccessibleHeader = true;
                gvPerjawatanWorkFlow.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvPerjawatanWorkFlow.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        #endregion

        protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
        {
            LinkButton btnAdd = (LinkButton)e.Row.Cells[5].FindControl("btnEditRow");
            if (btnAdd != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
            }
        }

        private void GetData()
        {
            try
            {
                List<MasterUser> data = new UsersDAL().GetUsers().ToList();

                if (data != null)
                {
                    Session["MasterUserData"] = data;
                    BindGrid();
                    LoadDropDown();
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
                gvUsers.DataSource = (List<MasterUser>)Session["MasterUserData"];
                gvUsers.DataBind();

                //Bind Mengurus Workflow - start
                foreach (AccountCode obj in new AccountCodeDAL().GetAccountCodes().ToList().Where(x => x.Status == "A"))
                {
                    if (obj.ParentAccountCode == "" || string.IsNullOrEmpty(obj.ParentAccountCode))
                    {
                        TreeNode tnParent = new TreeNode(obj.AccountCode1 + " - " + obj.AccountDesc, obj.AccountCode1);
                        tnParent.SelectAction = TreeNodeSelectAction.None;
                        tnParent.Collapse();

                        FillChild("MG", tnParent, 0, obj.AccountCode1);

                        tvMengurus.Nodes.Add(tnParent);
                    }
                }
                //Bind Mengurus Workflow - end

                List<string> ParServices = new GroupPerjawatanDAL().GetGroupPerjawatans().Select(x => x.ParentGroupPerjawatanID).Distinct().ToList();
                gvPerjawatanWorkFlow.DataSource = new GroupPerjawatanDAL().GetGroupPerjawatans().Where(x => x.Status == "A" && !ParServices.Contains(x.GroupPerjawatanCode)).ToList();
                gvPerjawatanWorkFlow.DataBind();

                //Bind Segment Details Workflow - start
                foreach (Segment segment in new SegmentDAL().GetSegments().Where(w => w.Status == "A" && w.AccountCodeFlag == false).OrderBy(x => x.SegmentOrder).ToList())
                {
                    TreeNode segmentNode = new TreeNode(segment.SegmentName, segment.SegmentID.ToString());
                    segmentNode.SelectAction = TreeNodeSelectAction.None;
                    segmentNode.Collapse();

                    foreach (SegmentDetail obj in new SegmentDetailsDAL().GetSegmentDetails().ToList().Where(x => x.Status == "A" && x.SegmentID == segment.SegmentID))
                    {
                        if (obj.ParentDetailID == 0)
                        {
                            TreeNode tnParent = new TreeNode(obj.DetailCode + " - " + obj.DetailDesc, obj.SegmentDetailID.ToString());
                            tnParent.SelectAction = TreeNodeSelectAction.None;
                            tnParent.Collapse();

                            segmentNode.ChildNodes.Add(tnParent);
                            FillChild("SD", tnParent, obj.SegmentDetailID, string.Empty);
                        }
                    }

                    tvSegmentDetails.Nodes.Add(segmentNode);
                }
                //Bind Segment Details Workflow - end
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void FillChild(string type, TreeNode parent, int ParentId = 0, string ParentAccountCode = "")
        {
            if (type == "SD")
            {
                var nodes = new SegmentDetailsDAL().GetSegmentDetails().ToList().Where(x => x.Status == "A" && x.ParentDetailID == ParentId);
                foreach (var node in nodes)
                {
                    TreeNode child = new TreeNode(node.DetailCode + " - " + node.DetailDesc, node.SegmentDetailID.ToString());
                    child.SelectAction = TreeNodeSelectAction.None;
                    child.Collapse();

                    parent.ChildNodes.Add(child);
                    FillChild("SD", child, node.SegmentDetailID, string.Empty);
                }
            }
            else if (type == "MG")
            {
                var nodes = new AccountCodeDAL().GetAccountCodes().ToList().Where(x => x.Status == "A" && x.ParentAccountCode == ParentAccountCode);
                foreach (var node in nodes)
                {
                    TreeNode child = new TreeNode(node.AccountCode1 + " - " + node.AccountDesc, node.AccountCode1);
                    child.SelectAction = TreeNodeSelectAction.None;
                    child.Collapse();

                    parent.ChildNodes.Add(child);
                    FillChild("MG", child, 0, node.AccountCode1);
                }
            }
            
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    ClearPageData();

                    GridViewRow selectedRow = gvUsers.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "skyblue";

                    MasterUser objMasterUser = DAL.UsersDAL.StaticUserId(Convert.ToInt32(gvUsers.DataKeys[selectedRow.RowIndex]["UserID"]), "");
                    MembershipUser _MembershipUser = Membership.GetUser(objMasterUser.UserName);

                    if ((Guid)_MembershipUser.ProviderUserKey == objMasterUser.UUID)
                    {
                        Session["SelectedMasterUser"] = objMasterUser;

                        username.Value = objMasterUser.UserName.Trim();
                        email.Value = objMasterUser.UserEmail.Trim();
                        question.Value = objMasterUser.SecQuestion.Trim();
                        answer.Value = Security.Decrypt(objMasterUser.SecAnswer.Trim());
                        fullname.Value = objMasterUser.FullName.Trim();
                        icno.Value = objMasterUser.UserIC.Trim();
                        dept.Value = objMasterUser.Department.Trim();
                        phone.Value = objMasterUser.UserPhoneNo.Trim();
                        designation.Value = objMasterUser.Designation.Trim();
                        fax.Value = objMasterUser.Fax.Trim();
                        offaddress.Value = objMasterUser.OfficeAddress.Trim();
                        period.Value = objMasterUser.PeriodOfService.Trim();
                        grade.Value = objMasterUser.PositionGrade.Trim();
                        title.Value = objMasterUser.Title.Trim();

                        status.SelectedIndex = -1;
                        status.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objMasterUser.UserStatus))).Selected = true;

                        role.SelectedIndex = -1;
                        ListItem item = role.Items.FindByValue(new UsersRoleDAL().ListUserRole().Where(x => x.UserID == objMasterUser.UserID)
                            .Select(y => Convert.ToString(y.RoleID)).FirstOrDefault());
                        if (item != null)
                        {
                            role.SelectedValue = item.Value;
                        }

                        /*User Mengurus Workflow - start*/
                        List<AccountCode> lstAccCode = objMasterUser.UserMengurusWorkflows.Where(x => x.Status == "A").Select(x => x.AccountCode1).ToList();
                        List<string> lstParAccCode = lstAccCode.Select(x => x.ParentAccountCode).Distinct().ToList();
                        while (lstParAccCode.Count() > 0)
                        {
                            List<AccountCode> lstObjPar = new AccountCodeDAL().GetAccountCodes().ToList().Where(x => lstParAccCode.Contains(x.AccountCode1)).ToList();
                            foreach (AccountCode ac in lstObjPar)
                            {
                                if (lstAccCode.Where(x => x.AccountCode1 == ac.AccountCode1).Count() == 0)
                                    lstAccCode.Add(ac);
                            }
                            lstParAccCode = lstObjPar.Select(x => x.ParentAccountCode).Distinct().ToList();
                        }

                        PopulateACChild(tvMengurus.Nodes, lstAccCode);
                        /*User Mengurus Workflow - end*/

                        /*User Perjawatan Workflow - start*/
                        List<string> lstServiceCode = objMasterUser.UserPerjawatanWorkflows.Where(x => x.Status == "A").Select(x => x.GroupPerjawatanCode).ToList();
                        for (int i = 0; i < gvPerjawatanWorkFlow.Rows.Count; i++)
                        {
                            ((CheckBox)gvPerjawatanWorkFlow.Rows[i].Cells[0].FindControl("chkSelect")).Checked = lstServiceCode.Contains(gvPerjawatanWorkFlow.DataKeys[i]["GroupPerjawatanCode"].ToString());
                        }
                        /*User Perjawatan Workflow - end*/

                        /*User Segment Details Workflow - start*/
                        List<SegmentDetail> lstSegDtls = objMasterUser.UserSegDtlWorkflows.Where(x => x.Status == "A").Select(x => x.SegmentDetail).ToList();
                        List<int> lstParSegDtls = lstSegDtls.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                        while (lstParSegDtls.Count > 0)
                        {
                            List<SegmentDetail> lstParObj = new SegmentDetailsDAL().GetSegmentDetails().ToList().Where(x => lstParSegDtls.Contains(x.SegmentDetailID)).ToList();
                            foreach (SegmentDetail o in lstParObj)
                            {
                                if (lstSegDtls.Where(x => x.SegmentDetailID == o.SegmentDetailID).Count() == 0)
                                    lstSegDtls.Add(o);
                            }
                            lstParSegDtls = lstParObj.Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                        }

                        Boolean TVcheckbox = false;
                        foreach (TreeNode node in tvSegmentDetails.Nodes)
                        {
                            TVcheckbox = PopulateSDChild(node, lstSegDtls, 0);

                            if (TVcheckbox == true)
                            {
                                node.Checked = true;
                            }
                        }
                        /*User Segment Details Workflow - end*/

                        //Change workflow button colors accordingly - start
                        if (role.SelectedValue != "3")
                        {
                            CustomizeButtonWorkflow(lstAccCode.Select(x => x.AccountCode1).ToList(), lstServiceCode, lstSegDtls.Select(x => x.SegmentDetailID).ToList());
                        }
                        //Change workflow button colors accordingly - end

                        ChangePageMode(Helper.PageMode.Edit);
                        form_Wiz.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        #region Segment Details
        protected Boolean PopulateSDChild(TreeNode nodes, List<SegmentDetail> ListSegDtls, int ParentID)
        {
            Boolean TVcheckbox = false;

            foreach (SegmentDetail obj in ListSegDtls.Where(x => x.Status == "A" && x.ParentDetailID == ParentID))
            {
                foreach (TreeNode subnode in nodes.ChildNodes)
                {
                    if (Convert.ToInt32(subnode.Value) == obj.SegmentDetailID || Convert.ToInt32(subnode.Value) == obj.ParentDetailID)
                    {
                        subnode.Checked = true;
                        TVcheckbox = true;
                    }

                    PopulateSDChild(subnode, ListSegDtls, Convert.ToInt32(obj.SegmentDetailID));
                }
            }

            return TVcheckbox;
        }
        #endregion

        #region Account Code/Mengurus
        protected void PopulateACChild(TreeNodeCollection nodes, List<AccountCode> lstAccCode)
        {
            foreach (AccountCode ac in lstAccCode.Where(x => x.Status == "A"))
            {
                foreach (TreeNode subnode in nodes)
                {
                    if (subnode.Value == ac.AccountCode1 || subnode.Value == ac.ParentAccountCode)
                    {
                        subnode.Checked = true;
                    }

                    PopulateACChild(subnode.ChildNodes, lstAccCode);
                }
            }
        }
        #endregion

        private void CustomizeButtonWorkflow(List<string> lstAccountCode, List<string> lstServiceCode, List<int> lstSegDtls)
        {
            var WorkflowBtn = (HtmlButton)btnWorkflow.FindControl("btnWorkflow");

            if (lstAccountCode.Count() > 0 || lstServiceCode.Count() > 0 || lstSegDtls.Count() > 0)
            {
                WorkflowBtn.InnerHtml = "<span class=\"ace-icon fa fa-unlock bigger-110 tooltip-info\" " +
                                        "data-rel=\"tooltip\" data-placement=\"bottom\" title=\"Workflow`s configured.\">&nbsp;Workflow</span>";
                WorkflowBtn.Attributes["class"] = "btn btn-info";
            }
            else
            {
                WorkflowBtn.InnerHtml = "<span class=\"ace-icon fa fa-lock bigger-110 tooltip-error\" " +
                                        "data-rel=\"tooltip\" data-placement=\"bottom\" title=\"Workflow`s NOT configured.\">&nbsp;Workflow</span>";
                WorkflowBtn.Attributes["class"] = "btn btn-danger";
            }
        }

        [WebMethod]
        public static ReturnValue FormValues(string obj)
        {
            var dt = new ReturnValue();
            MembershipCreateStatus result = new MembershipCreateStatus();

            User _Users = JsonConvert.DeserializeObject<User>(obj);

            try
            {
                if ((Helper.PageMode)HttpContext.Current.Session["UsersPageMode"] == Helper.PageMode.New)
                {
                    MembershipUser newUser = Membership.CreateUser(_Users.username, _Users.password, _Users.email, _Users.question,
                   _Users.answer, true, out result);

                    if (result.ToString() == "Success")
                    {
                        if (new UsersDAL().GetUsers().Where(x => x.UserName.ToUpper().Trim() == _Users.username.ToUpper().Trim()).Count() > 0)
                        {
                            dt.pageTitle = "Failure";
                            dt.pageBody = "Username already exists";
                            return dt;
                        }

                        MembershipUser _MembershipUser = Membership.GetUser(newUser.UserName);
                        Guid UserId = (Guid)_MembershipUser.ProviderUserKey;

                        MasterUser objMasterUser = new MasterUser();
                        objMasterUser.UUID = UserId;
                        objMasterUser.UserName = _Users.username.Trim();
                        objMasterUser.UserPassword = _MembershipUser.GetPassword(_Users.answer.Trim());
                        objMasterUser.FullName = _Users.fullname.Trim();
                        objMasterUser.UserEmail = _Users.email.Trim();
                        objMasterUser.UserIC = _Users.icno.Trim();
                        objMasterUser.Department = _Users.dept.Trim();
                        objMasterUser.UserPhoneNo = _Users.phone.Trim();
                        objMasterUser.Designation = _Users.designation.Trim();
                        objMasterUser.Fax = _Users.fax.Trim();
                        objMasterUser.OfficeAddress = _Users.offaddress.Trim();
                        objMasterUser.PeriodOfService = _Users.period.Trim();
                        objMasterUser.PositionGrade = _Users.grade.Trim();
                        objMasterUser.Title = _Users.title.Trim();
                        objMasterUser.SecQuestion = _Users.question.Trim();
                        objMasterUser.SecAnswer = _Users.answer.Trim();
                        objMasterUser.UserStatus = new Helper().GetItemStatusEnumValueByName(_Users.status.Trim());
                        objMasterUser.CreatedBy = DAL.UsersDAL.StaticUserId(0, HttpContext.Current.User.Identity.Name).UserID;
                        objMasterUser.CreatedTimeStamp = DateTime.Now;
                        objMasterUser.ModifiedBy = DAL.UsersDAL.StaticUserId(0, HttpContext.Current.User.Identity.Name).UserID;
                        objMasterUser.ModifiedTimeStamp = DateTime.Now;

                        //get workflow setup - start
                         List<UserMengurusWorkflow> lstUserMengurus =  new List<UserMengurusWorkflow>();
                         List<UserPerjawatanWorkflow> lstUserPerjawatan = new List<UserPerjawatanWorkflow>();
                         List<UserSegDtlWorkflow> lstUserSegmentDetails = new List<UserSegDtlWorkflow>();

                         GetWorkflowSetup(objMasterUser, out lstUserMengurus, out lstUserPerjawatan, out lstUserSegmentDetails);
                        //get workflow setup - end

                        int USERID = 0;
                        if (new UsersDAL().InsertUsers(objMasterUser, lstUserMengurus, lstUserPerjawatan, lstUserSegmentDetails, ref USERID))
                        {
                            objMasterUser.UserID = USERID;
                            if (new UserSetup().AddUserRole(objMasterUser, _Users.role) == false)
                            {
                                dt.pageTitle = "Failure";
                                dt.pageBody = "An error occurred while creating User";
                                throw new Exception();
                            }

                            bool mail = MailHelper.SendMail(objMasterUser, _MembershipUser.GetPassword(_Users.answer.Trim()));

                            dt.pageTitle = "Success";
                            dt.pageBody = "User created successfully updated." + ((mail) ? "Mail Sent to " + new Helper().EmailClipper(_MembershipUser.Email) + " for confirmation." : "Error Sending Mail. Please check your connection.");
                        }
                        else
                        {
                            dt.pageTitle = "Failure";
                            dt.pageBody = "An error occurred while creating User";
                        }
                    }
                    else
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = GetErrorMessage(result);
                        return dt;
                    }
                }
                else if ((Helper.PageMode)HttpContext.Current.Session["UsersPageMode"] == Helper.PageMode.Edit)
                {
                    MasterUser objMasterUser = (MasterUser)HttpContext.Current.Session["SelectedMasterUser"];
                    objMasterUser.UserID = DAL.UsersDAL.StaticUserId(0, _Users.username.Trim()).UserID;
                    objMasterUser.UserName = _Users.username.Trim();
                    objMasterUser.FullName = _Users.fullname.Trim();
                    objMasterUser.UserEmail = _Users.email.Trim();
                    objMasterUser.UserIC = _Users.icno.Trim();
                    objMasterUser.Department = _Users.dept.Trim();
                    objMasterUser.UserPhoneNo = _Users.phone.Trim();
                    objMasterUser.Designation = _Users.designation.Trim();
                    objMasterUser.Fax = _Users.fax.Trim();
                    objMasterUser.OfficeAddress = _Users.offaddress.Trim();
                    objMasterUser.PeriodOfService = _Users.period.Trim();
                    objMasterUser.PositionGrade = _Users.grade.Trim();
                    objMasterUser.Title = _Users.title.Trim();
                    objMasterUser.SecQuestion = _Users.question.Trim();
                    objMasterUser.UserStatus = new Helper().GetItemStatusEnumValueByName(_Users.status.Trim());
                    objMasterUser.ModifiedBy = DAL.UsersDAL.StaticUserId(0, HttpContext.Current.User.Identity.Name).UserID;
                    objMasterUser.ModifiedTimeStamp = DateTime.Now;

                    if (new UserSetup().AddUserRole(objMasterUser, _Users.role) == false)
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = "An error occurred while creating User";
                        throw new Exception();
                    }

                    //get workflow setup - start
                    List<UserMengurusWorkflow> lstUserMengurus = new List<UserMengurusWorkflow>();
                    List<UserPerjawatanWorkflow> lstUserPerjawatan = new List<UserPerjawatanWorkflow>();
                    List<UserSegDtlWorkflow> lstUserSegmentDetails = new List<UserSegDtlWorkflow>();

                    GetWorkflowSetup(objMasterUser, out lstUserMengurus, out lstUserPerjawatan, out lstUserSegmentDetails);
                    //get workflow setup - end

                    if (new UsersDAL().UpdateUsers(objMasterUser, _Users.role, lstUserMengurus, lstUserPerjawatan, lstUserSegmentDetails))
                    {
                        MembershipUser u = Membership.GetUser(objMasterUser.UserName);
                        u.Email = objMasterUser.UserEmail;
                        Membership.UpdateUser(u);

                        dt.pageTitle = "Success";
                        dt.pageBody = "User updated successfully";
                    }
                    else
                    {
                        dt.pageTitle = "Failure";
                        dt.pageBody = "An error occurred while updating User";
                    }
                }
            }
            catch
            {
                dt.pageTitle = "Failure";
                dt.pageBody = GetErrorMessage(result);
            }

            return dt;
        }

        private static void GetWorkflowSetup(MasterUser objMasterUser, 
            out List<UserMengurusWorkflow> lstUserMengurus,
            out List<UserPerjawatanWorkflow> lstUserPerjawatan,
            out List<UserSegDtlWorkflow> lstUserSegmentDetails)
        {
            /*User Mengurus Workflow - start*/
            lstUserMengurus = (List<UserMengurusWorkflow>)HttpContext.Current.Session["UserMengurusWorkflow"];
            if (lstUserMengurus == null || lstUserMengurus.Count() == 0)
            {
                lstUserMengurus = new List<UserMengurusWorkflow>();
            }
            else
            {
                for (int i = 0; i < lstUserMengurus.Count(); i++)
                {
                    string AccountCode = lstUserMengurus[i].AccountCode;

                    var objUM = lstUserMengurus.Where(o => o.AccountCode == AccountCode).FirstOrDefault();
                    if (objUM != null) { objUM.MasterUser = objMasterUser; }
                }
            }
            /*User Mengurus Workflow - end*/

            /*User Perjawatan Workflow - start*/
            lstUserPerjawatan = (List<UserPerjawatanWorkflow>)HttpContext.Current.Session["UserPerjawatanWorkflow"];
            if (lstUserPerjawatan == null || lstUserPerjawatan.Count() == 0)
            {
                lstUserPerjawatan = new List<UserPerjawatanWorkflow>();
            }
            else
            {
                for (int i = 0; i < lstUserPerjawatan.Count(); i++)
                {
                    string PerjawatanCode = lstUserPerjawatan[i].GroupPerjawatanCode;

                    var objUP = lstUserPerjawatan.Where(o => o.GroupPerjawatanCode == PerjawatanCode).FirstOrDefault();
                    if (objUP != null) { objUP.MasterUser = objMasterUser; }
                }
            }
            /*User Perjawatan Workflow - end*/

            /*User Segment Details Workflow - start*/
            lstUserSegmentDetails = (List<UserSegDtlWorkflow>)HttpContext.Current.Session["UserSegmentDetailsWorkflow"];
            if (lstUserSegmentDetails == null || lstUserSegmentDetails.Count() == 0)
            {
                lstUserSegmentDetails = new List<UserSegDtlWorkflow>();
            }
            else
            {
                for (int i = 0; i < lstUserSegmentDetails.Count(); i++)
                {
                    int SegmentDetailsId = Convert.ToInt32(lstUserSegmentDetails[i].SegmentDetailID);

                    var objUSG = lstUserSegmentDetails.Where(o => o.SegmentDetailID == SegmentDetailsId).FirstOrDefault();
                    if (objUSG != null)  { objUSG.MasterUser = objMasterUser; }
                }
            }
            /*User Segment Details Workflow - end*/
        }

        protected bool AddUserRole(MasterUser objMasterUser, string role)
        {
            MasterRole objMasterRole = new UsersRoleDAL().GetRoles().Where(x => x.RoleID == Convert.ToInt32(role)).FirstOrDefault();
            if (objMasterRole != null)
            {
                //back-end level;membership checking
                if (!Roles.IsUserInRole(objMasterUser.UserName, objMasterRole.RoleName))
                {
                    string[] getRoles = Roles.GetRolesForUser(objMasterUser.UserName);
                    if (getRoles.Count() > 0)
                    {
                        Roles.RemoveUserFromRoles(objMasterUser.UserName, getRoles);
                    }
                    Roles.AddUserToRole(objMasterUser.UserName, objMasterRole.RoleName);
                }

                //local level checking
                JuncUserRole objUserRole = new JuncUserRole();
                objUserRole.RoleID = Convert.ToInt32(role);
                objUserRole.UserID = objMasterUser.UserID;
                objUserRole.Status = "A";

                if (new UsersRoleDAL().InsertUserRole(objUserRole))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ChangePageMode(Helper.PageMode.New);
            ClearPageData();
            form_Wiz.Visible = true;
            //Response.Redirect(Request.RawUrl);
        }

        public static string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        private void LoadDropDown()
        {
            try
            {
                status.DataSource = Enum.GetValues(typeof(Helper.ItemStatus));
                status.DataBind();

                role.DataSource = new UsersRoleDAL().GetRoles().OrderBy(x=>x.RoleID);
                role.DataTextField = "RoleName";
                role.DataValueField = "RoleID";
                role.DataBind();
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
                    username.Attributes.Remove("readonly");
                    question.Attributes.Remove("readonly");
                    answer.Attributes.Remove("readonly");
                    widget_title.InnerText = "User Setup - New";
                    pwdDiv.Visible = true;
                    pwd2Div.Visible = true;
                    SecAns.Visible = true;
                    break;
                case Helper.PageMode.Edit:
                    username.Attributes.Add("readonly", "readonly");
                    question.Attributes.Add("readonly", "readonly");
                    answer.Attributes.Add("readonly", "readonly");
                    widget_title.InnerText = "User Setup - Edit";
                    pwdDiv.Visible = false;
                    pwd2Div.Visible = false;
                    SecAns.Visible = false;
                    break;
            }
            Session["UsersPageMode"] = pagemode;
        }

        private void ClearPageData()
        {
            try
            {
                username.Value = string.Empty;
                password.Value = string.Empty;
                password2.Value = string.Empty;
                email.Value = string.Empty;
                question.Value = string.Empty;
                answer.Value = string.Empty;

                status.SelectedIndex = 0;

                foreach (GridViewRow gvr in gvUsers.Rows)
                    gvr.Style["background-color"] = "";

                List<GridViewRowCollection> WorkflowGridview = new List<GridViewRowCollection>();
                WorkflowGridview.Add(gvPerjawatanWorkFlow.Rows);

                for (int i = 0; i < WorkflowGridview.Count(); i++)
                {
                    foreach (GridViewRow gvr in WorkflowGridview[i])
                    {
                        CheckBox cb = (CheckBox)gvr.Cells[0].FindControl("chkSelect");
                        cb.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<MasterUser> data = (List<MasterUser>)Session["MasterUserData"];
                var span = ((HtmlGenericControl)e.Row.Cells[4].FindControl("CustomStatus"));
                
                int UserId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "UserID"));
                string UserStatus = data.Where(x => x.UserID==UserId).Select(y => y.UserStatus).FirstOrDefault();
                bool LockedOut = Membership.GetUser(e.Row.Cells[1].Text).IsLockedOut;

                if (LockedOut == true)
                {
                    span.InnerHtml = "<span class=\"label label-sm label-warning arrowed-in arrowed-in-right tooltip-warning\" data-rel=\"tooltip\" " +
                                     "data-placement=\"right\" title=\"Account has been locked out because of too many invalid login attempts.\">Locked</span>";
                }
                else
                {
                    if (UserStatus == "A")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" " +
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                    }
                    else if (UserStatus == "D")
                    {
                        span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" " +
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                    }
                }
            }
        }

        protected void btnSaveWorkflow_OnClick(object sender, EventArgs e)
        {
            try
            {
                /*User Mengurus Workflow - start*/
                List<UserMengurusWorkflow> lstAccountCode = new List<UserMengurusWorkflow>();
                GetNodeRecursiveAC(tvMengurus.Nodes, lstAccountCode);

                List<string> ParAccCode = new AccountCodeDAL().GetAccountCodes().ToList().Select(x => x.ParentAccountCode).Distinct().ToList();
                for (int i = 0; i < ParAccCode.Count; i++)
                {
                    lstAccountCode = lstAccountCode.Where(x => x.AccountCode != ParAccCode[i]).ToList();
                }

                Session["UserMengurusWorkflow"] = lstAccountCode;
                /*User Mengurus Workflow - end*/

                /*User Perjawatan Workflow - START*/
                List<UserPerjawatanWorkflow> lstServiceCode = new List<UserPerjawatanWorkflow>();
                for (int i = 0; i < gvPerjawatanWorkFlow.Rows.Count; i++)
                {
                    if (((CheckBox)gvPerjawatanWorkFlow.Rows[i].Cells[0].FindControl("chkSelect")).Checked)
                        lstServiceCode.Add(new UserPerjawatanWorkflow()
                        {
                            GroupPerjawatanCode = gvPerjawatanWorkFlow.DataKeys[i]["GroupPerjawatanCode"].ToString(),
                            Status = "A"
                        });
                }

                Session["UserPerjawatanWorkflow"] = lstServiceCode;
                /*User Perjawatan Workflow - END*/

                /*User Segment Details Workflow - START*/
                List<UserSegDtlWorkflow> lstSegmentDetail = new List<UserSegDtlWorkflow>();

                foreach (TreeNode node in tvSegmentDetails.CheckedNodes)
                {
                    GetNodeRecursiveSD(node, lstSegmentDetail);
                }

                List<int> ParSegDtls = new SegmentDetailsDAL().GetSegmentDetails().ToList().Select(x => Convert.ToInt32(x.ParentDetailID)).Distinct().ToList();
                for (int i = 0; i < ParSegDtls.Count; i++)
                {
                    lstSegmentDetail = lstSegmentDetail.Where(x => x.SegmentDetailID != ParSegDtls[i]).ToList();
                }

                Session["UserSegmentDetailsWorkflow"] = lstSegmentDetail;
                /*User Segment Details Workflow - END*/

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "GetWorkflowFlag", "WorkflowFlag();", true);
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        #region Segment Details
        protected void GetNodeRecursiveSD(TreeNode nodes, List<UserSegDtlWorkflow> lstSegmentDetail)
        {
            foreach (TreeNode subnode in nodes.ChildNodes)
            {
                if (subnode.Checked)
                {
                    lstSegmentDetail.Add(new UserSegDtlWorkflow()
                    {
                        SegmentDetailID = Convert.ToInt32(subnode.Value),
                        Status = "A"
                    });
                }
            }
        }
        #endregion

        #region Account Code
        protected void GetNodeRecursiveAC(TreeNodeCollection nodes, List<UserMengurusWorkflow> lstAccountCode)
        {
            foreach (TreeNode subnode in nodes)
            {
                if (subnode.Checked)
                {
                    lstAccountCode.Add(new UserMengurusWorkflow()
                    {
                        AccountCode = subnode.Value,
                        Status = "A"
                    });
                }

                GetNodeRecursiveAC(subnode.ChildNodes, lstAccountCode);
            }
        }
        #endregion

        #region Clear Workflow Treeview
        protected void btnCancelWorkflow_OnClick(object sender, EventArgs e)
        {
            TreeView[] array = { tvSegmentDetails, tvMengurus };

            foreach (TreeView tv in array)
            {
                tv.CollapseAll();
            }
        }
        #endregion
    }
}