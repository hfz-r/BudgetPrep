<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BP.Setup.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta charset="utf-8" />
    
    <title><%: Page.Title %> - MyBudget</title>

    <meta name="description" content="overview &amp; stats" />
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

<!----------------------- load css start ----------------------->
    <!-- bootstrap & fontawesome -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/bootstrap.min.css")%>" />
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/font-awesome/4.5.0/css/font-awesome.min.css")%>" />

    <!-- page specific plugin styles -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/jquery.gritter.min.css")%>" />
    <!-- page specific plugin styles end -->

    <!-- text fonts -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/fonts.googleapis.com.css")%>" />

    <!-- ace styles -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/ace.min.css")%>" />
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/ace-rtl.min.css")%>" />
<!----------------------- load css end ----------------------->

    <!-- ace settings handler -->
	<script src="<%=Page.ResolveUrl("~/assets/js/ace-extra.min.js")%>"></script>

    <style>
        #spin {
		    position: fixed;
            text-align: center; 
            height: 100%; 
            width: 100%; 
            top: 0; 
            right: 0; 
            left: 0; 
            z-index: 9999999; 
            background-color: gray; 
            opacity: 0.7;
        }
    </style>
</head>
<body class="login-layout">
    <form id="login" runat="server">

        <div id="spin" style="display:none;"></div>

        <div class="main-container">
	        <div class="main-content">
			    <div class="row">
				    <div class="col-sm-10 col-sm-offset-1">
					    <div class="login-container">
						    <div class="center" style="margin-top:20px;margin-bottom:10px">
							    <img src="../Images/BP/mybudget2.png" />						    </div>

						    <div class="space-6"></div>

						    <div class="position-relative">
							    <div id="login-box" class="login-box visible widget-box no-border">
								    <div class="widget-body">
									    <div class="widget-main">
										    <h4 class="header blue lighter bigger">
											    <i class="ace-icon fa fa-coffee green"></i>
											    Enter Your Login Information
										    </h4>

										    <div class="space-6"></div>

                                            <asp:Login ID="LoginUser" runat="server" RenderOuterTable="False" 
                                                    OnAuthenticate="LoginUser_Authenticate" 
                                                    OnLoginError="LoginUser_LoginError" 
                                                    FailureTextStyle-ForeColor="Red">
                                                    <LayoutTemplate>
                                                        <fieldset>
                                                            <span class="small red">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                            </span>
                                                            <div class="space-4"></div>

                                                            <div class="form-group">
                                                                <label class="block clearfix">
                                                                    <span class="block input-icon input-icon-right">
                                                                        <asp:TextBox ID="UserName" placeholder="Username" CssClass="form-control" runat="server" />
                                                                        <i class="ace-icon fa fa-user"></i>
                                                                    </span>
                                                                </label>
                                                                <asp:RequiredFieldValidator
                                                                    ID="UserNameRequired"
                                                                    runat="server"
                                                                    ControlToValidate="UserName"
                                                                    Display="Dynamic"
                                                                    CssClass="help-block"
                                                                    ErrorMessage="Username is required."
                                                                    ValidationGroup="LoginUserValidationGroup" />
                                                                <asp:CustomValidator
                                                                    ID="CustomValidator1"
                                                                    runat="server"
                                                                    ClientValidationFunction="CustomValidationFunction"
                                                                    Display="None"
                                                                    ValidateEmptyText="True"
                                                                    ValidationGroup="LoginUserValidationGroup"
                                                                    ControlToValidate="UserName"
                                                                    SetFocusOnError="true" />
                                                            </div>

                                                            <div class="form-group">
                                                                <label class="block clearfix">
                                                                    <span class="block input-icon input-icon-right">
                                                                        <asp:TextBox ID="Password" CssClass="form-control" placeholder="Password"
                                                                            runat="server" TextMode="Password" />
                                                                        <i class="ace-icon fa fa-lock"></i>
                                                                    </span>
                                                                </label>
                                                                <asp:RequiredFieldValidator
                                                                    ID="PasswordRequired"
                                                                    runat="server"
                                                                    ControlToValidate="Password"
                                                                    Display="Dynamic"
                                                                    CssClass="help-block"
                                                                    ErrorMessage="Password is required."
                                                                    ValidationGroup="LoginUserValidationGroup" />
                                                                <asp:CustomValidator
                                                                    ID="CustomValidator2"
                                                                    runat="server"
                                                                    ClientValidationFunction="CustomValidationFunction"
                                                                    Display="None"
                                                                    ValidateEmptyText="True"
                                                                    ValidationGroup="LoginUserValidationGroup"
                                                                    ControlToValidate="Password"
                                                                    SetFocusOnError="true" />
                                                            </div>

		                                                    <div class="space"></div>
                                                             
		                                                    <div class="clearfix">
                                                                <label class="inline">
                                                                    <asp:CheckBox ID="RememberMe" runat="server"/> 
                                                                    <span class="lbl"> Remember Me</span>
                                                                </label>

			                                                    <asp:LinkButton ID="LoginButton" runat="server" CommandName="Login" 
                                                                    CssClass="width-35 pull-right btn btn-sm btn-primary" ValidationGroup="LoginUserValidationGroup">
                                                                    <i class="ace-icon fa fa-key"></i>
                                                                    <span class="bigger-110">Login</span>
                                                                </asp:LinkButton>
		                                                    </div>
	                                                    </fieldset>
                                                    </LayoutTemplate>
                                                </asp:Login>

										    <%--<div class="social-or-login center">
											    <span class="bigger-110">Or Login Using</span>
										    </div>

										    <div class="space-6"></div>

										    <div class="social-login center">
											    <a class="btn btn-primary">
												    <i class="ace-icon fa fa-facebook"></i>
											    </a>

											    <a class="btn btn-info">
												    <i class="ace-icon fa fa-twitter"></i>
											    </a>

											    <a class="btn btn-danger">
												    <i class="ace-icon fa fa-google-plus"></i>
											    </a>
										    </div>--%>

									    </div><!-- /.widget-main -->

									    <div class="toolbar clearfix">
										    <div>
											    <a href="#" data-target="#forgot-box" class="forgot-password-link">
												    <i class="ace-icon fa fa-arrow-left"></i>
												    I forgot my password
											    </a>
										    </div>
									    </div>
								    </div><!-- /.widget-body -->
							    </div><!-- /.login-box -->

							    <div id="forgot-box" class="forgot-box widget-box no-border">
								    <div class="widget-body">
									    <div class="widget-main">
										    <h4 class="header red lighter bigger">
											    <i class="ace-icon fa fa-key"></i>
											    Forgot Your Password?
										    </h4>
										    <div class="space-6"></div>
										    <p>
											    Input <i class="red">email</i> to reset password.
										    </p>

                                            <fieldset>
                                                <span class="small red">
					                                <asp:Label ID="lblMessage" runat="server" CssClass="control-label txt-info"></asp:Label>
				                                </span>
                                                <div class="space-4"></div>

                                                <div class="form-group">
                                                    <label class="block clearfix">
                                                        <span class="block input-icon input-icon-right">
                                                            <asp:TextBox ID="tbEmail" CssClass="form-control" placeholder="Email" TextMode="Email"
                                                                runat="server" ValidationGroup="VerifyAccountCheck"></asp:TextBox>
                                                            <i class="ace-icon fa fa-envelope"></i>
                                                        </span>
                                                    </label>
                                                    <asp:RequiredFieldValidator 
                                                        ID="EmailValidate" 
                                                        runat="server" 
                                                        ControlToValidate="tbEmail"
                                                        ErrorMessage="Email is required." 
                                                        Display="Dynamic" 
                                                        CssClass="help-block"
                                                        ValidationGroup="VerifyAccountCheck" />
                                                    <asp:RegularExpressionValidator 
                                                        ID="regexEmailValid" 
                                                        runat="server" 
                                                        CssClass="help-block"
                                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                        ControlToValidate="tbEmail" 
                                                        ErrorMessage="Invalid Email Format"
                                                        Display="Dynamic" 
                                                        ValidationGroup="VerifyAccountCheck" />
                                                    <asp:CustomValidator
                                                        ID="CustomValidator3"
                                                        runat="server"
                                                        ClientValidationFunction="CustomValidationFunction"
                                                        Display="None"
                                                        ValidateEmptyText="True"
                                                        ValidationGroup="VerifyAccountCheck"
                                                        ControlToValidate="tbEmail"
                                                        SetFocusOnError="true" />
                                                </div>

			                                    <div class="clearfix">
                                                    <asp:LinkButton ID="VerifyButton" runat="server" CssClass="width-35 pull-right btn btn-sm btn-danger" 
                                                        ValidationGroup="VerifyAccountCheck" OnClientClick="return VerifyButtonFunction(event)" > 
					                                    <i class="ace-icon fa fa-lightbulb-o"></i>
					                                    <span class="bigger-110">Reset</span>
				                                    </asp:LinkButton>
			                                    </div>
                                            </fieldset>

									    </div><!-- /.widget-main -->

									    <div class="toolbar center">
										    <a href="#" data-target="#login-box" class="back-to-login-link">
											    Back to login
											    <i class="ace-icon fa fa-arrow-right"></i>
										    </a>
									    </div>
								    </div><!-- /.widget-body -->
							    </div><!-- /.forgot-box -->
						    </div><!-- /.position-relative -->
					    </div>
				    </div><!-- /.col -->
			    </div><!-- /.row -->

                <!-- modal start -->
                <div id="QuestionModal" class="modal fade" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
					<div class="modal-dialog">
						<div class="modal-content">
							<div class="modal-header">
								<button type="button" class="close" data-dismiss="modal">&times;</button>
								<h4 class="blue bigger">Verify Your Identity</h4>
							</div>

							<div class="modal-body">
								<div class="row">
									<div class="col-xs-12">

                                        <div class="form-group">
                                            <div class="alert alert-warning">
											    <button type="button" class="close" data-dismiss="alert">
												    <i class="ace-icon fa fa-times"></i>
											    </button>
                                                <h4>Please answer your security questions.</h4>
                                                These questions help us verify your identity.
										    </div>
                                        </div>
										
                                        <div class="space-4"></div>

                                        <div class="form-group">
                                            <label class="control-label col-xs-3 no-padding-right" for="question">Question</label>

                                            <div class="col-xs-9">
									            <div class="clearfix">
                                                    <asp:Label ID="lblQuestion" runat="server" CssClass="control-label bolder"></asp:Label>
									            </div>
								            </div>
										</div>
                                        
										<div class="space-4"></div><br />

										<div class="form-group">
                                            <label class="control-label col-xs-3 no-padding-right" for="answer">Answer</label>

                                            <div class="col-xs-9">
									            <div class="clearfix">
										            <textarea class="form-control" name="answer" id="answer" runat="server"></textarea>
									            </div>
								            </div>
										</div>
									</div>
								</div>
							</div>

							<div class="modal-footer">
								<button class="btn btn-sm" data-dismiss="modal">
									<i class="ace-icon fa fa-times"></i>
									Cancel
								</button>

                                <asp:LinkButton ID="btnProceed" runat="server" CssClass="btn btn-sm btn-primary" OnClientClick="return ProceedBtnFunction(event)">
                                    <i class="ace-icon fa fa-check"></i>Proceed
                                </asp:LinkButton>
							</div>
						</div>
					</div>
				</div><!-- modal end -->

                <asp:HiddenField ID="hdnUsername" runat="server" />

	        </div><!-- /.main-content -->
        </div><!-- /.main-container -->

