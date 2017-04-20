<%@ Page Title="User Roles" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserRole.aspx.cs" Inherits="BP.Setup.UserRole" %>

<%@ Register Src="~/Setup/DualListbox.ascx" TagPrefix="dlbox" TagName="DualListbox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <!-- Duallist-Box -->
    <link href="Bootstrap-Duallistbox/bootstrap-duallistbox.css" rel="stylesheet" />

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
	    <li class="">Setup</li>
        <li class="active">User Roles</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <div class="page-header">
		<h1>
			User Roles
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				create &amp; manage users roles
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Edit Form -->
    <div id="EitForm" runat="server">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-2">
			<div class="widget-box" id="widget-box-edit">
				<div class="widget-header">
					<h5 class="widget-title">New User Roles</h5>

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

                        <form class="form-horizontal" id="edit-form" role="form">
                            <br />
                            <div class="form-group">
								<label class="control-label col-xs-12 col-sm-3 no-padding-right" for="rolename">Role-name:</label>

								<div class="col-xs-12 col-sm-9">
									<div class="clearfix">
                                         <input type="text" name="rolename" id="rolename" class="col-xs-12 col-sm-6" runat="server" />
									</div>
								</div>
							</div>

                            <div class="space-8"></div>

                            <div class="form-group">
	                            <label class="col-xs-12 col-sm-3 control-label no-padding-right" for="duallist">Users-list:</label>

	                            <div class="col-xs-12 col-sm-9">
                                    <dlbox:DualListbox runat="server" DataTextField="Name" DataValueField="Id" SelectedLisLabel="Selected"
                                        AvailableListLabel="Non-selected" ID="dlbGroupA" />
	                            </div>
                            </div>

                            <div class="space-2"></div>

                             <div class="form-group">
								<label class="control-label col-xs-12 col-sm-3 no-padding-right" for="desc">Description:</label>

								<div class="col-xs-12 col-sm-9">
									<div class="clearfix">
                                        <textarea class="input-xlarge" name="desc" id="desc" runat="server"></textarea>
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

                            <div class="clearfix form-actions">
								<div class="col-md-offset-9 col-md-9">
                                    <button id="btnSubmit" type="submit" class="btn btn-info" runat="server">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Submit
                                    </button>
									&nbsp; &nbsp;
                                    <button id="btnReset" type="reset" class="btn" runat="server">
                                        <i class="ace-icon fa fa-undo bigger-110"></i>
										Reset
                                    </button>
								</div>
							</div>

                        </form>
														
					</div>
				</div>
			</div>
		</div>
    </div>

    <!-- List Form -->
    <div id="ListForm" runat="server">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-1">
			<div class="widget-box" id="widget-box-list">
				<div class="widget-header">
					<h5 class="widget-title">User Roles</h5>

					<div class="widget-toolbar">
						<div class="widget-menu">
							<a href="#" data-action="settings" data-toggle="dropdown">
								<i class="ace-icon fa fa-bars"></i>
							</a>

							<ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
								<li>
									<a data-toggle="tab" href="#dropdown1">Option#1</a>
								</li>

								<li>
									<a data-toggle="tab" href="#dropdown2">Option#2</a>
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
							
                            <div class="clearfix">
                            <div class="pull-right tableTools-container"></div>
                        </div>
                           
					</div>
				</div>
			</div>
		</div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <!-- Duallist-Box-->
    <script src="Bootstrap-Duallistbox/asp.net-duallistbox-multiple-instances.js"></script>
    <script src="Bootstrap-Duallistbox/jquery.bootstrap-duallistbox.js"></script>
    <script src="Bootstrap-Duallistbox/UserRole.aspx.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            //widget
            jQuery(function ($) {
                // widget box drag & drop example
                $('.widget-container-col').sortable({
                    connectWith: '.widget-container-col',
                    items: '> .widget-box',
                    handle: ace.vars['touch'] ? '.widget-title' : false,
                    cancel: '.fullscreen',
                    opacity: 0.8,
                    revert: true,
                    cancel: '#edit-form',
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

                //form onreloaded
                $('#widget-box-edit').on('reloaded.ace.widget', function (event, info) {
                });

            });

            //jquery validator
            jQuery(function ($) {
                $('#btnSubmit').on('click', function (event) {
                    $('#edit-form').valid();
                    event.preventDefault();
                });
         
                $('#edit-form').validate({
                    errorElement: 'div',
                    errorClass: 'help-block',
                    focusInvalid: false,
                    ignore: "",
                    rules: {
                        'ctl00$MainContent$rolename': { required: true }
                    },
                    messages: {
                        'ctl00$MainContent$rolename': "Please specify a role name."
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
            })
        });

    </script>

</asp:Content>
