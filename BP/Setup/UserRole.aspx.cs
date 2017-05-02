using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL;
using BP.Classes;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace BP.Setup
{
    public partial class UserRole : PageHelper
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlRole.Enabled = false;
                GetRolesData();
                CheckRolesData();
            }
        }

        protected void gvRoles_PreRender(object sender, EventArgs e)
        {
            if (gvRoles.Rows.Count > 0)
            {
                gvRoles.UseAccessibleHeader = true;
                gvRoles.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvRoles.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        private void GetRolesData()
        {
            try
            {
                List<MasterRole> data = new UsersRoleDAL().GetRoles().OrderBy(x=>x.RoleName).ToList();

                Session["MasterRoleData"] = data;
                BindGrid();
                LoadDropDown();
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
                ddlStatus.SelectedValue = "A";

                ddlRole.DataSource = new UsersRoleDAL().GetRoles();
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "RoleID";
                ddlRole.DataBind();
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
                gvRoles.DataSource = (List<MasterRole>)Session["MasterRoleData"];
                gvRoles.DataBind();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        [WebMethod]
        public static string OnSubmit(string roleid, string dlbox, string desc, string stats)
        {
            string json;
            var ReturnObj = new { status = "", result = "" };

            try
            {
                if (dlbox == "null")
                {
                    MasterRole roleObj = new MasterRole();
                    roleObj.RoleID = Convert.ToInt32(roleid);
                    roleObj.RoleName = new UsersRoleDAL().GetRoles().Where(x => x.RoleID == Convert.ToInt32(roleid)).Select(y => y.RoleName).FirstOrDefault();
                    roleObj.Description = desc.Trim();
                    roleObj.RoleStatus = new Helper().GetItemStatusEnumValueByName(stats.Trim());

                    if (new UsersRoleDAL().UpdateMasterRole(roleObj))
                    {
                        ReturnObj = new { status = "Success", result = "Role updated successfully." };
                    }
                    else
                    {
                        throw new Exception("Fail to authenticated role. Please re-enter your values and try again.");
                    }
                }
                else
                {
                    string[] values = dlbox.Split(',');
                    for (int i = 0; i < values.Count(); i++)
                    {
                        MasterUser objMasterUser = new UsersDAL().GetUserDataByID(Convert.ToInt32(values[i]));
                        MasterRole objMasterRole = new UsersRoleDAL().GetRoles().Where(x => x.RoleID == Convert.ToInt32(roleid)).FirstOrDefault();
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
                            JuncUserRole userrole = new JuncUserRole();
                            userrole.RoleID = Convert.ToInt32(roleid);
                            userrole.UserID = objMasterUser.UserID;
                            userrole.Status = new Helper().GetItemStatusEnumValueByName(stats.Trim());
                            
                            if (new UsersRoleDAL().UserRoleFunc(userrole))
                            {
                                ReturnObj = new { status = "Success", result = "User successfully added to roles." };
                            }
                            else
                            {
                                throw new Exception("Fail to authenticated selected users-list. Please re-enter your values and try again.");
                            }
                        }
                        else
                        {
                            throw new Exception("Fail to authenticated selected users-list. Please re-enter your values and try again.");
                        }
                    }
                }
            }   
            catch (Exception ex)
            {
                ReturnObj = new { status = "Fail", result = "An error occurred. Error Message: " + ex.Message };
            }

            return json = JsonConvert.SerializeObject(ReturnObj, Formatting.Indented); ;
        }

        #region DualList-Box
        public class DualListClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public void GetDualListData(MasterRole _Role)
        {
            try
            {
                List<int> RoleUserId = new UsersRoleDAL().ListUserRole().Where(x => x.RoleID == _Role.RoleID).Select(y=>y.UserID).ToList();
                var SelectedItems = new List<DualListClass>();

                for (int i = 0; i < RoleUserId.Count(); i++)
                {
                    SelectedItems.Add(
                        new DualListClass
                        {
                            Id = RoleUserId[i],
                            Name = new UsersDAL().GetUsers().Where(x => x.UserID == RoleUserId[i]).Select(y => y.UserName).FirstOrDefault()
                        });
                }

                Session["SelectedDualList"] = SelectedItems;

                List<MasterUser> lst = new UsersDAL().GetUsers();
                var items = new List<DualListClass>();
                
                for (int i = 0; i < lst.Count(); i++)
                {
                    items.Add(
                        new DualListClass
                        {
                            Id = lst[i].UserID,
                            Name = lst[i].UserName
                        });
                }

                Session["DualList"] = items;
                BindDualList();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void BindDualList()
        {
            dlbGroupA.DataSource = (List<DualListClass>)Session["DualList"];
            dlbGroupA.BindData();

            List<DualListClass> SelectedItems = (List<DualListClass>)Session["SelectedDualList"];
            if (SelectedItems.Count > 0)
            {
                List<string> ids = SelectedItems.Select(x => Convert.ToString(x.Id)).ToList();
                dlbGroupA.SetSelectedValues(ids);
            }
        }
        #endregion

        #region backend process
        protected void CheckRolesData()
        {
            try
            {
                if (!Roles.RoleExists("Admin"))
                {
                    Roles.CreateRole("Admin");
                }
                if (!Roles.RoleExists("Preparer"))
                {
                    Roles.CreateRole("Preparer");
                }
                if (!Roles.RoleExists("Reviewer"))
                {
                    Roles.CreateRole("Reviewer");
                }
                if (!Roles.RoleExists("Approver"))
                {
                    Roles.CreateRole("Approver");
                }
                if (!Roles.RoleExists("Viewer"))
                {
                    Roles.CreateRole("Viewer");
                }

                LocalDBCheck();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void LocalDBCheck()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BPSecurity"].ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Roles", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                Guid roleid = (Guid)dt.Rows[r]["RoleId"];
                string rolename = dt.Rows[r]["RoleName"].ToString();

                if (!new UsersRoleDAL().GetRoles().Select(x => x.RoleName).Contains(rolename))
                {
                    MasterRole roleObj = new MasterRole();
                    roleObj.RUID = roleid;
                    roleObj.RoleName = rolename;
                    roleObj.RoleStatus = "A";

                    if (rolename == "Admin")
                    {
                        roleObj.Description = "Control the setting of the system.";
                    }
                    if (rolename == "Preparer")
                    {
                        roleObj.Description = "Prepare the budget.";
                    }
                    if (rolename == "Reviewer")
                    {
                        roleObj.Description = "Review the prepared budget.";
                    }
                    if (rolename == "Approver")
                    {
                        roleObj.Description = "Approved the budget after prepared or reviewed.";
                    }
                    if (rolename == "Viewer")
                    {
                        roleObj.Description = "Display the result.";
                    }
                    //add role. backend process - start
                    if (!new UsersRoleDAL().InsertMasterRole(roleObj))
                    {
                        ((SiteMaster)this.Master).ShowMessage("Failure", "An error occurred while saving User Role");
                    }
                    //add role. backend process - end
                }
            }
        }
        #endregion

        private void ClearPageData()
        {
            try
            {
                txtDesc.Text = string.Empty;

                foreach (GridViewRow gvr in gvRoles.Rows)
                    gvr.Style["background-color"] = "";
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvRoles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                ClearPageData();

                GridViewRow selectedRow = gvRoles.Rows[Convert.ToInt32(e.CommandArgument)];
                selectedRow.Style["background-color"] = "skyblue";
                
                Label lblRolename = (Label)selectedRow.Cells[1].FindControl("lblRolename");

                MasterRole objMasterRole = new MasterRole();
                objMasterRole.RoleID = Convert.ToInt32(gvRoles.DataKeys[selectedRow.RowIndex]["RoleID"]);
                objMasterRole.RoleName = lblRolename.Text;
                objMasterRole.Description = selectedRow.Cells[2].Text;
                objMasterRole.RoleStatus = new UsersRoleDAL().GetRoles().Where(x => x.RoleID == objMasterRole.RoleID).Select(y => y.RoleStatus).FirstOrDefault();

                Session["SelectedMasterRole"] = objMasterRole;

                ddlRole.SelectedValue = Convert.ToString(objMasterRole.RoleID);
                txtDesc.Text = objMasterRole.Description;
                ddlStatus.SelectedIndex = -1;
                ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objMasterRole.RoleStatus))).Selected = true;
                GetDualListData(objMasterRole);

                EditForm.Visible = true;
            }
        }

        protected void gvRoles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<MasterRole> data = (List<MasterRole>)Session["MasterRoleData"];
                var span = ((HtmlGenericControl)e.Row.Cells[3].FindControl("CustomStatus"));
                var countSpan = ((HtmlGenericControl)e.Row.Cells[0].FindControl("count"));

                int RoleId = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "RoleID"));
                string RoleStatus = data.Where(x => x.RoleID == RoleId).Select(y => y.RoleStatus).FirstOrDefault();

                if (RoleStatus == "A")
                {
                    //span.Attributes["class"] = "label label-success";
                    //span.InnerHtml = "<i class=\"fa fa-flag green bigger-150 tooltip-success\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Active\"></i>";
                    span.InnerHtml = "<span class=\"label label-sm label-success arrowed-in arrowed-in-right tooltip-success\" "+
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Active Status. All operation has been enabled.\">Active</span>";
                }
                else if (RoleStatus == "D")
                {
                    //span.InnerHtml = "<i class=\"fa fa-flag red bigger-150 tooltip-error\" data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive\"></i>";
                    span.InnerHtml = "<span class=\"label label-sm label-danger arrowed-in arrowed-in-right tooltip-error\" "+
                        "data-rel=\"tooltip\" data-placement=\"right\" title=\"Inactive Status. All operation has been disabled.\">Inactive</span>";
                }

                string countStr = Convert.ToString(new UsersRoleDAL().ListUserRole().Where(x => x.RoleID == RoleId).Select(y => y.UserID).Count());
                countSpan.InnerHtml = countStr;
                countSpan.Attributes["title"] = countStr + " users";
            }
        }
    }
}