<!----------------------- load scripts start ----------------------->
    <!--[if !IE]> -->
	<script src="<%=Page.ResolveUrl("~/assets/js/jquery-2.1.4.min.js")%>"></script>
	<!-- <![endif]-->

	<script type="text/javascript">
	    if ('ontouchstart' in document.documentElement) document.write("<script src='<%=Page.ResolveUrl("~/assets/js/jquery.mobile.custom.min.js")%>'>"
            + "<" + "/script>");
	</script>
    <script src="<%=Page.ResolveUrl("~/assets/js/bootstrap.min.js")%>"></script>     
    
    <!-- page specific plugin scripts start -->
    <script src="<%=Page.ResolveUrl("~/assets/js/jquery.gritter.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/assets/js/spin.js")%>"></script>
    <!-- page specific plugin scripts end -->

    <!-- ace scripts -->
	<script src="<%=Page.ResolveUrl("~/assets/js/ace-elements.min.js")%>"></script>
	<script src="<%=Page.ResolveUrl("~/assets/js/ace.min.js")%>"></script>
<!----------------------- load scripts end ----------------------->

	<!-- inline scripts related to this page -->
	<script type="text/javascript">
	    jQuery(function ($) {
	        $(document).on('click', '.toolbar a[data-target]', function (e) {
	            e.preventDefault();
	            var target = $(this).data('target');
	            $('.widget-box.visible').removeClass('visible');//hide others
	            $(target).addClass('visible');//show target
	        });

	        $('#LoginUser_RememberMe').addClass("ace");
	    });

	    function VerifyButtonFunction(event) {
	        spinnerInit();

	        event.preventDefault();
	        $.ajax({
	            type: "POST",
	            url: '<%= ResolveUrl("Login.aspx/GetVerification") %>',
	            data: '{email: "' + $("#<%=tbEmail.ClientID%>").val() + '" }',
	            contentType: "application/json; charset=utf-8",
	            dataType: "json",
	            success: function (response) {
	                $('#spin').data('spinner').stop();
	                $("#spin").hide();

	                if (response.d.Response != "") {
	                    $('#<%=lblMessage.ClientID%>').text(response.d.Response);
	                }
	                else {
	                    $('#QuestionModal').modal('show');

	                    $('#QuestionModal').on('shown.bs.modal', function () {
	                        $('#<%=hdnUsername.ClientID%>').val(response.d.Username)
		                    $('#<%=lblQuestion.ClientID%>').text(response.d.Question);
		                });
                    }
	            },
	            failure: function (response) {
	                $('#<%=lblMessage.ClientID%>').text(response.d);
	            }
	        });
            }

            function ProceedBtnFunction(event) {
                spinnerInit();

                event.preventDefault();
                $.ajax({
                    type: "POST",
                    url: '<%= ResolveUrl("Login.aspx/ResetPassword") %>',
                    data: '{username: "' + $('#<%=hdnUsername.ClientID%>').val() + '",answer: "' + $("#answer").val() + '" }',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        $('#spin').data('spinner').stop();
                        $("#spin").hide();

                        var res = JSON.parse(response.d);

                        $.gritter.add({
                            title: res.status,
                            text: res.result,
                            class_name: (res.status == 'Fail' ? 'gritter-error' : 'gritter-info') + ' gritter-light',
                            after_close: function () {
                                if (res.status == 'Success') {
                                    location.reload();
                                }
                            }
                        });
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }

            function CustomValidationFunction(sender, args) {
                var control = document.getElementById(sender.controltovalidate);

                if (args.Value == "") {
                    args.isValid = false;
                    $(control).closest(".form-group").addClass("has-error");

                    return;
                }
                else {

                    if (sender.controltovalidate == "tbEmail") {
                        //RegularExpressionValidator
                        var exp = new RegExp(control.Validators[1].validationexpression);

                        if (exp.test(args.Value)) {
                            args.isValid = true;
                            $(control).closest(".form-group").removeClass("has-error");

                            return;
                        }
                        else {
                            args.isValid = false;
                            $(control).closest(".form-group").addClass("has-error");

                            return;
                        }
                    }
                    else {
                        args.isValid = true;
                        $(control).closest(".form-group").removeClass("has-error");

                        return;
                    }
                }
            }

            function spinnerInit() {
                var opts = {
                    lines: 12, // The number of lines to draw
                    length: 7, // The length of each line
                    width: 4, // The line thickness
                    radius: 10, // The radius of the inner circle
                    corners: 1, // Corner roundness (0..1)
                    rotate: 0, // The rotation offset
                    color: '#000', // #rgb or #rrggbb
                    speed: 1, // Rounds per second
                    trail: 66, // Afterglow percentage
                    shadow: false, // Whether to render a shadow
                    hwaccel: false, // Whether to use hardware acceleration
                };

                $("#spin").show().spin(opts);
            }

            $.fn.spin = function (opts) {
                this.each(function () {
                    var $this = $(this),
                        spinner = $this.data('spinner');
                    if (spinner) spinner.stop();
                    if (opts !== false) {
                        opts = $.extend({ color: $this.css('color') }, opts);
                        spinner = new Spinner(opts).spin(this);
                        $this.data('spinner', spinner);
                    }
                });
                return this;
            };

            $(document).one('ajaxloadstart.page', function (e) {
                $.gritter.removeAll();
                $('.modal').modal('hide');
            });

	</script>

    </form>
</body>
</html>