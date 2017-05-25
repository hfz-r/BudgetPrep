<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SegmentSetup.aspx.cs" Inherits="BP.Setup.SegmentSetup" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
	    <li>
		    <i class="ace-icon fa fa-home home-icon"></i>
		    <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
	    </li>
	    <li class="">Setup</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
     <div class="page-header">
		<h1>
			Setup
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				Segment 
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
    <div id="dvSegment" runat="server" visible="false">
        <div class="col-xs-12 widget-container-col ui-sortable" id="widget-container-col-2">
			<div class="widget-header">
				<h6 class="widget-title">Segment</h6>
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
                    <div class="row">
                        <div class="col-xs-12">
                            <form id="frmSegment" class="form-horizontal" role="form" method="get">
                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Segment Name</label>
                                    <div class="col-sm-9">
										<input type="text" id="txtSegmentName" class="col-xs-10 col-sm-5" runat="server" onkeypress="return NoSpaceKey(event);" />
									</div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Shape Format</label>
                                    <div class="col-sm-9">
										<input type="text" id="txtShapeFormat" class="col-xs-10 col-sm-5" runat="server" onkeypress="return NoSpaceKey(event);" />
									</div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-3 control-label no-padding-right" for="form-field-1">Segment Order</label>
                                    <div class="col-sm-9">
										<input type="text" id="txtSegmentOrder" class="col-xs-10 col-sm-5" runat="server" onkeypress="return NoSpaceKey(event);" />
									</div>
                                </div>
                                <div class="form-group">
								    <label class="control-label col-xs-12 col-sm-3 no-padding-right" for="status">Status</label>
                                                        
                                    <div class="col-xs-12 col-sm-9">
									    <div class="clearfix">
										    <asp:DropDownList ID="ddlStatus" name="ddlStatus" runat="server" CssClass="col-xs-12 col-sm-4">
										    </asp:DropDownList>
									    </div>
								    </div>
								</div>
                                <hr />
								<div style="text-align:center">
									<button class="btn btn-info" type="button">
										<i class="ace-icon fa fa-check bigger-110"></i>
										Save
									</button>

									&nbsp; &nbsp; &nbsp;
									<button class="btn" type="reset">
										<i class="ace-icon fa fa-undo bigger-110"></i>
										Cancel
									</button>
								</div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="col-xs-12 widget-container-col ui-sortable" id="widget-container-col-1">
		<div class="widget-box ui-sortable-handle" id="widget-box-1">
			<div class="widget-header">
				<h6 class="widget-title">View</h6>
				<div class="widget-toolbar">
					<div class="widget-menu">
						<a href="#" data-action="settings" data-toggle="dropdown" aria-expanded="false">
			                <i class="ace-icon fa fa-cogs"></i>
						</a>

						<ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
							<li>
                                <asp:LinkButton ID="btnAdd" runat="server" PostBackUrl="~/Setup/SegmentSetup.aspx" CssClass="blue" OnClick="btnAdd_Click">
                                    <i class="ace-icon fa fa-user-plus"></i>&nbsp;&nbsp;Add Segment
                                </asp:LinkButton>
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
				</div>
			</div>

            <div class="widget-body">
		        <div class="widget-main">
                        <div class="clearfix">
						    <div class="pull-right tableTools-container"></div>
                        </div>
                        <asp:UpdatePanel ID="pnlSegmentSetup" UpdateMode="Conditional" ChildrenAsTriggers="true" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvSegmentSetup" runat="server" AutoGenerateColumns="false" AllowSorting="true"
                                    CssClass="table table-bordered table-striped table-hover" DataKeyNames="SegmentID"
                                    OnRowCommand="gvSegmentSetup_RowCommand"
                                    OnRowDataBound="gvSegmentSetup_RowDataBound"
                                    OnPreRender="gvSegmentSetup_PreRender">
                                    <Columns>
                                        <asp:BoundField DataField="SegmentName" HeaderText="Segment Name" />
                                        <asp:BoundField DataField="ShapeFormat" HeaderText="Shape Format" />
                                        <asp:BoundField DataField="SegmentOrder" HeaderText="Segment Order" />
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <span id="Status" runat="server"></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div class="hidden-sm hidden-xs btn-group">
                                                    <asp:LinkButton ID="btnEditRow" CssClass="btn btn-white btn-minier btn-bold" runat="server" CommandName="EditRow" CommandArgument='<%# Container.DataItemIndex %>'>
                                                        <i class="ace-icon glyphicon glyphicon-edit blue"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <div class="hidden-sm hidden-xs btn-group">
                                                    <asp:LinkButton ID="btnEditDetails" runat="server" CommandName="EditDetails" CommandArgument='<%# Container.DataItemIndex %>'>
                                                        <i class="fa fa-info-circle"></i>
                                                    </asp:LinkButton>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <button id="btnSegmentSetup" runat="server" style="visibility:hidden;"></button>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btnSegmentSetup" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
