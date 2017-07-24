<%@ Page Title="Segment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SegmentSetup.aspx.cs" Inherits="BP.SegmentSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
        <li class=""><a href="#">Setup</a></li>
        <li class="active">Segment</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <div class="page-header">
		<h1>
			Segment
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				setup &amp; manage segment
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
					<h5 id="widget_title" class="widget-title" runat="server">Segment - New</h5>

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

                        <div id="edit-form" class="form-horizontal" role="form">
                            <br />
                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="tbSegName">Segment Name:</label>

                                <div class="col-xs-12 col-sm-9">
                                    <div class="clearfix">
                                        <asp:TextBox ID="tbSegName" runat="server" CssClass="col-xs-12 col-sm-6"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="SegmentNameRequired" runat="server" ControlToValidate="tbSegName" Display="Dynamic"
                                        CssClass="help-block" ErrorMessage="Segment Name is required." ValidationGroup="SaveValidation" />
                                    <asp:CustomValidator ID="CustomVal1" runat="server" ClientValidationFunction="CustomValidationFunction" Display="None"
                                        ValidateEmptyText="True" ValidationGroup="SaveValidation" ControlToValidate="tbSegName" SetFocusOnError="true" />
                                </div>
                            </div>

                            <div class="space-2"></div>

                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="tbSegFormat">Shape Format:</label>

                                <div class="col-xs-12 col-sm-9">
                                    <div class="clearfix">
                                        <asp:TextBox ID="tbSegFormat" runat="server" CssClass="col-xs-12 col-sm-6" onkeypress="return IsQuestionKey(event);"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="ShapeFormatRequired" runat="server" ControlToValidate="tbSegFormat" Display="Dynamic"
                                        CssClass="help-block" ErrorMessage="Shape Format is required." ValidationGroup="SaveValidation" />
                                    <asp:CustomValidator ID="CustomVal2" runat="server" ClientValidationFunction="CustomValidationFunction" Display="None"
                                        ValidateEmptyText="True" ValidationGroup="SaveValidation" ControlToValidate="tbSegFormat" SetFocusOnError="true" />
                                </div>
                            </div>

                            <div class="space-2"></div>

                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="tbSegOrder">Segment Order:</label>

                                <div class="col-xs-12 col-sm-9">
                                    <div class="clearfix">
                                        <asp:TextBox ID="tbSegOrder" runat="server" CssClass="col-xs-12 col-sm-6" onkeypress="return IsNumberKey(event);"></asp:TextBox>
                                    </div>
                                    <asp:RequiredFieldValidator ID="SegmentOrderRequired" runat="server" ControlToValidate="tbSegOrder" Display="Dynamic"
                                        CssClass="help-block" ErrorMessage="Segment Order is required." ValidationGroup="SaveValidation" />
                                    <asp:CustomValidator ID="CustomVal3" runat="server" ClientValidationFunction="CustomValidationFunction" Display="None"
                                        ValidateEmptyText="True" ValidationGroup="SaveValidation" ControlToValidate="tbSegOrder" SetFocusOnError="true" />
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

                            <div class="space-10"></div>

                            <div class="form-group">
                                <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="cdaccflag">
                                    Enable/Disable <br /><small class="red">Account Code Flag</small>:
                                </label>

                                <div class="col-xs-12 col-sm-9" style="margin-top:15px;">
                                    <div class="clearfix">
                                        <label>
                                            <input id="cdaccflag" name="cdaccflag" runat="server" class="ace ace-switch ace-switch-6" type="checkbox" />
                                            <span class="lbl"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="space-2"></div>

                            <div class="clearfix form-actions">
                                <div class="col-md-offset-9 col-md-9">

                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-info" ValidationGroup="SaveValidation" OnClick="btnSave_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Save
                                    </asp:LinkButton>
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
					<h5 class="widget-title">Segment - List</h5>

					<div class="widget-toolbar">
                        <div class="widget-menu">
                            <a href="#" data-action="settings" data-toggle="dropdown">
							    <i class="ace-icon fa fa-cogs"></i>
						    </a>

						    <ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
							    <li> 
                                    <asp:LinkButton ID="btnAdd" runat="server" CssClass="blue" OnClientClick="ShowForm();" OnClick="btnAdd_Click">
                                        <i class="ace-icon fa fa-briefcase"></i>&nbsp;&nbsp;Add Segment
                                    </asp:LinkButton>
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

                            <asp:GridView ID="gvSegmentSetup" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover"
                                DataKeyNames="SegmentID" OnRowCommand="gvSegmentSetup_RowCommand" OnRowDataBound="gvSegmentSetup_RowDataBound"
                                OnPreRender="gvSegmentSetup_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="1px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEditDetails" runat="server" CommandName="EditDetails" CommandArgument='<%# Container.DataItemIndex %>'>
                                                <span><i class="ace-icon glyphicon glyphicon-info-sign red " data-rel="popover" title="Show Segment Details" 
                                                    data-content="Heads up! This button leads to Segment Details."></i>
                                                </span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SegmentName" HeaderText="Segment Name" />
                                    <asp:BoundField DataField="ShapeFormat" HeaderText="Shape Format" />
                                    <asp:BoundField DataField="SegmentOrder" HeaderText="Segment Order" />
                                    <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <span id="CustomStatus" runat="server"></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <div class="hidden-sm hidden-xs btn-group">
                                                <asp:LinkButton ID="btnEditRow" runat="server" OnClientClick="ShowForm();" CommandName="EditRow" 
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

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">

        function ShowForm() {
            spinnerInit();
            ace.data.remove('demo', 'widget-state');
            ace.data.remove('demo', 'widget-order');
            $("#<%=EditForm.ClientID%>").show();
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

                $("#MainContent_gvSegmentSetup tr").each(function () {
                    $(this).css("background-color", "");
                });
            });

            //acc code list onreloaded
            $('#widget-box-list').on('reloaded.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');

                location.href = "<%=Page.ResolveUrl("~/SegmentSetup.aspx")%>";
            });

            //info button - to segment details page
            $('[data-rel="popover"]').popover({
                html: true,
                placement: 'top',
                trigger: 'hover'
            });
        }

        function LoadDataTable() {
            //initiate dataTables plugin
            var myTable = $('#<%=gvSegmentSetup.ClientID%>').DataTable({
                bAutoWidth: false,
				"lengthMenu": [[20, 40, 60, -1], [20, 40, 60, "All"]],
                "aoColumns": [
					{ "bSortable": false },
					  null,
                      null,
                      null,
                      null,
                    { "bSortable": false }
                ],
                "aaSorting": [],
                select: {
                    style: 'single'
                }
            });

            $.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

            new $.fn.dataTable.Buttons(myTable, {
                buttons: [
                  {
                      "extend": "colvis",
                      "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      columns: ':not(:first), :not(:last)'
                  },
                  {
                      "extend": "copyHtml5",
                      "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [1, 2, 3, 4]
                      }
                  },
                  {
                      "extend": "csvHtml5",
                      "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          columns: [1, 2, 3, 4],
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
                          columns: [1, 2, 3, 4],
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
                          columns: [1, 2, 3, 4],
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
                          columns: [1, 2, 3, 4]
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

            $(document).on('click', '#<%=gvSegmentSetup.ClientID%> .dropdown-toggle', function (e) {
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
            LoadDataTable();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            InitScript();
            LoadDataTable();
        });

    </script>

</asp:Content>
