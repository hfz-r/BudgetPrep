<%@ Page Title="User Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserSetup.aspx.cs" Inherits="BP.Setup.UserSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%--<%: Styles.Render("~/styles/Form-Elements_Styles") %>
    <%: Styles.Render("~/styles/Form-Wizard_Styles") %>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
	    <li class="">Setup</li>
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

    <!-- wizard-form -->
    <div id="form_Wiz" runat="server" visible="false">
        <div id="widget-container-wizform">
            <div class="col-xs-12 widget-container-col" id="wizard-form">
                <div class="widget-box" id="widget-box-wizform">
                    <div class="widget-header">
                        <h5 id="widget_title" class="widget-title" runat="server">User Setup - New</h5>

				        <div class="widget-toolbar">
					        <a href="#" data-action="fullscreen" class="orange2">
						        <i class="ace-icon fa fa-expand"></i>
					        </a>

					        <a href="#" data-action="reload">
						        <i class="ace-icon fa fa-refresh"></i>
					        </a>

					        <a href="#" data-action="collapse">
						        <i class="ace-icon fa fa-chevron-up"></i>
					        </a>

					        <a href="#" data-action="close">
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

                                        <!--form used for jquery validation only-->
                                        <form class="form-horizontal" id="validation-form" method="get">

                                            <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="username">Username:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="text" name="username" id="username" class="col-xs-12 col-sm-6" runat="server" onkeypress="return NoSpaceKey(event);"/>
									                </div>
								                </div>
							                </div>
                                            <div class="space-2"></div>
                                            <div id="pwdDiv" class="form-group" runat="server">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password">Password:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="password" name="password" id="password" class="col-xs-12 col-sm-4" runat="server"/>
									                </div>
								                </div>
							                </div>
							                <div class="space-2"></div>
							                <div id="pwd2Div" class="form-group" runat="server">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password2">Confirm Password:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="password" name="password2" id="password2" class="col-xs-12 col-sm-4" runat="server"/>
									                </div>
								                </div>
							                </div>
                                            <div class="space-2"></div>
							                <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email">Email Address:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="email" name="email" id="email" class="col-xs-12 col-sm-6" runat="server"/>
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
                                            <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="role">Role:</label>
                                                        
                                                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <asp:DropDownList ID="role" name="role" runat="server" CssClass="col-xs-12 col-sm-4">
										                </asp:DropDownList>
									                </div>
								                </div>
									        </div>
                                            <div class="space-2"></div>
                                            <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="status">Status:</label>
                                                        
                                                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <asp:DropDownList ID="status" name="status" runat="server" CssClass="col-xs-12 col-sm-4">
										                </asp:DropDownList>
									                </div>
								                </div>
									        </div>

						                </form>

					                </div>

					                <div class="step-pane" data-step="2">
						                <h4 class="lighter block green">Enter the personal information</h4>

                                        <!--form used for jquery validation only-->
                                        <form class="form-horizontal" id="validation-form2" method="get">
                                        
                                            <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="fullname">Fullname:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="text" name="fullname" id="fullname" class="col-xs-12 col-sm-6" runat="server"/>
									                </div>
								                </div>
							                </div>
                                            <div class="space-2"></div>
                                            <div class="form-group">
								                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="icno">IC:</label>

								                <div class="col-xs-12 col-sm-9">
									                <div class="clearfix">
										                <input type="text" name="icno" id="icno" class="col-xs-12 col-sm-6" runat="server"/>
									                </div>
								                </div>
							                </div>
                                            <div class="space-2"></div>
                                            <div class="hr hr-dotted"></div>
                                            <div class="space-2"></div>
                                            <div class="form-group">
		                                        <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="dept">Department:</label>

		                                        <div class="col-xs-12 col-sm-9">
			                                        <div class="clearfix">
				                                        <input type="text" name="dept" id="dept" class="col-xs-12 col-sm-4" runat="server"/>
			                                        </div>
		                                        </div>
	                                        </div>
                                            <div class="space-2"></div>
                                            <div class="form-group">
		                                        <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="post">Position:</label>

		                                        <div class="col-xs-12 col-sm-9">
			                                        <div class="clearfix">
				                                        <input type="text" name="post" id="post" class="col-xs-12 col-sm-4" runat="server"/>
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
													    <input type="tel" id="phone" name="phone" runat="server"/>
												    </div>
											    </div>
										    </div>
                                            <div class="space-8"></div>
										    <div class="form-group">
											    <div class="col-xs-12 col-sm-4 col-sm-offset-3">
												    <label>
													    <input name="agree" id="agree" type="checkbox" class="ace" runat="server" />
													    <span class="lbl"> I accept the policy</span>
												    </label>
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
		                </div><!-- /.widget-main -->
	                </div><!-- /.widget-body -->
                </div>
            </div>
        </div>
    </div>

    <!--widget-->
    <div id="form_List" runat="server">
        <div id="widget-container">
            <div class="col-xs-12 widget-container-col" id="widget-container-col-1">
		        <div class="widget-box" id="widget-box-1">
			        <div class="widget-header">
				        <h5 class="widget-title">Users Setup - List</h5>

				        <div class="widget-toolbar">
					        <div class="widget-menu">
						        <a href="#" data-action="settings" data-toggle="dropdown">
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

					        <a href="#" data-action="fullscreen" class="orange2">
						        <i class="ace-icon fa fa-expand"></i>
					        </a>

					        <a href="#" data-action="reload">
						        <i class="ace-icon fa fa-refresh"></i>
					        </a>

					        <a href="#" data-action="collapse">
						        <i class="ace-icon fa fa-chevron-up"></i>
					        </a>

					        <%--<a href="#" data-action="close">
						        <i class="ace-icon fa fa-times"></i>
					        </a>--%>
				        </div>
			        </div>

			        <div id="UserListWidget" class="widget-body">
				        <div class="widget-main">
                            <div class="clearfix">
                                <div class="pull-right tableTools-container"></div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" 
                                        DataKeyNames="UserID" OnRowCommand="gvUsers_RowCommand" OnRowDataBound="gvUsers_RowDataBound" OnPreRender="gvUsers_PreRender">
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
                                                        <asp:LinkButton ID="btnEditRow" runat="server" PostBackUrl="~/Setup/UserSetup.aspx" OnClientClick="ShowWizForm();" 
                                                            CommandName="EditRow" CommandArgument='<%# Container.DataItemIndex %>' 
                                                            CssClass="btn btn-white btn-minier btn-bold">
												            <i class="ace-icon glyphicon glyphicon-edit blue"></i>
												            Edit
                                                        </asp:LinkButton>
													</div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                <button id="UpdatePanel2Btn" runat="server" style="visibility:hidden;"></button>
                            </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="UpdatePanel2Btn" />
                                </Triggers>
                            </asp:UpdatePanel>
				        </div>
			        </div>
		        </div>
	        </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">
    
    <%--<%: Scripts.Render("~/scripts/Form-Elements_Scripts") %>--%>
    <script type="text/javascript">

        jQuery(function ($) {

            // widget box drag & drop example
            $('.widget-container-col').sortable({
                connectWith: '.widget-container-col',
                items: '> .widget-box',
                handle: ace.vars['touch'] ? '.widget-title' : false,
                cancel: '.fullscreen',
                opacity: 0.8,
                revert: true,
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

            (function () {
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
                    
                    //move to step 1
                    $('[data-step=1]').trigger("click");
                });

                //wizard form onclosed
                $('#widget-box-wizform').on('closed.ace.widget', function (event, info) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');
                    $('form').each(function () { this.reset() });

                    $("#MainContent_gvUsers tr").each(function () {
                        $(this).css("background-color", "");
                    });
                });

                //btn add new user
                $('#MainContent_btnAdd').on('click', function (e) {
                    spinnerInit();
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-state');
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

                    window.location.href = "<%=Page.ResolveUrl("~/Setup/UserSetup.aspx")%>";
                });

                //disable selection
                $('#validation-form, #validation-form2, #UserListWidget, .widget-toolbar, #<%=prev.ClientID%>, #<%=next.ClientID%>')
                    .bind('mousedown.ui-disableSelection selectstart.ui-disableSelection', function (event) {
                    event.stopImmediatePropagation();
                });

            })();
        });

    </script>

    <%--<%: Scripts.Render("~/scripts/Form-Wizard_Scripts") %>--%>
    <script type="text/javascript">

        function dummyFunc() { };

        function ShowWizForm() {
            spinnerInit();
            ace.data.remove('demo', 'widget-state');
            ace.data.remove('demo', 'widget-order');
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

                                window.location.href = "<%=Page.ResolveUrl("~/Setup/UserSetup.aspx")%>";
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

        jQuery(function ($) {

            var $validation = true;
            $('#fuelux-wizard-container').ace_wizard({
                //step: 2 //optional argument. wizard will jump to step "2" at first
                //buttons: '.wizard-actions:eq(0)'
            }).on('actionclicked.fu.wizard', function (e, info) {
                if (info.step == 1 && $validation) {
                    if (!$('#validation-form').valid()) e.preventDefault();
                }
                if (info.step == 2 && $validation) {
                    if (!$('#validation-form2').valid()) e.preventDefault();
                }
            }).on('finished.fu.wizard', function (e) {
                fncUpdatePanel();
            }).on('stepclick.fu.wizard', function (e) {
                //e.preventDefault();//this will prevent clicking and selecting steps
            });

            $.mask.definitions['~'] = '[+-]';
            $('#<%=phone.ClientID%>').mask('(999) 999-9999');
            $('#<%=icno.ClientID%>').mask('999999-99-9999');

            jQuery.validator.addMethod("phone", function (value, element) {
                return this.optional(element) || /^\(\d{3}\) \d{3}\-\d{4}( x\d{1,6})?$/.test(value);
            }, "Enter a valid phone number.");

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
                        required: true
                    },
                    'ctl00$MainContent$phone': {
                        required: true,
                        phone: 'required'
                    },
                    'ctl00$MainContent$agree': {
                        required: true
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
                    'ctl00$MainContent$fullname': "Please specify a fullname.",
                    'ctl00$MainContent$agree': "Please accept our policy."
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

            $(document).one('ajaxloadstart.page', function (e) {
                //in ajax mode, remove remaining elements before leaving page
                $.gritter.removeAll();
                $('.modal').modal('hide');
            });

            $('[data-rel=tooltip]').tooltip();
        });

    </script>

    <script type="text/javascript">

        //table script
        jQuery(function ($) {

            //initiate dataTables plugin
            var myTable = $('#<%=gvUsers.ClientID%>').DataTable({
                bAutoWidth: false,
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

        })
    </script>

</asp:Content>
