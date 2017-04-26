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
    <div id="EditForm" runat="server" visible="false">
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
                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="col-xs-12 col-sm-6 muted" />
                                         <%--<input type="text" name="rolename" id="rolename" class="col-xs-12 col-sm-6" runat="server" />--%>
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
                                        <asp:TextBox id="txtDesc" CssClass="col-xs-12 col-sm-6" TextMode="multiline" Columns="50" Rows="5" runat="server" />
									</div>
								</div>
							</div>

                            <div class="space-2"></div>

                            <div class="form-group">
								<label class="control-label col-xs-12 col-sm-3 no-padding-right" for="status">Status:</label>
                                                        
                                <div class="col-xs-12 col-sm-9">
									<div class="clearfix">
										<asp:DropDownList ID="ddlStatus" runat="server" CssClass="col-xs-12 col-sm-4" />
									</div>
								</div>
							</div>

                            <div class="clearfix form-actions">
								<div class="col-md-offset-9 col-md-9">
                                    <%--<asp:LinkButton ID="btnSubmit" runat="server" CssClass="btn btn-info" OnClick="btnSave_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Submit
                                    </asp:LinkButton>--%>
                                    <button id="btnSubmit" type="submit" class="btn btn-info" runat="server" onclick="btnSubmit_Submit(event);">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Submit
                                    </button>
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

				<div id="RoleList" class="widget-body">
					<div class="widget-main">
                        <div class="clearfix">
                            <div class="pull-right tableTools-container"></div>
                        </div>
                        <asp:GridView ID="gvRoles" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-hover" 
                            DataKeyNames="RoleID" OnRowDataBound="gvRoles_RowDataBound" OnRowCommand="gvRoles_RowCommand" OnPreRender="gvRoles_PreRender">
                            <Columns>
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <span id="count" class="badge badge-yellow tooltip-warning" data-rel="tooltip" data-placement="left" 
                                            title="null" runat="server"></span>
                                        <asp:Label ID="lblRolename" runat="server" Text='<%# Eval("RoleName")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" 
                                    HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <span id="CustomStatus" runat="server"></span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div class="hidden-sm hidden-xs btn-group">
                                            <asp:LinkButton ID="btnEditRow" runat="server" CommandName="EditRow" CommandArgument='<%# Container.DataItemIndex %>' 
                                                OnClientClick="ShowEditForm();" CssClass="btn btn-white btn-minier btn-bold" PostBackUrl="~/Setup/UserRole.aspx">
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

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <!-- Duallist-Box-->
    <script src="Bootstrap-Duallistbox/asp.net-duallistbox-multiple-instances.js"></script>
    <script src="Bootstrap-Duallistbox/jquery.bootstrap-duallistbox.js"></script>
    <script src="Bootstrap-Duallistbox/UserRole.aspx.js"></script>

    <script type="text/javascript">

        var demo1 = $('select[name="ctl00$MainContent$dlbGroupA$dlbGeneric"]').bootstrapDualListbox();

        function ShowEditForm() {
            spinnerInit();
            ace.data.remove('demo', 'widget-state');
            ace.data.remove('demo', 'widget-order');
            $("#EditForm").show();
        }

        function btnSubmit_Submit(event) {
            spinnerInit();
            event.preventDefault();
            $.ajax({
                type: "POST",
                url: "UserRole.aspx/OnSubmit",
                data: '{roleid: "' + $('#<%=ddlRole.ClientID%>').val() + '",' + 
                      'dlbox: "' + demo1.val() + '",' + 
                      'desc: "' + $('#<%=txtDesc.ClientID%>').val() + '",' + 
                      'stats: "' + $('#<%=ddlStatus.ClientID%>').val() + '" }',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $('#spin').data('spinner').stop();
                    $("#spin").hide();

                    var res = JSON.parse(response.d);

                    bootbox.alert({
                        title: res.status,
                        message: res.result,
                        callback: function () {
                            $('form#edit-form')[0].reset();
                            location.href = "<%=Page.ResolveUrl("~/Setup/UserRole.aspx")%>";
                        }
                    });
                },
                failure: function (response) {
                    alert(response.d.result);
                }
            });
        }

        $(document).ready(function () {

            $('select[name="ctl00$MainContent$dlbGroupA$dlbGeneric_helper2"').css("background-color", "#F4F4F4");

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
                    cancel: '#edit-form, #RoleList',
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

                //edit-form onreloaded
                $('#widget-box-edit').on('reloaded.ace.widget', function (event, info) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');
                    $('form#edit-form')[0].reset();
                    
                    demo1.bootstrapDualListbox('refresh', true);
                });

                //edit-form onclosed
                $('#widget-box-edit').on('closed.ace.widget', function (event, info) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');
                    $('form#edit-form')[0].reset();

                    demo1.bootstrapDualListbox('refresh', true);

                    $("#MainContent_gvRoles tr").each(function () {
                        $(this).css("background-color", "");
                    });
                });

                //users-role list onreloaded
                $('#widget-box-list').on('reloaded.ace.widget', function (event, info) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');

                    var myTable = $('#<%=gvRoles.ClientID%>').DataTable();
                    myTable.state.clear();

                    location.href = "<%=Page.ResolveUrl("~/Setup/UserRole.aspx")%>";
                });
                

                $('[data-rel=tooltip]').tooltip();
            });

            //datatables jquery
            jQuery(function ($) {

                //initiate dataTables plugin
                var myTable = $('#<%=gvRoles.ClientID%>').DataTable({
                    bAutoWidth: false,
                    "aoColumns": [
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
                          "className": "btn btn-white btn-primary btn-bold"
                      },
                      {
                          "extend": "csvHtml5",
                          "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                          "className": "btn btn-white btn-primary btn-bold",
                          exportOptions: {
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
                          message: 'This print was produced using the Print button for DataTables'
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

                setTimeout(function () {
                    $($('.tableTools-container')).find('a.dt-button').each(function () {
                        var div = $(this).find(' > div').first();
                        if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                        else $(this).tooltip({ container: 'body', title: $(this).text() });
                    });
                }, 500);

                $(document).on('click', '#<%=gvRoles.ClientID%> .dropdown-toggle', function (e) {
                    e.stopImmediatePropagation();
                    e.stopPropagation();
                    e.preventDefault();
                });
            })
        });

    </script>

</asp:Content>
