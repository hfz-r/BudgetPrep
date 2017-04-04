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

    <!-- modal-print -->
    <div id="modal-form" class="modal" tabindex="-1">
	    <div class="modal-dialog">
		    <div class="modal-content">
			    <div class="modal-header">
				    <button type="button" class="close" data-dismiss="modal">&times;</button>
				    <h4 class="blue bigger">Print Preview</h4>
			    </div>

			    <div class="modal-body">
                </div>

                <div class="modal-footer">
				    <button class="btn btn-sm" data-dismiss="modal">
					    <i class="ace-icon fa fa-times"></i>
					    Cancel
				    </button>

				    <button class="btn btn-sm btn-primary" data-dismiss="modal">
					    <i class="ace-icon fa fa-check"></i>
					    Print
				    </button>
			    </div>
            </div>
        </div>
    </div>

    <!-- wizard-form -->
    <div class="invisible" id="widget-container-wizform">
        <div class="col-xs-12 widget-container-col" id="wizard-form">
            <div class="widget-box" id="widget-box-wizform">
                <div class="widget-header">
                    <h6 class="widget-title">New-User Registration Wizard</h6>

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
										            <input type="text" name="username" id="username" class="col-xs-12 col-sm-6" />
									            </div>
								            </div>
							            </div>
                                        <div class="space-2"></div>
                                        <div class="form-group">
								            <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password">Password:</label>

								            <div class="col-xs-12 col-sm-9">
									            <div class="clearfix">
										            <input type="password" name="password" id="password" class="col-xs-12 col-sm-4" />
									            </div>
								            </div>
							            </div>
							            <div class="space-2"></div>
							            <div class="form-group">
								            <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="password2">Confirm Password:</label>

								            <div class="col-xs-12 col-sm-9">
									            <div class="clearfix">
										            <input type="password" name="password2" id="password2" class="col-xs-12 col-sm-4" />
									            </div>
								            </div>
							            </div>
                                        <div class="space-2"></div>
							            <div class="form-group">
								            <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="email">Email Address:</label>

								            <div class="col-xs-12 col-sm-9">
									            <div class="clearfix">
										            <input type="email" name="email" id="email" class="col-xs-12 col-sm-6" />
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
										            <textarea class="input-xlarge" name="question" id="question"></textarea>
									            </div>
								            </div>
							            </div>
							            <div class="space-2"></div>
                                        <div class="form-group">
								            <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="answer">Security Answer:</label>

								            <div class="col-xs-12 col-sm-9">
									            <div class="clearfix">
										            <textarea class="input-xlarge" name="answer" id="answer"></textarea>
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
										            <input type="text" name="fullname" id="fullname" class="col-xs-12 col-sm-6" />
									            </div>
								            </div>
							            </div>
                                        <div class="space-2"></div>
                                        <div class="form-group">
								            <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="ic">IC:</label>

								            <div class="col-xs-12 col-sm-9">
									            <div class="clearfix">
										            <input type="text" name="ic" id="ic" class="col-xs-12 col-sm-6" />
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
				                                    <input type="text" name="dept" id="dept" class="col-xs-12 col-sm-4" />
			                                    </div>
		                                    </div>
	                                    </div>
                                        <div class="space-2"></div>
                                        <div class="form-group">
		                                    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="post">Position:</label>

		                                    <div class="col-xs-12 col-sm-9">
			                                    <div class="clearfix">
				                                    <input type="text" name="post" id="post" class="col-xs-12 col-sm-4" />
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
													<input type="tel" id="phone" name="phone" />
												</div>
											</div>
										</div>
                                        <div class="space-8"></div>
										<div class="form-group">
											<div class="col-xs-12 col-sm-4 col-sm-offset-3">
												<label>
													<input name="agree" id="agree" type="checkbox" class="ace" />
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
				            <button id="prev" class="btn btn-prev" onclick="dummyFunc(); return false">
					            <i class="ace-icon fa fa-arrow-left"></i>
					            Prev
				            </button>

				            <button id="next" class="btn btn-success btn-next" data-last="Finish" onclick="dummyFunc(); return false">
						        Next
						        <i class="ace-icon fa fa-arrow-right icon-on-right"></i>
					        </button>
			            </div>
		            </div><!-- /.widget-main -->
	            </div><!-- /.widget-body -->

            </div>
        </div>
    </div>

    <!--widget-->
    <div class="invisible" id="widget-container">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-1">
		    <div class="widget-box" id="widget-box-1">
			    <div class="widget-header">
				    <h6 class="widget-title">List of Registered Users</h6>

				    <div class="widget-toolbar">
					    <div class="widget-menu">
						    <a href="#" data-action="settings" data-toggle="dropdown">
							    <i class="ace-icon fa fa-bars"></i>
						    </a>

						    <ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
							    <li>
								    <a data-toggle="collapse" href="#wizard-form" role="button" class="blue" aria-expanded="false" aria-controls="wizard-form">
                                        <i class="ace-icon fa fa-user-plus"></i>&nbsp;&nbsp;Add User
								    </a>
							    </li>

							    <li>
								    <a data-toggle="modal" href="#modal-form" role="button" class="blue">
                                        <i class="ace-icon fa fa-print"></i>&nbsp;&nbsp;Print
								    </a>
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

					    <a href="#" data-action="close">
						    <i class="ace-icon fa fa-times"></i>
					    </a>
				    </div>
			    </div>

			    <div class="widget-body">
				    <div class="widget-main">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
					            <%--GridView gvUsers HERE--%>
                                Last update:
                                <%= DateTime.Now.ToString() %>
                                <br />
                                <button id="UpdatePanelBtn" runat="server" style="visibility:hidden;"></button>
                            </ContentTemplate>
                            <Triggers>
                              <asp:AsyncPostBackTrigger ControlID="UpdatePanelBtn" />
                            </Triggers>
                        </asp:UpdatePanel>
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

                $('#widget-container').removeClass('invisible');
                $('#widget-container-wizform').removeClass('invisible');
                //$('#widget-box-wizform').widget_box('show');

                $('#widget-box-wizform').on('reloaded.ace.widget', function (event,info) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');
                    $('form').each(function () { this.reset() });

                    //move to step 1
                    $('[data-step=1]').trigger("click");
                });

                $('#widget-box-1').on('reloaded.ace.widget', function (event) {
                    __doPostBack('<%= UpdatePanelBtn.ClientID %>', '');
                });

                $('#validation-form, #validation-form2, #prev, #next').bind('mousedown.ui-disableSelection selectstart.ui-disableSelection', function (event) {
                    event.stopImmediatePropagation();
                });

            })();
        })

    </script>

    <%--<%: Scripts.Render("~/scripts/Form-Wizard_Scripts") %>--%>
    <script type="text/javascript">
        
        function dummyFunc() { };

        function fncUpdatePanel() {
            //$('#widget-box-wizform').widget_box('toggle');
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
                success: function (dt)
                {
                    if (dt.d.indexOf("Fail") != -1)
                    {
                        $.gritter.add({
                            title: 'Added Process Fail!',
                            text: dt.d,
                            class_name: 'gritter-error gritter-light'
                        });
                        //location.reload();
                    }
                    else
                    {
                        $.gritter.add({
                            title: 'Added Process Successful!',
                            text: dt.d,
                            class_name: 'gritter-dark'
                        });

                        __doPostBack('<%= UpdatePanelBtn.ClientID %>', '');
                    }
                }
            });
        }

        jQuery(function ($) {

            $('[data-rel=tooltip]').tooltip();

            $('.select2').css('width', '200px').select2({ allowClear: true })
            .on('change', function () {
                $(this).closest('form').validate().element($(this));
            });

            var $validation = true;
            $('#fuelux-wizard-container').ace_wizard({
                //step: 2 //optional argument. wizard will jump to step "2" at first
                //buttons: '.wizard-actions:eq(0)'
            })
            .on('actionclicked.fu.wizard', function (e, info) {
                if (info.step == 1 && $validation) {
                    if (!$('#validation-form').valid()) e.preventDefault();
                }
                if (info.step == 2 && $validation) {
                    if (!$('#validation-form2').valid()) e.preventDefault();
                }
            })
            //.on('changed.fu.wizard', function() {
            //})
            .on('finished.fu.wizard', function (e)
            {
                fncUpdatePanel();

                //bootbox.dialog({
                //    message: "Thank you! Your information was successfully saved!",
                //    buttons: {
                //        "success": {
                //            "label": "OK",
                //            "className": "btn-sm btn-primary",
                //            "callback": function () {
                //                fncUpdatePanel()
                //            }
                //        }
                //    }
                //});
            }).on('stepclick.fu.wizard', function (e) {
                //e.preventDefault();//this will prevent clicking and selecting steps
            });

            $.mask.definitions['~'] = '[+-]';
            $('#phone').mask('(999) 999-9999');
            $('#ic').mask('999999-99-9999');

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
                        email: {
                            required: true,
                            email: true
                        },
                        password: {
                            required: true,
                            minlength: 5
                        },
                        password2: {
                            required: true,
                            minlength: 5,
                            equalTo: "#password"
                        },
                        username: {
                            required: true
                        },
                        question: {
                            required: true
                        },
                        answer: {
                            required: true
                        },
                        fullname: {
                            required: true,
                        },
                        ic: {
                            required: true,
                            ic: 'required'
                        },
                        phone: {
                            required: true,
                            phone: 'required'
                        },
                        agree: {
                            required: true
                        }
                    },

                    messages: {
                        email: {
                            required: "Please provide a valid email.",
                            email: "Please provide a valid email."
                        },
                        password: {
                            required: "Please specify a password.",
                            minlength: "Please specify a secure password."
                        },
                        username: "Please specify a username.",
                        question: "Please specify a security question.",
                        answer: "Please specify a security answer.",
                        fullname: "Please specify a fullname.",
                        agree: "Please accept our policy."
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
                $('[class*=select2]').remove();
            });
        })

    </script>

</asp:Content>
