using BP.Classes;
using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BP.Setup
{
    public partial class SegmentSetup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.GetData();
                Session["SegmentPageMode"] = Helper.PageMode.New;
                this.LoadDropDown();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ChangePageMode(Helper.PageMode.New);
            //ClearPageData();
            dvSegment.Visible = true;
            //Response.Redirect(Request.RawUrl);
        }

        protected void gvSegmentSetup_PreRender(object sender, EventArgs e)
        {
            gvSegmentSetup.UseAccessibleHeader = true;
            gvSegmentSetup.HeaderRow.TableSection = TableRowSection.TableHeader;
            gvSegmentSetup.FooterRow.TableSection = TableRowSection.TableFooter;
        }

        protected void gvSegmentSetup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditRow")
                {
                    //ClearPageData();
                    GridViewRow selectedRow = gvSegmentSetup.Rows[Convert.ToInt32(e.CommandArgument)];
                    selectedRow.Style["background-color"] = "gold";

                    Segment objSegment = new Segment();
                    objSegment.SegmentID = Convert.ToInt32(gvSegmentSetup.DataKeys[selectedRow.RowIndex]["SegmentID"]);
                    objSegment.SegmentName = selectedRow.Cells[0].Text;
                    objSegment.ShapeFormat = selectedRow.Cells[1].Text;
                    objSegment.SegmentOrder = Convert.ToInt32(selectedRow.Cells[2].Text);
                    objSegment.Status = selectedRow.Cells[3].Text;

                    Session["SelectedSegment"] = objSegment;

                    txtSegmentName.Value = objSegment.SegmentName;
                    txtShapeFormat.Value = objSegment.ShapeFormat;
                    txtSegmentOrder.Value = objSegment.SegmentOrder.ToString();
                    //tbSegName.Text = objSegment.SegmentName;
                    //tbSegFormat.Attributes.Add("value", objSegment.ShapeFormat);
                    //tbSegOrder.Text = objSegment.SegmentOrder.ToString();
                    ddlStatus.SelectedIndex = -1;
                    ddlStatus.Items.FindByValue(new Helper().GetItemStatusEnumName(Convert.ToChar(objSegment.Status))).Selected = true;

                    ChangePageMode(Helper.PageMode.Edit);
                    dvSegment.Visible = true;
                }

                if (e.CommandName == "EditDetails")
                {
                    GridViewRow selectedRow = gvSegmentSetup.Rows[Convert.ToInt32(e.CommandArgument)];

                    Segment objSegment = new Segment();
                    objSegment.SegmentID = Convert.ToInt32(gvSegmentSetup.DataKeys[selectedRow.RowIndex]["SegmentID"]);
                    objSegment.SegmentName = selectedRow.Cells[0].Text;
                    objSegment.ShapeFormat = selectedRow.Cells[1].Text;
                    objSegment.SegmentOrder = Convert.ToInt32(selectedRow.Cells[2].Text);
                    objSegment.Status = selectedRow.Cells[3].Text;

                    Session["SelectedSegment"] = objSegment;

                    Response.Redirect("~/SegmentDetails.aspx");
                }
            }
            catch (Exception ex)
            {
                ((SiteMaster)this.Master).ShowMessage("Error", "An error occurred", ex, true);
            }
        }

        protected void gvSegmentSetup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<Segment> data = (List<Segment>)Session["SegmentData"];
                var Status = ((System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.Cells[3].FindControl("Status"));

                int SegmentID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "SegmentID"));
                string SegmentStatus = data.Where(x => x.SegmentID == SegmentID).Select(y => y.Status).FirstOrDefault();
                if (SegmentStatus == "A")
                {
                    Status.InnerHtml = "<span class=\"label label-success arrowed-in arrowed-in-right\">Active</span>";
                }
                else if (SegmentStatus == "D")
                {
                    Status.InnerHtml = "<span class=\"label label-inverse arrowed\">Inactive</span>";
                }
                else
                {
                    Status.InnerHtml = "<span class=\"label label-warning\">Unknown</span>";
                }
            }
        }

        private void GetData()
        {
            try
            {
                List<Segment> data = new SegmentDAL().GetSegments();

                Session["SegmentData"] = data.OrderBy(x => x.SegmentOrder).ThenBy(x => x.SegmentName).ToList();
                this.BindGrid();
                this.LoadDropDown();
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
                    //username.Attributes.Remove("readonly");
                    //question.Attributes.Remove("readonly");
                    //answer.Attributes.Remove("readonly");
                    //widget_title.InnerText = "New-User Registration Info";
                    //pwdDiv.Visible = true;
                    //pwd2Div.Visible = true;
                    break;
                case Helper.PageMode.Edit:
                    //username.Attributes.Add("readonly", "readonly");
                    //question.Attributes.Add("readonly", "readonly");
                    //answer.Attributes.Add("readonly", "readonly");
                    //widget_title.InnerText = "Edit User Registration Info";
                    //pwdDiv.Visible = false;
                    //pwd2Div.Visible = false;
                    break;
            }
            Session["UsersPageMode"] = pagemode;
        }

        private void BindGrid()
        {
            try
            {
                //((SiteMaster)this.Master).ChangeLanguage();

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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
        public static object getSegmentsList()
        {
            List<Segment> data = new SegmentDAL().GetSegments();
            data.OrderBy(x => x.SegmentOrder).ThenBy(x => x.SegmentName).ToList();


            //var json = JsonConvert.SerializeObject(data);

            var sb = new StringBuilder();
            sb.Append(@"{" + "\"sEcho\": " + 1 + ",");
            sb.Append("\"recordsTotal\": " + 1 + ",");
            sb.Append("\"recordsFiltered\": " + 1 + ",");
            sb.Append("\"iTotalRecords\": " + 1 + ",");
            sb.Append("\"iTotalDisplayRecords\": " + 1 + ",");
            sb.Append("\"aaData\": [[");
            sb.Append("\"" + "Dasar" + "\",");
            sb.Append("\"" + 2 + "\",");
            sb.Append("\"" + "???" + "\",");
            sb.Append("\"" + "A" + "\"");
            sb.Append("]]}");

            //   var test = "{'\sEcho':'1','iTotalRecords':97,'iTotalDisplayRecords':3,'aaData':[['SegmentName':'Dasar','SegmentOrder':2,'ShapeFormat':'???','Status':'A']]}";

            return sb.ToString();
        }
    }
}