<%--                    <table id="dynamic-table" class="table table-striped table-bordered table-hover">
                       <thead>
                          <tr class="gridStyle">
                             <th>Segment Name</th>
                             <th>Shape Format</th>
                             <th>Segment Order</th>
                             <th>Status</th>
                          </tr>
                       </thead>
                       <tbody></tbody>
                    </table>--%>
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

                    $("#MainContent_gvSegmentSetup tr").each(function () {
                        $(this).css("background-color", "");
                    });
                });

                ////btn add new user
                //$('#MainContent_btnAdd').on('click', function (e) {
                //    spinnerInit();
                //    ace.data.remove('demo', 'widget-state');
                //    ace.data.remove('demo', 'widget-state');
                //    $('#MainContent_form_Wiz').show();
                //});

                //user lists onreloaded
                $('#widget-box-1').on('reloaded.ace.widget', function (event) {
                    ace.data.remove('demo', 'widget-state');
                    ace.data.remove('demo', 'widget-order');
                    //$('form').each(function () { this.reset() });

                    var myTable = $('#<%=gvSegmentSetup.ClientID%>').DataTable();
                    myTable.state.clear();

                    window.location.href = "<%=Page.ResolveUrl("~/Setup/SegmentSetup.aspx")%>";
                });

                //disable selection
                $('#validation-form, #validation-form2, #UserListWidget, .widget-toolbar')
                    .bind('mousedown.ui-disableSelection selectstart.ui-disableSelection', function (event) {
                        event.stopImmediatePropagation();
                    });

            })();
        });

    </script>

    <script type="text/javascript">
        jQuery(function($) {
            var gvSegmentSetup = $('#<%=gvSegmentSetup.ClientID%>').DataTable({
                bAutoWidth: false,
                "aoColumns": [
					  null,
                      null,
                      null,
                      null,
                      null,
                    { "bSortable": false }
                ],
                "columnDefs": [ {
                    "targets": 4,
                    "orderable": false
                }],
                "aaSorting": [],
                select: {
                    style: 'multi'
                }
            });

            $.fn.dataTable.Buttons.defaults.dom.container.className = 'dt-buttons btn-overlap btn-group btn-overlap';

            new $.fn.dataTable.Buttons(gvSegmentSetup, {
                buttons: [
                  {
                      "extend": "colvis",
                      "text": "<i class='fa fa-search bigger-110 blue'></i> <span class='hidden'>Show/hide columns</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      columns: ':lt(4)'
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
            gvSegmentSetup.buttons().container().appendTo($('.tableTools-container'));

            //style the message box
            var defaultCopyAction = gvSegmentSetup.button(1).action();
            gvSegmentSetup.button(1).action(function (e, dt, button, config) {
                defaultCopyAction(e, dt, button, config);
                $('.dt-button-info').addClass('gritter-item-wrapper gritter-info gritter-center white');
            });

            var defaultColvisAction = gvSegmentSetup.button(0).action();
            gvSegmentSetup.button(0).action(function (e, dt, button, config) {

                defaultColvisAction(e, dt, button, config);

                if ($('.dt-button-collection > .dropdown-menu').length == 0) {
                    $('.dt-button-collection')
                    .wrapInner('<ul class="dropdown-menu dropdown-light dropdown-caret dropdown-caret" />')
                    .find('a').attr('href', '#').wrap("<li />")
                }
                $('.dt-button-collection').appendTo('.tableTools-container .dt-buttons')
            });

            ////

            setTimeout(function () {
                $($('.tableTools-container')).find('a.dt-button').each(function () {
                    var div = $(this).find(' > div').first();
                    if (div.length == 1) div.tooltip({ container: 'body', title: div.parent().text() });
                    else $(this).tooltip({ container: 'body', title: $(this).text() });
                });
            }, 500);

            gvSegmentSetup.on('select', function (e, dt, type, index) {
                if (type === 'row') {
                    $(gvSegmentSetup.row(index).node()).find('input:checkbox').prop('checked', true);
                }
            });
            gvSegmentSetup.on('deselect', function (e, dt, type, index) {
                if (type === 'row') {
                    $(gvSegmentSetup.row(index).node()).find('input:checkbox').prop('checked', false);
                }
            });

            /////////////////////////////////
            //table checkboxes
            $('th input[type=checkbox], td input[type=checkbox]').prop('checked', false);
            $('#<%=gvSegmentSetup.ClientID%>')
            //select/deselect all rows according to table header checkbox
            $('#<%=gvSegmentSetup.ClientID%> > thead > tr > th input[type=checkbox], #<%=gvSegmentSetup.ClientID%>_wrapper input[type=checkbox]').eq(0).on('click', function () {
                var th_checked = this.checked;//checkbox inside "TH" table header

                $('#<%=gvSegmentSetup.ClientID%>').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) gvSegmentSetup.row(row).select();
                    else gvSegmentSetup.row(row).deselect();
                });
            });

            //select/deselect a row when the checkbox is checked/unchecked
            $('#<%=gvSegmentSetup.ClientID%>').on('click', 'td input[type=checkbox]', function () {
                var row = $(this).closest('tr').get(0);
                if (this.checked) gvSegmentSetup.row(row).deselect();
                else gvSegmentSetup.row(row).select();
            });

            $(document).on('click', '#<%=gvSegmentSetup.ClientID%> .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });
        })
    </script>
</asp:Content>
