<%@ Page Title="Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="BP.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
     <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
        <li class="active">User Profile</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
        <div class="page-header">
		<h1>
			User Profile
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				view &amp; manage your profile
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
	        <div id="user-profile-3" class="user-profile row">
		        <div class="col-sm-offset-1 col-sm-10">
			        <div class="tabbable">
				        <ul class="nav nav-tabs padding-16">
					        <li class="active">
						        <a data-toggle="tab" href="#edit-basic">
							        <i class="green ace-icon fa fa-pencil-square-o bigger-125"></i>
							        Info
						        </a>
					        </li>

					        <li>
						        <a data-toggle="tab" href="#edit-password">
							        <i class="blue ace-icon fa fa-key bigger-125"></i>
							        Password
						        </a>
					        </li>

                            <li>
						        <a data-toggle="tab" href="#edit-settings">
							        <i class="purple ace-icon fa fa-cog bigger-125"></i>
							        Security
						        </a>
					        </li>
				        </ul>

				        <div class="tab-content profile-edit-tab-content">
					        <div id="edit-basic" class="tab-pane in active">
						        <h4 class="header blue bolder smaller">General</h4>

						        <div class="row">
							        <div class="col-xs-12 col-sm-4">
                                        <img id="avatar" class="hidden" runat="server" />
                                        <asp:FileUpload ID="imgUpload" runat="server" />
								        <%--<input type="file" id="Upload" runat="server" />--%>
							        </div>

							        <div class="vspace-12-sm"></div>

							        <div class="col-xs-12 col-sm-8">
								        <div class="form-group">
                                            <label class="col-sm-4 control-label no-padding-right" for="username">Username</label>

									        <div class="col-sm-8">
                                                <input class="col-xs-12 col-sm-6" id="username" type="text" placeholder="Username" readonly="true" runat="server" />
									        </div>
								        </div>

								        <div class="space-4"></div>

								        <div class="form-group">
									        <label class="col-sm-4 control-label no-padding-right" for="fullname">Fullname</label>

									        <div class="col-sm-8">
                                                <input class="col-xs-12 col-sm-10" id="fullname" type="text" placeholder="Fullname" runat="server" />
                                                <br /><br />
                                                <asp:RequiredFieldValidator ID="ReqFullname" runat="server" ControlToValidate="fullname" ForeColor="Red"
                                                    CssClass="failureNotification" ErrorMessage="Full Name is required." Display="Dynamic"></asp:RequiredFieldValidator>
									        </div>
								        </div>
							        </div>
						        </div>

						        <hr />
						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="birthdate">Birth Date</label>

							        <div class="col-sm-9">
								        <div class="input-medium">
									        <div class="input-group">
                                                <input class="input-medium date-picker" id="birthdate" type="text" data-date-format="dd-mm-yyyy" placeholder="dd-mm-yyyy"
                                                    runat="server" />
										        <span class="input-group-addon">
											        <i class="ace-icon fa fa-calendar"></i>
										        </span>
									        </div>
								        </div>
							        </div>
						        </div>

						        <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right">Gender</label>

							        <div class="col-sm-9">
								        <label class="inline">
									        <input name="form-field-radio" type="radio" class="ace" id="rbMale" runat="server" />
									        <span class="lbl middle"> Male</span>
								        </label>

								        &nbsp; &nbsp; &nbsp;
								        <label class="inline">
									        <input name="form-field-radio" type="radio" class="ace" id="rbFemale" runat="server" />
									        <span class="lbl middle"> Female</span>
								        </label>
							        </div>
						        </div>

						        <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="comment">Comment</label>

							        <div class="col-sm-9">
								        <textarea id="comment" runat="server"></textarea>
							        </div>
						        </div>

						        <div class="space"></div>
						        <h4 class="header blue bolder smaller">Contact</h4>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="email">Email</label>

							        <div class="col-sm-9">
								        <span class="input-icon input-icon-right">
									        <input type="email" id="email" runat="server"/>
									        <i class="ace-icon fa fa-envelope"></i>
								        </span>
							        </div>
						        </div>

						        <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="website">Website</label>

							        <div class="col-sm-9">
								        <span class="input-icon input-icon-right">
									        <input type="url" id="website" runat="server" />
									        <i class="ace-icon fa fa-globe"></i>
								        </span>
							        </div>
						        </div>

						        <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="phone">Phone</label>

							        <div class="col-sm-9">
								        <span class="input-icon input-icon-right">
									        <input class="input-medium input-mask-phone" type="text" id="phone" runat="server"/>
									        <i class="ace-icon fa fa-phone fa-flip-horizontal"></i>
								        </span>
							        </div>
						        </div>
					        </div>

					        <div id="edit-settings" class="tab-pane">
						        <div class="space-10"></div>

                                <div class="alert alert-success alert-dismissable">
							        <button type="button" class="close" data-dismiss="alert">
								        <i class="ace-icon fa fa-times"></i>
							        </button>

							        <i class="ace-icon fa fa-umbrella bigger-120 blue"></i>
							        Customize your security question &amp; answer.
						        </div>

                                <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="question">Security Question</label>

							        <div class="col-sm-9">
								        <textarea id="question" runat="server" cols="50"></textarea>
							        </div>
						        </div>
							
                                <div class="space-4"></div>

                                <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="answer">Security Answer</label>

							        <div class="col-sm-9">
								        <textarea id="answer" runat="server" cols="50"></textarea>
							        </div>
						        </div>

					        </div>

					        <div id="edit-password" class="tab-pane">
						        <div class="space-10"></div>

                                <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="oldpassword">Old Password</label>

							        <div class="col-sm-9">
								        <input type="password" id="oldpassword" runat="server" />
							        </div>
						        </div>

                                <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="newpassword">New Password</label>

							        <div class="col-sm-9">
								        <input type="password" id="newpassword" runat="server" />
							        </div>
						        </div>

						        <div class="space-4"></div>

						        <div class="form-group">
							        <label class="col-sm-3 control-label no-padding-right" for="confirmpassword">Confirm Password</label>

							        <div class="col-sm-9">
								        <input type="password" id="confirmpassword" runat="server" />
							        </div>
						        </div>
					        </div>
				        </div>
			        </div>

			        <div class="clearfix form-actions">
				        <div class="col-md-offset-3 col-md-9">
                            <asp:LinkButton id="btnSave" CssClass="btn btn-info" OnClick="btnSave_OnClick" runat="server">
                                <i class="ace-icon fa fa-check bigger-110"></i>
						        Save
                            </asp:LinkButton>
					        &nbsp; &nbsp;
					        <button class="btn" type="reset" runat="server">
						        <i class="ace-icon fa fa-undo bigger-110"></i>
						        Reset
					        </button>
				        </div>
			        </div>
		        </div><!-- /.span -->
	        </div><!-- /.user-profile -->
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID = "btnSave" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">
        var baseUrl = '<%= Page.ResolveClientUrl("~/") %>';

        function PopulateImage(src) {
            var url = src.replace("~/", baseUrl);

            $('#MainContent_avatar').attr('src', url).load(function () {
                $('#user-profile-3').find('input[type=file]').ace_file_input('show_file_list', [{ type: 'image', name: $('#MainContent_avatar').attr('src') }]);
            });
        }

        jQuery(function ($) {

            $('#user-profile-3').find('input[type=file]').ace_file_input({
                style: 'well',
                btn_choose: 'Change avatar',
                btn_change: null,
                no_icon: 'ace-icon fa fa-picture-o',
                thumbnail: 'large',
                droppable: true,
                allowExt: ['jpg', 'jpeg', 'png', 'gif'],
                allowMime: ['image/jpg', 'image/jpeg', 'image/png', 'image/gif']
            })
			.end().find('button[type=reset]').on(ace.click_event, function () {
			    $('#user-profile-3 input[type=file]').ace_file_input('reset_input');
			})
			.end().find('.date-picker').datepicker().next().on(ace.click_event, function () {
			    $(this).prev().focus();
			});

            $('.input-mask-phone').mask('(999) 999-9999');

        })

	</script>
</asp:Content>
