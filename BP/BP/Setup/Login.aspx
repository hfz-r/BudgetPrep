<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BP.Setup.Login" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
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

        <!-- text fonts -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/fonts.googleapis.com.css")%>" />

    <!-- ace styles -->
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/ace.min.css")%>" />
    <link rel="stylesheet" href="<%=Page.ResolveUrl("~/assets/css/ace-rtl.min.css")%>" />
<!----------------------- load css end ----------------------->

</head>
<body class="login-layout">
		<div class="main-container">
			<div class="main-content">
				<div class="row">
					<div class="col-sm-10 col-sm-offset-1">
						<div class="login-container">
							<div class="center" style="margin-top:20px;margin-bottom:10px">
								<img src="../Images/BP/mybudget2.png" />							</div>

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

											<form id="main" runat="server">
                                                <asp:Login ID="LoginUser" runat="server" RenderOuterTable="False" 
                                                    OnAuthenticate="LoginUser_Authenticate" 
                                                    OnLoginError="LoginUser_LoginError" 
                                                    FailureTextStyle-ForeColor="Red">
                                                    <LayoutTemplate>
                                                        <fieldset>
                                                            <span class="small">
                                                                <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                            </span>
                                                            <div class="space-4"></div>

                                                            <label class="block clearfix">
			                                                    <span class="block input-icon input-icon-right">
				                                                    <asp:TextBox ID="UserName" class="form-control" placeholder="Username" runat="server"></asp:TextBox>
                                                                    <i class="ace-icon fa fa-user"></i>
                                                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ForeColor="Red"
                                                                        CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required." Display="Dynamic"
                                                                        ValidationGroup="LoginUserValidationGroup"></asp:RequiredFieldValidator>
			                                                    </span>
                                                            </label>
                                                            
                                                            <label class="block clearfix">
			                                                    <span class="block input-icon input-icon-right">
                                                                    <asp:TextBox ID="Password" class="form-control" placeholder="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                                    <i class="ace-icon fa fa-lock"></i>
                                                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ForeColor="Red" 
                                                                        CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required." Display="Dynamic"
                                                                        ValidationGroup="LoginUserValidationGroup"></asp:RequiredFieldValidator>            
			                                                    </span>
		                                                    </label>

		                                                    <div class="space"></div>
                                                             
		                                                    <div class="clearfix">
			                                                    <label class="inline">
				                                                    <input type="checkbox" ID="RememberMe" class="ace" runat="server" />
				                                                    <span class="lbl"> Remember Me</span>
			                                                    </label>

			                                                    <asp:LinkButton ID="LoginButton" runat="server" CommandName="Login" CssClass="width-35 pull-right btn btn-sm btn-primary" ValidationGroup="LoginUserValidationGroup">
                                                                    <span><i class="ace-icon fa fa-key"></i></span> <span class="bigger-110">Login</span>
                                                                </asp:LinkButton>
		                                                    </div>
	                                                    </fieldset>
                                                    </LayoutTemplate>
                                                </asp:Login>
                                            </form>

											<div class="social-or-login center">
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
											</div>
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
												Retrieve Password
											</h4>

											<div class="space-6"></div>
											<p>
												Enter your email and to receive instructions
											</p>

											<form>
												<fieldset>
													<label class="block clearfix">
														<span class="block input-icon input-icon-right">
															<input type="email" class="form-control" placeholder="Email" />
															<i class="ace-icon fa fa-envelope"></i>
														</span>
													</label>

													<div class="clearfix">
														<button type="button" class="width-35 pull-right btn btn-sm btn-danger">
															<i class="ace-icon fa fa-lightbulb-o"></i>
															<span class="bigger-110">Send Me!</span>
														</button>
													</div>
												</fieldset>
											</form>
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
			</div><!-- /.main-content -->
		</div><!-- /.main-container -->

<!----------------------- load scripts start ----------------------->
        <script src="<%=Page.ResolveUrl("~/assets/js/jquery-2.1.4.min.js")%>"></script>  
<!----------------------- load scripts end ----------------------->

		<script type="text/javascript">
		    if ('ontouchstart' in document.documentElement) document.write("<script src='assets/js/jquery.mobile.custom.min.js'>" + "<" + "/script>");
		</script>

		<!-- inline scripts related to this page -->
		<script type="text/javascript">
		    jQuery(function ($) {
		        $(document).on('click', '.toolbar a[data-target]', function (e) {
		            e.preventDefault();
		            var target = $(this).data('target');
		            $('.widget-box.visible').removeClass('visible');//hide others
		            $(target).addClass('visible');//show target
		        });
		    });

		</script>
	</body>
</html>
