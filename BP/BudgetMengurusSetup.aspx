<%@ Page Title="Budget Mengurus" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetMengurusSetup.aspx.cs" Inherits="BP.BudgetMengurusSetup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <style>
        body .popover {
            max-width: 830px;
        }

        .treecontainer {
            min-width: 200px;
            white-space: nowrap;
            overflow-wrap: inherit;
        }

        .DDlPanel {
            position: fixed;
            z-index: 9999999;
            visibility: hidden;
        }

        .PeriodGrid {
            margin-top: -21px;
        }

        fieldset.scheduler-border {
            border: 1px groove #ddd !important;
            padding: 0 1.4em 1.4em 1.4em !important;
            margin: 0 0 1.5em 0 !important;
            -webkit-box-shadow: 0px 0px 0px 0px #000;
            box-shadow: 0px 0px 0px 0px #000;
        }

        legend.scheduler-border {
            font-size: 1.2em !important;
            font-weight: bold !important;
            text-align: left !important;
            width: auto;
            padding: 0 10px;
            border-bottom: none;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
        <li>
            <i class="ace-icon fa fa-home home-icon"></i>
            <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
        </li>
        <li class=""><a href="#">Budget</a></li>
        <li class="active">Mengurus</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <div class="page-header">
		<h1>
			Budget Mengurus
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				setup &amp; manage budget mengurus
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <!-- Edit Form -->
    <div id="EditForm" runat="server">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-2">
            <div class="widget-box widget-color-dark" id="widget-box-edit">
                <div class="widget-header widget-header-small">
                    <h6 id="widget_title" class="widget-title" runat="server">
                        <i class="ace-icon fa fa-search"></i>
                        Budget Mengurus - Search
                    </h6>

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
                    <div class="widget-main padding-24">

                        <div id="edit-form" class="form-horizontal" role="form">
                            <script type="text/javascript">

                                function cancelClick(e) {
                                    debugger;
                                    if (e.stopPropagation) e.stopPropagation();
                                    e.cancelBubble = true;
                                }

                            </script>

                            <div class="row">

                                <div class="col-sm-6" style="overflow: auto; min-height: 300px; max-height: 300px;">
                                    <div class="row">
                                        <div class="col-xs-11 label label-lg label-info arrowed-in arrowed-right">
                                            <b>Segments</b>
                                        </div>
                                    </div>

                                    <div class="col-xs-11">
                                        <asp:GridView ID="gvSegmentDLLs" runat="server" AutoGenerateColumns="false" AllowSorting="true" ShowHeader="false"
                                            CssClass="table table-bordered table-striped table-hover" DataKeyNames="SegmentID" OnRowDataBound="gvSegmentDLLs_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="SegmentName" HeaderText="Segment" />
                                                <asp:TemplateField HeaderText="Value" HeaderStyle-CssClass="treecontainer" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:TextBox 
                                                            ID="tbSegmentDDL" 
                                                            runat="server" 
                                                            CssClass="form-control" 
                                                            placeholder="Please Select" />
                                                        <asp:RequiredFieldValidator 
                                                            ID="RequiredFieldValidator1" 
                                                            runat="server" 
                                                            ControlToValidate="tbSegmentDDL" 
                                                            Display="Dynamic"
                                                            CssClass="help-block"
                                                            ForeColor="Red" 
                                                            ErrorMessage="Segment is required." 
                                                            ValidationGroup="SearchValidation" />
                                                        <ajaxToolkit:DropDownExtender ID="DropDownExtender1" runat="server" TargetControlID="tbSegmentDDL"
                                                            DropDownControlID="pnlSegmentDDL"></ajaxToolkit:DropDownExtender>
                                                        <asp:Panel ID="pnlSegmentDDL" runat="server" OnClientClick="cancelClick(event)"
                                                            BackColor="#e8e8e8" CssClass="DDlPanel" Width="50%" Height="200px" ScrollBars="Auto">
                                                            <asp:TreeView ID="tvSegmentDDL" runat="server" BackColor="#e8e8e8" OnClientClick="cancelClick(event)"
                                                                OnSelectedNodeChanged="tvSegmentDDL_SelectedNodeChanged">
                                                            </asp:TreeView>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div><!-- /.col -->

                                <div class="col-sm-6" style="overflow: auto; max-height: 300px;">
                                    <div class="row">
                                        <div class="col-xs-11 label label-lg label-success arrowed-in arrowed-right">
                                            <b>Period</b>
                                        </div>
                                    </div>

                                    <div class="col-xs-11">
                                        <table class="table table-bordered table-striped">
                                            <tr style="display: none;">
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="chkKeterangan" runat="server" /></td>
                                                <td>Keterangan</td>
                                                <td style="text-align: center;">
                                                    <asp:CheckBox ID="chkPengiraan" runat="server" /></td>
                                                <td>Pengiraan</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 70px; text-align: center;">
                                                    <asp:CheckBox ID="chkMedan" runat="server" AutoPostBack="true" OnCheckedChanged="chkMedan_CheckedChanged" /></td>
                                                <td colspan="3">Medan</td>
                                            </tr>
                                        </table>
                                        <asp:GridView ID="gvPeriod" runat="server" AutoGenerateColumns="false" AllowSorting="true" ShowHeader="false"
                                            CssClass="table table-bordered table-striped PeriodGrid" DataKeyNames="PeriodMengurusID"
                                            OnRowDataBound="gvPeriod_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Value" HeaderStyle-Width="70px" ItemStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbPeriodSelect" runat="server"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="MengurusYear" HeaderText="Mengurus Year" />
                                                <asp:BoundField DataField="FieldMengurusDesc" HeaderText="Description" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div><!-- /.col -->

                            </div>

                            <div class="clearfix form-actions">
                                <div class="col-md-offset-9 col-md-9">
                                    <asp:LinkButton ID="btnSearch" runat="server" CssClass="btn btn-info" ValidationGroup="SearchValidation" OnClick="btnSearch_Click">
                                        <i class="ace-icon fa fa-check bigger-110"></i>
                                        Search
                                    </asp:LinkButton>

                                    <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn" OnClientClick="return Reset();" OnClick="btnCancel_Click">
                                        <i class="ace-icon fa fa-undo bigger-110"></i>Reset
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- List Form -->
    <div id="ListForm" runat="server" visible="false">
        <div class="col-xs-12 widget-container-col" id="widget-container-col-1">
            <div class="widget-box widget-color-dark" id="widget-box-list">
                <div class="widget-header widget-header-small">
                    <h6 class="widget-title">Budget Mengurus - List</h6>

                    <div class="widget-toolbar">
                        <div class="widget-menu">
                            <a href="#" data-action="settings" data-toggle="dropdown" class="tooltip-info" data-rel="tooltip" data-placement="top" title="Settings">
                                <i class="ace-icon fa fa-cogs"></i>
                            </a>

                            <ul class="dropdown-menu dropdown-menu-right dropdown-light-blue dropdown-caret dropdown-closer">
                                <li>
                                    <a href="#LegendExample" id="btnLegend" runat="server" role="button" class="red" data-toggle="collapse" data-rel="tooltip" 
                                        data-placement="top" title="Legend">
                                        <i class="ace-icon fa fa-list bigger-110"></i>
                                        Legend
                                    </a>
                                </li>
                                <li>
                                    <a href="#upload-form" id="btnFileUpload" runat="server" role="button" class="blue" data-toggle="modal" data-rel="tooltip" 
                                        data-placement="top" title="Upload your .CSV file format here.">
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

                    <div class="widget-toolbar">
                        <button id="btnSearchbox" runat="server" class="badge badge-grey" onserverclick="btnSearchbox_Click">
                           <i class="icon-only ace-icon fa fa-list txt-primary bigger-110"></i>
                            Filter
                        </button>
                    </div>
                </div>

                <div class="widget-body">

                    <div class="form-horizontal" role="form">

                        <!-- LegendExample - start -->
                        <div class="collapse" id="LegendExample">
                            <div class="well">
                                <fieldset class="scheduler-border">
                                    <legend class="scheduler-border">Legend</legend>
                                    <div class="row">
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #999999;">Saved</div>
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #ffff00;">Prepared</div>
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #00ffff;">Reviewed</div>
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #00ff00;">Approved</div>
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #ff00ff;">Reviewer Rejected</div>
                                        <div class="col-lg-2 col-md-4 col-sm-4" style="background-color: #ff0000;">Approver Rejected</div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <!-- LegendExample - end -->

                        <div class="widget-main padding-16">
                            <div class="clearfix">
                                <div class="pull-right tableTools-container"></div>
                            </div>

                            <asp:GridView ID="gvAccountCodes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered table-striped table-hover"
                                DataKeyNames="AccountCode" Visible="true" ShowFooter="true" OnRowCommand="gvAccountCodes_RowCommand"
                                OnRowDataBound="gvAccountCodes_RowDataBound" OnRowCreated="gvAccountCodes_RowCreated" OnPreRender="gvAccountCodes_PreRender">
                                <FooterStyle Font-Bold="true" BackColor="LightGray" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Code" HeaderStyle-CssClass="treecontainer" ItemStyle-HorizontalAlign="Left"
                                        ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnExpand" runat="server" Font-Underline="false" CommandName="Expand" 
                                                CommandArgument='<%# Container.DataItemIndex %>'>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <div class="clearfix form-actions" style="margin-top:20px;">
                                <div class="col-md-offset-9 col-md-9">
                                    <button id="btnSubmit" runat="server" class="btn btn-warning" type="button" data-toggle="modal" data-target="#BulkSubmitExample" />
                                    <button id="btnReload" runat="server" type="button" class="btn" onclick="return location.reload();">
                                        <i class="ace-icon fa fa-times bigger-110"></i>Cancel
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Bulk Submit - start -->
    <div id="BulkSubmitExample" class="modal fade" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Budget Mengurus - Bulk Submit</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-10 col-lg-push-1 col-sm-12">
                            <div class="col-md-12 col-lg-4">
                                <label class="control-label">Decision</label>
                            </div>
                            <div class="col-md-12 col-lg-8">
                                <asp:DropDownList ID="ddlBulkDecision" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Accept" />
                                    <asp:ListItem Text="Reject" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-lg-10 col-lg-push-1 col-sm-12">
                            <div class="col-md-12 col-lg-4">
                                <label class="control-label">Remarks</label>
                            </div>
                            <div class="col-md-12 col-lg-8">
                                <asp:TextBox ID="tbBulkRemarks" runat="server" TextMode="MultiLine" Height="60px" Text="" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:LinkButton ID="btnBulkUpdate" runat="server" CssClass="btn btn-success" OnClientClick="return CloseModal();" OnClick="btnBulkUpdate_Click">
						<i class="ace-icon fa fa-save"></i>Save changes
					</asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Bulk Submit - end -->

    <!-- Decision - start -->
    <div class="modal fade" id="myDecisionModal" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblDecisionModalTitle" runat="server" Text="Budget Mengurus - Decision"></asp:Label></h4>
                </div>
                <div class="modal-body">
                    <asp:Label ID="lblDecisionModalPeriodID" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                    <asp:Label ID="lblDecisionModalAccountCode" runat="server" Text="" class="control-label" Visible="false"></asp:Label><br />
                    <asp:Label ID="lblDecisionModalAccount" runat="server" Text="" class="control-label"></asp:Label><br />
                    <asp:Label ID="lblDecisionModalPeriod" runat="server" Text="" class="control-label"></asp:Label><br />
                    <asp:Label ID="lblDecisionModalAmount" runat="server" Text="" class="control-label"></asp:Label><br />
                    <hr />
                    <asp:RadioButtonList ID="rbldecision" runat="server">
                        <asp:ListItem Text="Accept" Value="A" Selected="True" />
                        <asp:ListItem Text="Reject" Value="R" />
                    </asp:RadioButtonList>
                    <asp:Label ID="Label1" runat="server" Text="Remarks" class="control-label"></asp:Label>
                    <asp:TextBox ID="tbRemarks" runat="server" TextMode="MultiLine" Height="60px" Text="" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnDecisionSave" runat="server" CssClass="btn btn-label-left"
                        OnClientClick="return CloseModal();" OnClick="btnDecisionSave_Click">
                            <span><i class="fa fa-save txt-success"></i></span>Simpan
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <!-- Decision Modal - end -->

    <!-- upload box -->
    <div id="upload-form" class="modal" tabindex="-1">
        <div class="modal-dialog" style="width: 400px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="blue bigger">Budget Mengurus File Upload</h5>
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
                    <a class="btn btn-sm btn-success pull-left" data-toggle="popover" title="Template Preview - Budget Mengurus"
                        data-full="<%=Page.ResolveUrl("~/Images/BP/budget_mengurus_template.png")%>" onclick="return false;">Preview Sample
                    </a>

                    <button id="btnResetFile" type="reset" class="btn btn-sm">
                        <i class="ace-icon fa fa-times"></i>
                        Reset
                    </button>

                    <button id="btnUpload" class="btn btn-sm btn-primary" data-dismiss="modal">
                        <i class="ace-icon fa fa-cloud-upload"></i>
                        Upload
                    </button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <script type="text/javascript">

        function CloseModal() {
            $('#myDecisionModal').modal('hide');
            $('#BulkSubmitExample').modal('hide');
            return true;
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

                $("#MainContent_gvAccountCodes tr").each(function () {
                    $(this).css("background-color", "");
                });
            });

            //acc code list onreloaded
            $('#widget-box-list').on('reloaded.ace.widget', function (event, info) {
                ace.data.remove('demo', 'widget-state');
                ace.data.remove('demo', 'widget-order');

                location.href = "<%=Page.ResolveUrl("~/BudgetMengurusSetup.aspx")%>";
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
                allowExt: ['csv', 'CSV']
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
                    url: 'FileUploadHandler.ashx?source=BudgetMengurus',
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
                            time: 30000,
                            after_close: function () {
                                if (item.status.indexOf("Success") >= 0) {
                                    location.href = "<%=Page.ResolveUrl("~/BudgetMengurusSetup.aspx")%>";
                                }
                            }
                        });
                    });
                }

            });
        }

        function LoadDataTable(cols)
        {
            spinnerInit();

            var myTable = $('#<%=gvAccountCodes.ClientID%>').DataTable({
                bAutoWidth: false,
                "aoColumnDefs": [
                    {
                    "targets": 0,
                    "bSortable": false
                    }
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
                      columns: ':not(:first)'
                  },
                  {
                      "extend": "copyHtml5",
                      "text": "<i class='fa fa-copy bigger-110 pink'></i> <span class='hidden'>Copy to clipboard</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          modifier: {
                              page: 'current'
                          }
                      }
                  },
                  {
                      "extend": "csvHtml5",
                      "text": "<i class='fa fa-database bigger-110 orange'></i> <span class='hidden'>Export to CSV</span>",
                      "className": "btn btn-white btn-primary btn-bold",
                      exportOptions: {
                          modifier: {
                              page: 'current'
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
                      message: 'This print was produced using the Print button for DataTables',
                      exportOptions: {
                          modifier: {
                              page: 'current'
                          }
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

                $('#spin').data('spinner').stop();
                $("#spin").hide();

            }, 500);

            $(document).on('click', '#<%=gvAccountCodes.ClientID%> .dropdown-toggle', function (e) {
                e.stopImmediatePropagation();
                e.stopPropagation();
                e.preventDefault();
            });
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
            FileInput();

            $('input[type=a], label').click(function (e) {
                if (!e) var e = window.event;
                e.cancelBubble = true;
                if (e.stopPropagation) e.stopPropagation();
            });

        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            InitScript();
            FileInput();
        });

    </script>

</asp:Content>
