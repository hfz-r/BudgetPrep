<%@ Page Title="Peruntukan Asal Setup" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PeruntukanAsalSetup.aspx.cs" Inherits="BP.PeruntukanAsalSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

<style type="text/css">
        
    body .popover { 
        max-width: 830px;
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
        <li class="active">PeruntukanAsal</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <div class="page-header">
		<h1>
			Peruntukan Asal
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				setup &amp; manage peruntukan asal
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <!-- upload box -->
    <div id="modal-form" class="modal" tabindex="-1">
	    <div class="modal-dialog" style="width:400px;">
		    <div class="modal-content">
                <div class="modal-header">
					<button type="button" class="close" data-dismiss="modal">&times;</button>
					<h5 class="blue bigger">Peruntukan Asal File Upload</h5>
				</div>

			    <div class="modal-body">
                    <div class="row">
						<div class="col-xs-12">
                            <%--<asp:FileUpload ID="FileUpload1" runat="server" />--%>
                            <input type="file" id="upload" name="upload" />
						</div>
                    </div>
                </div>

                <div class="modal-footer">
                    <a class="btn btn-sm btn-success pull-left" data-toggle="popover" title="Template Preview - Account Code" 
                        data-full="<%=Page.ResolveUrl("~/Images/BP/acc_code_template.png")%>" onclick="return false;">
                        Preview Sample
                    </a>

				    <button id="btnResetFile" type="reset" class="btn btn-sm">
					    <i class="ace-icon fa fa-times"></i>
					    Reset
				    </button>

				    <button id="btnUpload" class="btn btn-sm btn-primary" data-dismiss="modal">
					    <i class="ace-icon fa fa-cloud-upload"></i>
					    Upload
				    </button>
                    <%--<asp:LinkButton ID="btnUpload" runat="server" CssClass="btn btn-sm btn-primary" OnClick="btnUpload_Click">
                        <i class="ace-icon fa fa-cloud-upload"></i>
					    Upload
                    </asp:LinkButton>--%>
			    </div>
            </div>
        </div>
    </div>

    <!-- Edit Form -->
    <div id="EditForm" runat="server" visible="false">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-2">
			<div class="widget-box" id="widget-box-edit">
				<div class="widget-header">
					<h5 class="widget-title">Account Code - Edit</h5>

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

                        <div class="form-horizontal" id="edit-form" role="form">
                            <br />
                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="fullname">Account Code:</label>

                                <div class="col-xs-12 col-sm-9">
                                    <div class="clearfix">
                                        <asp:TextBox ID="tbCode" runat="server" CssClass="col-xs-12 col-sm-6"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbCode" Display="Dynamic" 
                                        CssClass="help-block" ErrorMessage="Account Code is required." ValidationGroup="SaveValidation" />
                                     <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="CustomValidationFunction" Display="None"
                                        ValidateEmptyText="True" ValidationGroup="SaveValidation" ControlToValidate="tbCode" SetFocusOnError="true" />
                                </div>
                            </div>

                            <div class="space-2"></div>

                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="tbDesc">Description:</label>

                                <div class="col-xs-12 col-sm-9">
                                    <div class="clearfix">
                                        <asp:TextBox ID="tbDesc" CssClass="col-xs-12 col-sm-6" TextMode="multiline" Columns="50" Rows="5" runat="server" />
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="tbDesc" Display="Dynamic" 
                                        CssClass="help-block" ErrorMessage="Description is required." ValidationGroup="SaveValidation" />
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="CustomValidationFunction" Display="None"
                                        ValidateEmptyText="True" ValidationGroup="SaveValidation" ControlToValidate="tbDesc" SetFocusOnError="true" />
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

                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-info" ValidationGroup="SaveValidation" OnClick="btnSave_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Save
                                    </asp:LinkButton>
                                    <%--<asp:LinkButton ID="btnCancel" runat="server" CssClass="btn" OnClick="btnCancel_Click">
                                        <i class="ace-icon fa fa-undo bigger-110"></i>
									    Reset
                                    </asp:LinkButton>--%>
                                    <button id="btnCancel" type="reset" class="btn" onclick="Reset()">
                                        <i class="ace-icon fa fa-undo bigger-110"></i>Reset
                                    </button>
                                </div>
                            </div>

                        </div>
														
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
					<h5 class="widget-title">Account Code - List</h5>

					<div class="widget-toolbar">
                        <div class="widget-menu">
						    <a href="#" data-action="settings" data-toggle="dropdown" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Settings">
							    <i class="ace-icon fa fa-cogs"></i>
						    </a>

						    <ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
							    <li>
                                    <a href="#modal-form" id="btnFileUpload" runat="server" role="button" class="blue" data-toggle="modal" data-rel="tooltip" 
                                        data-placement="top" title="**[Year Upload] must in 'Active' to use this function**">
                                        <i class="ace-icon fa fa-cloud-upload bigger-110"></i>
                                        Upload
                                    </a>
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
					</div>
				</div>

				<div class="widget-body">
					<div class="widget-main">

                        <div class="form-horizontal" role="form">

                            <div class="clearfix">
                                <div class="pull-right tableTools-container"></div>
                            </div>

                            <asp:GridView ID="gvOpenbudget" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover"
                                DataKeyNames="BudgetAccount" OnRowCommand="gvOpenbudget_RowCommand" OnRowDataBound="gvOpenbudget_RowDataBound"
                                OnPreRender="gvOpenbudget_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="BudgetAccount" HeaderStyle-CssClass="treecontainer" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnExpand" Font-Underline="false" runat="server" CommandName="Expand"
                                                CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="BudgetType" HeaderText="BudgetType" />
                                    <asp:BoundField DataField="BudgetAccKey" HeaderText="BudgetAccKey" />
                                    <asp:BoundField DataField="BudgetLedgerKey" HeaderText="BudgetLedgerKey" />
                                    <asp:BoundField DataField="BudgetYear" HeaderText="BudgetYear" />
                                    <asp:BoundField DataField="Type" HeaderText="Type" />
                                    <asp:BoundField DataField="BudgetAmount" HeaderText="BudgetAmount" />
                                   <%-- <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span id="CustomStatus" runat="server"></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <div class="btn-group">
                                                <button data-toggle="dropdown" class="btn btn-xs btn-light dropdown-toggle">
                                                    <span class="ace-icon fa fa-caret-down icon-only"></span>
                                                </button>
                                                <ul class="dropdown-menu dropdown-info dropdown-menu-right">
                                                    <li>
                                                        <asp:LinkButton ID="lbEit" runat="server" CommandName="CmdEdit" CommandArgument='<%# Container.DataItemIndex %>'
                                                            OnClientClick="ShowEditForm();">
											            <span><i class="fa fa-edit"></i></span> Edit
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbDelete" runat="server" CommandName="CmdDelete"
                                                            CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-trash-o"></i></span> Delete
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lbCut" runat="server" CommandName="CmdCut" CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-cut"></i></span> Cut
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbPaste" runat="server" CommandName="CmdPaste" CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-paste"></i></span> Paste
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li class="divider"></li>
                                                    <li>
                                                        <asp:LinkButton ID="lbAddItem" runat="server" Visible="false" CommandName="AddItem"
                                                            CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-plus"></i></span> Add Root
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbMakeRoot" runat="server" CommandName="MakeRoot"
                                                            CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-plus"></i></span> Make Root
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbAddChild" runat="server" CommandName="AddChild" OnClientClick="ShowEditForm();"
                                                            CommandArgument='<%# Container.DataItemIndex %>'>
											            <span><i class="fa fa-level-down"></i></span> Add Child
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                            <!-- /.btn-group -->
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

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">

        function ShowEditForm() {
            spinnerInit();
            ace.data.remove('demo', 'widget-state');
            ace.data.remove('demo', 'widget-order');
            $("#EditForm").show();
        }

        function InitScript() {
            // widget box drag & drop
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

            //edit-form onreloaded
            $('#widget-box-edit').on('reloaded.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');
                $('#btnCancel').click();
            });

            //edit-form onclosed
            $('#widget-box-edit').on('closed.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');
                $('#btnCancel').click();

                $("#MainContent_gvOpenbudget tr").each(function () {
                    $(this).css("background-color", "");
                });

                $.ajax({
                    type: "POST",
                    url: "PeruntukanAsalSetup.aspx/ReloadField?f=widgetclosed",
                    data: "{}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        console.log("Field Reloaded!");
                    }
                });
            });

            //acc code list onreloaded
            $('#widget-box-list').on('reloaded.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');

                location.href = "<%=Page.ResolveUrl("~/PeruntukanAsalSetup.aspx")%>";
            });

            //preview template
            $('[data-toggle="popover"]').popover({
                container: 'body',
                html: true,
                placement: 'bottom',
                trigger: 'hover',
                content: function () {
                    // get the url for the full size img
                    var url = $(this).data('full');
                    return '<img src="' + url + '">'
                }
            });

            $('[data-rel=tooltip]').tooltip();
        }

        function FileInput() {
            var file_input = $('#upload');

            file_input.ace_file_input({
                style: 'well',
                btn_choose: 'Drop files here or click to choose',
                btn_change: null,
                no_icon: 'ace-icon fa fa-cloud-upload',
                droppable: true,
                thumbnail: 'small',
                allowExt: ['csv', 'CSV', 'xls', 'XLS', 'xlsx', 'XLSX']
            });

            $('#btnUpload').on('click', function (e) {
                spinnerInit();

                if (!file_input.data('ace_input_files')) {
                    $('#spin').data('spinner').stop();
                    $("#spin").hide();

                    alert("Upload Fail - No file selected!");
                    return false;
                }

                var fd = new FormData();
                fd.append('upload', $('#upload')[0].files[0]);

                if (file_input.data('ace_input_method') == 'drop') {
                    var files = file_input.data('ace_input_files');
                    if (files && files.length > 0) {
                        fd.append(file_input.attr('name'), files[0]);
                    }
                }

                $.ajax({
                    url: 'FileUploadHandler.ashx?source=PeruntukanAsal',
                    type: 'post',
                    data: fd,
                    success: fnsuccesscallback,
                    processData: false,
                    contentType: false,
                    error: function (response) {
                        alert(response.d.result);
                    }
                });

                function fnsuccesscallback(response) {
                    $('#spin').data('spinner').stop();
                    $("#spin").hide();

                    var res = JSON.parse(JSON.stringify(response));
                    $.each(res, function (i, item) {
                        var classname;
                        if (item.status.indexOf("Error") >= 0 || item.status.indexOf("Failure") >= 0) {
                            classname = 'gritter-error';
                        }
                        if (item.status.indexOf("Success") >= 0) { classname = 'gritter-success'; }

                        $.gritter.add({
                            title: item.status,
                            text: item.message,
                            class_name: classname,
                            sticky: false,
                            time: 60000,
                            after_close: function () {
                                if (item.status.indexOf("Success") >= 0) {
                                    location.href = "<%=Page.ResolveUrl("~/PeruntukanAsalSetup.aspx")%>";
                                }
                            }
                        });
                    });
                }

            });
        }

        function LoadDataTable() {
            //initiate dataTables plugin
            var myTable = $('#<%=gvOpenbudget.ClientID%>').DataTable({
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
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2]
                      }
                  },
                  {
                      "extend": "csvHtml5",
                      "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [0, 1, 2],
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
                          columns: [0, 1, 2],
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
                          columns: [0, 1, 2],
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
                          columns: [0, 1, 2]
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

            setTimeout(function () {
                $($('.tableTools-container')).find('a.dt-button').each(function () {
                    var div = $(this).find(' > div').first();
                    if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                    else $(this).tooltip({ container: 'body', title: $(this).text() });
                });
            }, 500);

            $(document).on('click', '#<%=gvOpenbudget.ClientID%> .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
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
                args.isValid = true;
                $(control).closest(".form-group").removeClass("has-error");

                return;
            }
        }

        function Reset() {
            //Page_ClientValidate('');

            if (typeof (Page_Validators) != "undefined") {
                for (var i = 0; i < Page_Validators.length; i++) {
                    var validator = Page_Validators[i];
                    validator.isvalid = true;
                    ValidatorUpdateDisplay(validator);

                    $('#edit-form').find('.form-group').each(function () {
                        $(this).removeClass("has-error");
                    })
                }
            }
        }

        $(document).ready(function () {
            InitScript();
            FileInput();
            LoadDataTable();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            InitScript();
            FileInput();
            LoadDataTable();
        });

    </script>

</asp:Content>
