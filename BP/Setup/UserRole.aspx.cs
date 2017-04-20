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

namespace BP.Setup
{
    public partial class UserRole : PageHelper
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetData();
                GetDualList();
                CheckRolesData();
                Session["MasterRolePageMode"] = Helper.PageMode.New;

                List<string> getselectedvalue = dlbGroupA.GetSelectedValues();

            }
        }

        private void GetData()
        {
            try
            {
                List<MasterRole> data = new UsersRoleDAL().GetRoles();

                Session["MasterRoleData"] = data;
                BindGrid();
                //LoadDropDown();
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        private void BindGrid()
        {
        }

        public class DualListClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public void GetDualList()
        {
            try
            {
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
            dlbGroupA.SetSelectedValues(new List<string> { "admin" });
        }

        protected void CheckRolesData()
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

        protected void LocalDBCheck()
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["BPSecurity"].ConnectionString);

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Roles", conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int r = 0; r < dt.Rows.Count; r++)
            {
                string roleid = dt.Rows[r]["RoleId"].ToString();
                string rolename = dt.Rows[r]["RoleName"].ToString();

                if (!new UsersRoleDAL().GetRoles().Select(x => x.RoleName).Contains(rolename))
                { 
                    
                }
            }
        }
    }
}