<%@ Page Title="User Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserSetup.aspx.cs" Inherits="BP.Setup.UserSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        .ext {
            position: relative;
            display: table;
            overflow-y: auto;
            overflow-x: auto;
            width: auto;
            min-width: 800px;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
	    <li class=""><a href="#">Setup</a></li>
        <li class="active">User Setup</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
     <div class="page-header">
		<h1>
			User Setup
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				create &amp; manage users 
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
       
      <asp:UpdatePanel ID="UpdatePanelWizardForm" runat="server" UpdateMode="Conditional">
          <ContentTemplate>
              <!-- wizard-form -->
              <div id="form_Wiz" runat="server" visible="false">
                  <div id="widget-container-wizform">
                      <div class="col-xs-12 widget-container-col" id="wizard-form">
                          <div class="widget-box" id="widget-box-wizform">
                              <div class="widget-header">
                                  <h5 id="widget_title" class="widget-title" runat="server">User Setup - New</h5>

                                  <div class="widget-toolbar">
                                      <a href="#" data-action="fullscreen" class="orange2 tooltip-info" data-rel="tooltip" data-placement="top" title="Fullscreen">
                                          <i class="ace-icon fa fa-expand"></i>
                                      </a>

                                      <a href="#" data-action="reload" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Reload">
                                          <i class="ace-icon fa fa-refresh"></i>
                                      </a>

                                      <a href="#" data-action="collapse" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Collapse">
                                          <i class="ace-icon fa fa-chevron-up"></i>
                                      </a>

                                      <a href="#" data-action="close" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Close">
                                          <i class="ace-icon fa fa-times"></i>
                                      </a>
                                  </div>
                              </div>

                              <div class="widget-body">
                                  <div class="widget-main">
                                      <div id="fuelux-wizard-container">
                                          <div class="steps-container">
                                              <ul class="steps">
                                                  <li data-step="1" class="active">
                                                      <span class="step">1</span>
                                                      <span class="title">Accounts Information</span>
                                                  </li>

                                                  <li data-step="2">
                                                      <span class="step">2</span>
                                                      <span class="title">Personal Information</span>
                                                  </li>

                                                  <li data-step="3">
                                                      <span class="step">3</span>
                                                      <span class="title">Complete</span>
                                                  </li>
                                              </ul>
                                          </div>

                                          <hr />

                                          <div class="step-content pos-rel">

                                              <div class="step-pane active" data-step="1">
                                                  <h4 class="lighter block green">Enter the accounts information</h4>
                                                  <br />
                                                  <!--form used for jquery validation only-->
                                                  <form class="form-horizontal" id="validation-form" method="get">

                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="username">Username:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <input type="text" name="username" id="username" class="col-xs-12 col-sm-6" runat="server" onkeypress="return NoSpaceKey(event);" />
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div id="pwdDiv" class="form-group" runat="server">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password">Password:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <input type="password" name="password" id="password" class="col-xs-12 col-sm-4" runat="server" />
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div id="pwd2Div" class="form-group" runat="server">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password2">Confirm Password:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <input type="password" name="password2" id="password2" class="col-xs-12 col-sm-4" runat="server" />
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email">Email Address:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <input type="email" name="email" id="email" class="col-xs-12 col-sm-6" runat="server" />
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div class="hr hr-dotted"></div>
                                                      <div class="space-2"></div>
                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="question">Security Question:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <textarea class="input-xlarge" name="question" id="question" runat="server"></textarea>
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div id="SecAns" class="form-group" runat="server">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="answer">Security Answer:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <textarea class="input-xlarge" name="answer" id="answer" runat="server"></textarea>
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div class="hr hr-dotted"></div>
                                                      <div class="space-2"></div>
                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="role">Role:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <asp:DropDownList ID="role" name="role" runat="server" CssClass="col-xs-12 col-sm-4" />
                                                              </div>
                                                          </div>
                                                      </div>
                                                      <div class="space-2"></div>
                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="btnWorkflow">Workflow:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="btn-group">
                                                                  <button id="btnWorkflow" runat="server" class="btn btn-info">
                                                                      <span class="ace-icon fa fa-unlock bigger-110 tooltip-info" data-rel="tooltip" 
                                                                          data-placement="bottom" title="Workflow`s setup.">&nbsp;Workflow</span>
                                                                  </button>

                                                                  <%--<button id="btnWFdropdown" runat="server" data-toggle="dropdown" class="btn btn-info dropdown-toggle">
                                                                      <span class="ace-icon fa fa-caret-down icon-only"></span>
                                                                  </button>

                                                                  <ul class="dropdown-menu dropdown-inverse">
                                                                      <li>
                                                                          <asp:LinkButton 
                                                                              ID="disableWorkflow" 
                                                                              runat="server" 
                                                                              Text="Disable Workflow" 
                                                                              OnClientClick="return DisableWorkflow(event);" />
                                                                      </li>
                                                                  </ul>--%>
                                                              </div>
                                                              <!-- /.btn-group -->
                                                          </div>
                                                      </div>
                                                      <div class="hr hr-dotted"></div>
                                                      <div class="space-2"></div>
                                                      <div class="form-group">
                                                          <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="status">Status:</label>

                                                          <div class="col-xs-12 col-sm-9">
                                                              <div class="clearfix">
                                                                  <asp:DropDownList ID="status" name="status" runat="server" CssClass="col-xs-12 col-sm-4" />
                                                              </div>
                                                          </div>
                                                      </div>

                                                  </form>

                                              </div>

                                              <div class="step-pane" data-step="2">
                                                  <h4 class="lighter block green">Enter the personal information</h4>
                                                  <br />
                                                  <!--form used for jquery validation only-->
                                                  <form class="form-horizontal" id="validation-form2" method="get">
                                                      <div class="row">
                                                          <div class="col-xs-6">
                                                              
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="fullname">Fullname:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="fullname" id="fullname" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="icno">IC:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="icno" id="icno" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="title">Title:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="title" id="title" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="phone">Phone Number:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="input-group">
                                                                          <span class="input-group-addon">
                                                                              <i class="ace-icon fa fa-phone"></i>
                                                                          </span>
                                                                          <input type="tel" id="phone" name="phone" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="fax">Fax Number:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="input-group">
                                                                          <span class="input-group-addon">
                                                                              <i class="ace-icon fa fa-fax"></i>
                                                                          </span>
                                                                          <input type="tel" id="fax" name="fax" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-8"></div>

                                                          </div>

                                                          <div class="col-xs-6">
                                                              
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="designation">Designation:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="designation" id="designation" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="dept">Department:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="dept" id="dept" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="grade">Position Grade:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <input type="text" name="grade" id="grade" class="col-xs-12 col-sm-9" runat="server" />
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="period">
                                                                      Period of Service:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="input-group">
                                                                          <input type="text" class="input-sm" id="period" name="period" runat="server"/> &nbsp;&nbsp;months
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-2"></div>
                                                              <div class="form-group">
                                                                  <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="offaddress">Office Address:</label>

                                                                  <div class="col-xs-12 col-sm-9">
                                                                      <div class="clearfix">
                                                                          <textarea id="offaddress" name="offaddress" class="autosize-transition form-control"
                                                                              runat="server"></textarea>
                                                                      </div>
                                                                  </div>
                                                              </div>
                                                              <div class="space-8"></div>

                                                          </div>
                                                      </div>
                                                  </form>
                                              </div>

                                              <div class="step-pane" data-step="3">
                                                  <div class="center">
                                                      <h3 class="green">Complete!</h3>
                                                      Your information is ready to save! Click finish to continue!
                                                  </div>
                                              </div>

                                          </div>
                                      </div>
                                      <hr />
                                      <div class="wizard-actions">

                                          <button id="prev" class="btn btn-prev" onclick="dummyFunc(); return false" runat="server">
                                              <i class="ace-icon fa fa-arrow-left"></i>
                                              Prev
                                          </button>

                                          <button id="next" class="btn btn-success btn-next" data-last="Finish" onclick="dummyFunc(); return false" runat="server">
                                              Next
						                    <i class="ace-icon fa fa-arrow-right icon-on-right"></i>
                                          </button>

                                      </div>
                                  </div>
                                  <!-- /.widget-main -->
                              </div>
                              <!-- /.widget-body -->
                          </div>
                      </div>
                  </div>
              </div>
          </ContentTemplate>
          <Triggers>
              <asp:PostBackTrigger ControlID="btnSaveWorkflow" />
              <asp:PostBackTrigger ControlID="btnCancelWorkflow" />
          </Triggers>
    </asp:UpdatePanel>

    <!--widget-->
    <div id="form_List" runat="server">
        <div id="widget-container">
            <div class="col-xs-12 widget-container-col" id="widget-container-col-1">
		        <div class="widget-box" id="widget-box-1">
			        <div class="widget-header">
				        <h5 class="widget-title">Users Setup - List</h5>

				        <div class="widget-toolbar">
					        <div class="widget-menu">
						        <a href="#" data-action="settings" data-toggle="dropdown" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Settings">
							        <i class="ace-icon fa fa-cogs"></i>
						        </a>

						        <ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
							        <li> 
                                        <asp:LinkButton ID="btnAdd" runat="server" PostBackUrl="~/Setup/UserSetup.aspx" CssClass="blue" OnClick="btnAdd_Click">
                                            <i class="ace-icon fa fa-user-plus"></i>&nbsp;&nbsp;Add User
                                        </asp:LinkButton>
								        <%--<a href="#AddUser" role="button" class="blue" runat="server" OnClick="btnAdd_Click">
                                            <i class="ace-icon fa fa-user-plus"></i>&nbsp;&nbsp;Add User
								        </a>--%>
							        </li>
						        </ul>
					        </div>

					        <a href="#" data-action="fullscreen" class="orange2 tooltip-info" data-rel="tooltip" data-placement="top" title="Fullscreen">
						        <i class="ace-icon fa fa-expand"></i>
					        </a>

					        <a href="#" data-action="reload" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Reload">
						        <i class="ace-icon fa fa-refresh"></i>
					        </a>

					        <a href="#" data-action="collapse" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Collapse">
						        <i class="ace-icon fa fa-chevron-up"></i>
					        </a>

					        <%--<a href="#" data-action="close">
						        <i class="ace-icon fa fa-times"></i>
					        </a>--%>
				        </div>
			        </div>

			        <div class="widget-body">
				        <div class="widget-main">
                            <div class="clearfix">
                                <div class="pull-right tableTools-container"></div>
                            </div>
                            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover"
                                DataKeyNames="UserID" OnRowCommand="gvUsers_RowCommand" OnRowDataBound="gvUsers_RowDataBound" OnRowCreated="gvUsers_RowCreated" 
                                OnPreRender="gvUsers_PreRender">
                                <Columns>
                                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                    <asp:BoundField DataField="UserEmail" HeaderText="Email" />
                                    <asp:BoundField DataField="UserPhoneNo" HeaderText="Phone No" />
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span id="CustomStatus" runat="server"></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div class="hidden-sm hidden-xs btn-group">
                                                <asp:LinkButton ID="btnEditRow" runat="server" OnClientClick="ShowWizForm();" CommandName="EditRow" 
                                                    CommandArgument='<%# Container.DataItemIndex %>' CssClass="btn btn-white btn-minier btn-bold">
                                                    <i class="ace-icon glyphicon glyphicon-edit blue"></i>
                                                    Edit
                                                </asp:LinkButton>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
				        </div>
			        </div>
		        </div>
	        </div>
        </div>
    </div>

    <!-- workflow -->
    <div id="workflow-modal" class="modal gray" tabindex="-1">
		<div class="modal-dialog ext">
			<div class="modal-content" style="width:70%; margin:0 auto;">
				<div class="modal-header">
					<button type="button" class="close" data-dismiss="modal">&times;</button>
					<h4 class="blue bigger">Workflow Setup</h4>
				</div>

				<div class="modal-body">
					<div class="row">
                        <div class="col-sm-12">
                            <div id="accordion" class="accordion-style1 panel-group accordion-style2">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseOne">
                                                <i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                                &nbsp; <i class="pink ace-icon fa fa-briefcase bigger-120"></i>&nbsp;Mengurus
                                            </a>
                                        </h4>
                                    </div>

                                    <div class="panel-collapse collapse" id="collapseOne">
                                        <div class="panel-body">
                                            <div class="widget-box transparent">
                                                <div class="widget-header widget-header-small">
                                                    <h4 class="widget-title smaller">
                                                        <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                                        Mengurus Workflow Setting.
                                                    </h4>

                                                    <div class="widget-toolbar no-border">
                                                        <a href="#" data-action="settings">
                                                            <i class="ace-icon fa fa-cog"></i>
                                                        </a>
                                                        <asp:LinkButton ID="ReloadMG" runat="server" OnClientClick="return ReloadTreeView(event,this);">
                                                            <i class="ace-icon fa fa-refresh"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>

                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:TreeView ID="tvMengurus" runat="server" ShowCheckBoxes="All" ImageSet="Arrows" Font-Size="Large">
                                                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px"
                                                                NodeSpacing="0px" VerticalPadding="0px" />
                                                            <ParentNodeStyle Font-Bold="False" />
                                                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                                        </asp:TreeView>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo">
                                                <i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                                &nbsp;<i class="blue ace-icon fa fa-users bigger-110"></i>&nbsp;Perjawatan
                                            </a>
                                        </h4>
                                    </div>

                                    <div class="panel-collapse collapse" id="collapseTwo">
                                        <div class="panel-body">
                                            <asp:GridView ID="gvPerjawatanWorkFlow" runat="server" AutoGenerateColumns="false" Width="100%"
                                                CssClass="table table-bordered table-striped table-hover" DataKeyNames="GroupPerjawatanCode"
                                                OnPreRender="gvPerjawatanWorkFlow_PreRender">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="GroupPerjawatanCode" HeaderText="Service Code" />
                                                    <asp:BoundField DataField="GroupPerjawatanDesc" HeaderText="Service Description" />
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle collapsed" data-toggle="collapse" data-parent="#accordion" href="#collapseThree">
                                                <i class="ace-icon fa fa-angle-right bigger-110" data-icon-hide="ace-icon fa fa-angle-down" data-icon-show="ace-icon fa fa-angle-right"></i>
                                                &nbsp; <i class="green ace-icon fa fa-laptop bigger-110"></i>&nbsp;Segment Details
                                            </a>
                                        </h4>
                                    </div>

                                    <div class="panel-collapse collapse" id="collapseThree">
                                        <div class="panel-body">
                                            <div class="widget-box transparent">
                                                <div class="widget-header widget-header-small">
                                                    <h4 class="widget-title smaller">
                                                        <i class="ace-icon fa fa-check-square-o bigger-110"></i>
                                                        Segment Details Workflow Setting.
                                                    </h4>

                                                    <div class="widget-toolbar no-border">
														<a href="#" data-action="settings">
															<i class="ace-icon fa fa-cog"></i>
														</a>
                                                        <asp:LinkButton ID="ReloadSD" runat="server" OnClientClick="return ReloadTreeView(event,this);">
                                                            <i class="ace-icon fa fa-refresh"></i>
                                                        </asp:LinkButton>
													</div>
                                                </div>

                                                <div class="widget-body">
                                                    <div class="widget-main">
                                                        <asp:TreeView ID="tvSegmentDetails" runat="server" ShowCheckBoxes="All" ImageSet="Arrows" Font-Size="Large">
                                                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                            <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="5px"
                                                                NodeSpacing="0px" VerticalPadding="0px" />
                                                            <ParentNodeStyle Font-Bold="False" />
                                                            <SelectedNodeStyle Font-Underline="True" ForeColor="#5555DD" HorizontalPadding="0px" VerticalPadding="0px" />
                                                        </asp:TreeView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
					</div>
				</div>

				<div class="modal-footer">
					<button id="btnCancelWorkflow" runat="server" class="btn btn-sm" data-dismiss="modal" onserverclick="btnCancelWorkflow_OnClick">
						<i class="ace-icon fa fa-times"></i>
						Cancel
					</button>
                    <button id="btnSaveWorkflow" runat="server" class="btn btn-sm btn-primary" data-dismiss="modal" onserverclick="btnSaveWorkflow_OnClick">
                        <i class="ace-icon fa fa-check"></i>
                        Save
                    </button>
				</div>
			</div>
		</div>
	</div><!-- PAGE CONTENT ENDS -->

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">
    
    <script type="text/javascript">

        function InitScript()
        {
            // widget box drag & drop example
            $('.widget-container-col').sortable({
                connectWith: '.widget-container-col',
                items: '> .widget-box',
                handle: ace.vars['touch'] ? '.widget-title' : false,
                cancel: '.fullscreen',
                opacity: 0.8,
                revert: true,
                cancel: '.widget-main, .widget-toolbar',
                forceHelperSize: true,
                placeholder: 'widget-placeholder',
                forcePlaceholderSize: true,
                tolerance: 'pointer',
                start: function (event, ui) {
                    //when an element is moved, it's parent becomes empty with almost zero height.
                    //we set a min-height for it to be large enough so that later we can easily drop elements back onto it
                    ui.item.parent().css({ 'min-height': ui.item.height() })
                    //ui.sender.css({'min-height':ui.item.height() , 'background-color' : '#F5F5F5'})
                },
                update: function (event, ui) {
                    ui.item.parent({ 'min-height': '' })
                    //p.style.removeProperty('background-color');

                    //save widget positions
                    var widget_order = {}
                    $('.widget-container-col').each(function () {
                        var container_id = $(this).attr('id');
                        widget_order[container_id] = []

                        $(this).find('> .widget-box').each(function () {
                            var widget_id = $(this).attr('id');
                            widget_order[container_id].push(widget_id);
                            //now we know each container contains which widgets
                        });
                    });

                    ace.data.set('demo', 'widget-order', widget_order, null, true);
                }
            });

            //when a widget is shown/hidden/closed, we save its state for later retrieval
            $(document).on('shown.ace.widget hidden.ace.widget closed.ace.widget', '.widget-box', function (event) {
                var widgets = ace.data.get('demo', 'widget-state', true);
                if (widgets == null) widgets = {}

                var id = $(this).attr('id');
                widgets[id] = event.type;
                ace.data.set('demo', 'widget-state', widgets, null, true);
            });

            //restore widget order
            var container_list = ace.data.get('demo', 'widget-order', true);
            if (container_list) {
                for (var container_id in container_list) if (container_list.hasOwnProperty(container_id)) {

                    var widgets_inside_container = container_list[container_id];
                    if (widgets_inside_container.length == 0) continue;

                    for (var i = 0; i < widgets_inside_container.length; i++) {
                        var widget = widgets_inside_container[i];
                        $('#' + widget).appendTo('#' + container_id);
                    }
                }
            }

            //restore widget state
            var widgets = ace.data.get('demo', 'widget-state', true);
            if (widgets != null) {
                for (var id in widgets) if (widgets.hasOwnProperty(id)) {
                    var state = widgets[id];
                    var widget = $('#' + id);
                    if
					(
                        (state == 'shown' && widget.hasClass('collapsed'))
                        ||
                        (state == 'hidden' && !widget.hasClass('collapsed'))
                    ) {
                        widget.widget_box('toggleFast');
                    }
                    else if (state == 'closed') {
                        widget.widget_box('closeFast');
                    }
                }
            }

            //wizard form onreloaded
            $('#widget-box-wizform').on('reloaded.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');
                $('form').each(function () { this.reset() });

                ClearWorkflowCheckbox();
                localStorage.clear();

                //move to step 1
                $('[data-step=1]').trigger("click");
            });

            //wizard form onclosed
            $('#widget-box-wizform').on('closed.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');
                $('form').each(function () { this.reset() });

                ClearWorkflowCheckbox()
                localStorage.clear();

                $("#MainContent_gvUsers tr").each(function () {
                    $(this).css("background-color", "");
                });
            });

            //btn add new user
            $('#MainContent_btnAdd').on('click', function (e) {
                spinnerInit();
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-state');

                localStorage.clear();

                $('#MainContent_form_Wiz').show();
            });

            //user lists onreloaded
            $('#widget-box-1').on('reloaded.ace.widget', function (event) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');
                $('form').each(function () { this.reset() });
                $('#MainContent_form_Wiz').hide();

                var myTable = $('#<%=gvUsers.ClientID%>').DataTable();
                myTable.state.clear();

                location.href = "<%=Page.ResolveUrl("~/Setup/UserSetup.aspx")%>";
            });

            //*workflow handler - start*//
            $('#<%=role.ClientID%>').bind('change', function ()
            {
                if ($('#<%=role.ClientID%> :selected').text() == "Admin")
                {
                    $('#<%=btnWorkflow.ClientID%>').prop('disabled', true);
                }
                else
                {
                    $('#<%=btnWorkflow.ClientID%>').prop('disabled', false);
                }
            });
            $('#<%=role.ClientID%>').trigger('change');

            $('[id^=<%=btnWorkflow.ClientID%>]').on('click', function (e) {
                e.preventDefault();
                $('#workflow-modal').modal({
                    backdrop: 'static',
                    keyboard: false
                });
            });

            $('#<%=btnSaveWorkflow.ClientID%>, #<%=btnCancelWorkflow.ClientID%>').on('click', function (e) {
                SaveFormState();
            });
            //*workflow handler - end*//

            //tooltip
            $('[data-rel=tooltip]').tooltip();
            //autosize
            autosize($('textarea[class*=autosize]'));

            //spinner - used by period of service
            $('#<%=period.ClientID%>').ace_spinner({
                value: 0, min: 0, max: 10000, step: 1, touch_spinner: true,
                icon_up: 'ace-icon fa fa-caret-up bigger-110',
                icon_down: 'ace-icon fa fa-caret-down bigger-110'
            });

            //destroy gritter and modal
            $(document).one('ajaxloadstart.page', function (e) {
                //in ajax mode, remove remaining elements before leaving page
                autosize.destroy('textarea[class*=autosize]')
                $.gritter.removeAll();
                $('.modal').modal('hide');
            });
        }

        function SaveFormState()
        {
            localStorage.username = $('#<%=username.ClientID%>').val();
            localStorage.email = $('#<%=email.ClientID%>').val();
            localStorage.question = $('#<%=question.ClientID%>').val();
            localStorage.answer = $('#<%=answer.ClientID%>').val();
            localStorage.role = $('#<%=role.ClientID%>').val();
            localStorage.status = $('#<%=status.ClientID%>').val();
            localStorage.fullname = $('#<%=fullname.ClientID%>').val();
            localStorage.icno = $('#<%=icno.ClientID%>').val();
            localStorage.title = $('#<%=title.ClientID%>').val();
            localStorage.phone = $('#<%=phone.ClientID%>').val();
            localStorage.fax = $('#<%=fax.ClientID%>').val();
            localStorage.designation = $('#<%=designation.ClientID%>').val();
            localStorage.dept = $('#<%=dept.ClientID%>').val();
            localStorage.grade = $('#<%=grade.ClientID%>').val();
            localStorage.period = $('#<%=period.ClientID%>').val();
            localStorage.offaddress = $('#<%=offaddress.ClientID%>').val();
        }

        function LoadFormState()
        {
            if (localStorage.username != null)
                $('#<%=username.ClientID%>').val(localStorage.username);
            if (localStorage.email != null)
                $('#<%=email.ClientID%>').val(localStorage.email);
            if (localStorage.question != null)
                $('#<%=question.ClientID%>').val(localStorage.question);
            if (localStorage.answer != null)
                $('#<%=answer.ClientID%>').val(localStorage.answer);
            if (localStorage.role != null)
                $('#<%=role.ClientID%>').val(localStorage.role);
            if (localStorage.status != null)
                $('#<%=status.ClientID%>').val(localStorage.status);
            if (localStorage.fullname != null)
                $('#<%=fullname.ClientID%>').val(localStorage.fullname);
            if (localStorage.icno != null)
                $('#<%=icno.ClientID%>').val(localStorage.icno);
            if (localStorage.title != null)
                $('#<%=title.ClientID%>').val(localStorage.title);
            if (localStorage.phone != null)
                $('#<%=phone.ClientID%>').val(localStorage.phone);
            if (localStorage.fax != null)
                $('#<%=fax.ClientID%>').val(localStorage.fax);
            if (localStorage.designation != null)
                $('#<%=designation.ClientID%>').val(localStorage.designation);
            if (localStorage.dept != null)
                $('#<%=dept.ClientID%>').val(localStorage.dept);
            if (localStorage.grade != null)
                $('#<%=grade.ClientID%>').val(localStorage.grade);
            if (localStorage.period != null)
                $('#<%=period.ClientID%>').val(localStorage.period);
            if (localStorage.offaddress != null)
                $('#<%=offaddress.ClientID%>').val(localStorage.offaddress);
        }

        function ClearWorkflowCheckbox()
        {
            var gvPerjawatan = $('#<%=gvPerjawatanWorkFlow.ClientID%>');
            gvPerjawatan.find("input[type='checkbox']").prop('checked', false);
        }

        function dummyFunc() { };

        function ShowWizForm() {
            spinnerInit();
            ace.data.remove('demo', 'widget-state');
            ace.data.remove('demo', 'widget-order');

            localStorage.clear();

            $("#MainContent_form_Wiz").show();
        }

        function fncUpdatePanel() {
            spinnerInit();

            var formArray = $('#validation-form,#validation-form2').serializeArray();
            var returnArray = {};
            for (var i = 0; i < formArray.length; i++) {
                returnArray[formArray[i]['name']] = formArray[i]['value'];
            }
            var jsonData = JSON.stringify({ obj: JSON.stringify(returnArray) });

            $.ajax({
                type: "POST",
                url: "UserSetup.aspx/FormValues",
                data: jsonData,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (dt) {
                    $('#spin').data('spinner').stop();
                    $("#spin").hide();

                    bootbox.alert({
                        title: dt.d.pageTitle,
                        message: dt.d.pageBody,
                        callback: function () {
                            if (dt.d.pageTitle == "Success")
                            {
                                //$('#widget-box-wizform').trigger("reload");
                                $('form').each(function () { this.reset() });
                                $('#widget-box-wizform').widget_box('close');

                                location.href = "<%=Page.ResolveUrl("~/Setup/UserSetup.aspx")%>";
                            }
                            else if (dt.d.pageTitle == "Failure")
                            {
                                //move to step 1
                                $('[data-step=1]').trigger("click");
                            }
                        }
                    });
                }
            });
        }

        function ValidationInit()
        {
            $('#fuelux-wizard-container').ace_wizard({
            }).on('actionclicked.fu.wizard', function (e, info)
            {
                if (info.step == 1)
                {
                    if (!$('#validation-form').valid()) e.preventDefault();
                }
                if (info.step == 2)
                {
                    if (!$('#validation-form2').valid()) e.preventDefault();
                }
            }).on('finished.fu.wizard', function (e)
            {
                fncUpdatePanel();
            }).on('stepclick.fu.wizard', function (e) 
            {
                //e.preventDefault();//this will prevent clicking and selecting steps
            });
        
            $.mask.definitions['~'] = '[+-]';
            $('#<%=phone.ClientID%>').mask('(999) 999-9999');
            $('#<%=fax.ClientID%>').mask('(999) 999-9999');
            $('#<%=icno.ClientID%>').mask('999999-99-9999');

            $.validator.addMethod("phone", function (value, element) {
                return this.optional(element) || /^\(\d{3}\) \d{3}\-\d{4}( x\d{1,6})?$/.test(value);
            }, "Enter a valid phone number.");

            $.validator.addMethod("icno", function (value, element) {
                return this.optional(element) || /^\d{6}-\d{2}-\d{4}$/.test(value);
            }, "Enter a valid IC number.");

            $('form').each(function () {
                $(this).validate({
                    errorElement: 'div',
                    errorClass: 'help-block',
                    focusInvalid: false,
                    ignore: "",
                    rules: {
                        'ctl00$MainContent$email': {
                            required: true,
                            email: true
                        },
                        'ctl00$MainContent$password': {
                            required: true,
                            minlength: 5
                        },
                        'ctl00$MainContent$password2': {
                            required: true,
                            minlength: 5,
                            equalTo: $('#<%=password.ClientID%>')
                    },
                        'ctl00$MainContent$username': {
                            required: true
                        },
                        'ctl00$MainContent$question': {
                            required: true
                        },
                        'ctl00$MainContent$answer': {
                            required: true
                        },
                        'ctl00$MainContent$status': {
                            required: true
                        },
                        'ctl00$MainContent$role': {
                            required: true
                        },
                        'ctl00$MainContent$fullname': {
                            required: true,
                        },
                        'ctl00$MainContent$icno': {
                            required: true,
                            icno: 'required'
                        },
                        'ctl00$MainContent$phone': {
                            required: true,
                            phone: 'required'
                        },
                        'ctl00$MainContent$designation': {
                            required: true,
                        }
                    },

                    messages: {
                        'ctl00$MainContent$email': {
                            required: "Please provide a valid email.",
                            email: "Please provide a valid email."
                        },
                        'ctl00$MainContent$password': {
                            required: "Please specify a password.",
                            minlength: "Please specify a secure password."
                        },
                        'ctl00$MainContent$username': "Please specify a username.",
                        'ctl00$MainContent$question': "Please specify a security question.",
                        'ctl00$MainContent$answer': "Please specify a security answer.",
                        'ctl00$MainContent$status': "Please specify a status",
                        'ctl00$MainContent$role': "Please specify a role",
                        'ctl00$MainContent$fullname': "Please specify a fullname",
                        'ctl00$MainContent$icno': "Please specify IC number",
                        'ctl00$MainContent$phone': "Please specify phone number",
                        'ctl00$MainContent$designation': "Please specify a designation"
                    },

                    highlight: function (e) {
                        $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
                    },

                    success: function (e) {
                        $(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
                        $(e).remove();
                    },

                    errorPlacement: function (error, element) {
                        if (element.is('input[type=checkbox]') || element.is('input[type=radio]')) {
                            var controls = element.closest('div[class*="col-"]');
                            if (controls.find(':checkbox,:radio').length > 1) controls.append(error);
                            else error.insertAfter(element.nextAll('.lbl:eq(0)').eq(0));
                        }
                        else if (element.is('.select2')) {
                            error.insertAfter(element.siblings('[class*="select2-container"]:eq(0)'));
                        }
                        else if (element.is('.chosen-select')) {
                            error.insertAfter(element.siblings('[class*="chosen-container"]:eq(0)'));
                        }
                        else error.insertAfter(element.parent());
                    },

                    submitHandler: function (form) {
                    },
                    invalidHandler: function (form) {
                    }
                });
            });
        }

        function LoadDataTable() {
            //initiate dataTables plugin
            var myTable = $('#<%=gvUsers.ClientID%>').DataTable({
                bAutoWidth: false,
                "lengthMenu": [[20, 40, 60, -1], [20, 40, 60, "All"]],
                "aoColumns": [
					  null,
                      null,
                      null,
                      null,
                      null,
                    { "bSortable": false }
                ],
                "aaSorting": [],
                select: {
                    style: 'multi'
                }
            });

            $.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

            new $.fn.dataTable.Buttons(myTable, {
                buttons: [
                  {
                      "extend": "colvis",
                      "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      columns: ':not(:last)'
                  },
                  {
                      "extend": "copyHtml5",
                      "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2, 3, 4]
                      }
                  },
                  {
                      "extend": "csvHtml5",
                      "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2, 3, 4],
                          modifier: {
                              search: 'none'
                          }
                      }
                  },
                  {
                      "extend": "excelHtml5",
                      "text": "<i class='fa fa-file-excel-o bigger-110 green'></i> <span class='hidden'>Export to Excel</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2, 3, 4],
                          modifier: {
                              page: 'current'
                          }
                      }
                  },
                  {
                      "extend": "pdfHtml5",
                      "text": "<i class='fa fa-file-pdf-o bigger-110 red'></i> <span class='hidden'>Export to PDF</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2, 3, 4],
                          modifier: {
                              page: 'current'
                          }
                      }
                  },
                  {
                      "extend": "print",
                      "text": "<i class='fa fa-print bigger-110 grey'></i> <span class='hidden'>Print</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      autoPrint: false,
                      message: 'This print was produced using the Print button for DataTables',
                      exportOptions: {
                          columns: [0, 1, 2, 3, 4]
                      }
                  }
                ]
            });
            myTable.buttons().container().appendTo($('.tableTools-container'));

            //style the message box
            var defaultCopyAction = myTable.button(1).action();
            myTable.button(1).action(function (e, dt, button, config) {
                defaultCopyAction(e, dt, button, config);
                $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
            });

            var defaultColvisAction = myTable.button(0).action();
            myTable.button(0).action(function (e, dt, button, config) {

                defaultColvisAction(e, dt, button, config);

                if ($('.dt-button-collection > .dropdown-menu').length == 0) {
                    $('.dt-button-collection')
                    .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
                    .find('a').attr('href', '#').wrap("<li />")
                }
                $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
            });

            setTimeout(function ()
            {
                $($('.tableTools-container')).find('a.dt-button').each(function () {
                    var div = $(this).find(' > div').first();
                    if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                    else $(this).tooltip({ container: 'body', title: $(this).text() });
                });
            }, 500);

            $(document).on('click', '#<%=gvUsers.ClientID%> .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });

            //Load Workflow Datatables!
            var gvs = [$('#<%=gvPerjawatanWorkFlow.ClientID%>')];
            for (var i = 0; i < 1; i++) {
                LoadWorkflowDatatable(gvs[i]);
            }
        }

        function LoadWorkflowDatatable(gvs)
        {
            var myTable = gvs.DataTable({
                bAutoWidth: false,
                "lengthMenu": [[10, 20, 30, -1], [10, 20, 30, "All"]],
                "aoColumnDefs": [
                    { bSortable: false, aTargets: [0] }
                ],
                "aaSorting": []
            });
        }

        function WorkflowFlag()
        {
            bootbox.alert({
                title: "<span class='red'>Important Note</span>",
                message: "Please be informed that your <b>**Workflow Setup**</b> is <b><span class='red'>'TEMPORY SAVE'</span></b> until you continue to <b><span class='green'>(Complete)</span></b> the <b>**User Setup**</b>",
                backdrop: true
            });
        }

        //*Tree-View - start*//
        function postBackByObject(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    if (nxtSibling.tagName.toLowerCase() == "div") {
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                CheckUncheckParents(src, src.checked);
            }
        }

        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;
                var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                if (isAllSiblingsChecked) {
                    checkUncheckSwitch = true;
                }
                else {
                    checkUncheckSwitch = false;
                }
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;

                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            var k = 0;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (prevChkBox.checked) {
                            //add each selected node one value
                            k = k + 1;
                        }
                    }
                }
            }

            if (k > 0) {
                return true;
            }
            else {
                return false;
            }
        }

        function ReloadTreeView(evt, elem)
        {
            evt.preventDefault();

            var tv;
            var id = $(elem).attr("id");

            if (id == "MainContent_ReloadSD")
            {
                tv = $("div#<%=tvSegmentDetails.ClientID %>");
            }
            else if (id == "MainContent_ReloadMG")
            {
                tv = $("div#<%=tvMengurus.ClientID %>");
            }

            tv.find("table input:checkbox").each(function () {
                var isChecked = $(this).is(":checked");
                if (isChecked) {
                    $(this).removeAttr("checked");
                }
            });
        }
        //*Tree-View - end*//

        $(document).ready(function () {
            InitScript();
            ValidationInit();
            LoadDataTable();
            LoadFormState();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            InitScript();
            ValidationInit();
            LoadDataTable();
        });

    </script>

</asp:Content>
