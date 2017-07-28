<%@ Page Title="Inbox" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MailInbox.aspx.cs" Inherits="BP.MailInbox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadcrumbsContent" runat="server">
    <ul class="breadcrumb">
        <li>
            <i class="ace-icon fa fa-home home-icon"></i>
            <a href="<%=Page.ResolveUrl("~/Dashboard.aspx")%>">Home</a>
        </li>
        <li class="active">Inbox</li>
    </ul><!-- /.breadcrumb -->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="PageHeaderContent" runat="server">
    <div class="page-header">
		<h1>
			Inbox
			<small>
				<i class="ace-icon fa fa-angle-double-right"></i>
				view &amp; manage mailbox
			</small>
		</h1>
	</div><!-- /.page-header -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row">
        <div class="col-xs-12">
            <!-- PAGE CONTENT BEGINS -->
            <div class="row">
                <div class="col-xs-12">
                    <div class="tabbable">
                        <div class="tab-content no-border no-padding">
                            <div id="inbox" class="tab-pane in active">
                                <div class="message-container">
                                    <div id="id-message-list-navbar" class="message-navbar clearfix">
                                        <div class="message-bar">
                                            <div class="message-infobar" id="id-message-infobar">
                                                <span class="blue bigger-150">Inbox</span>
                                                <span class="grey bigger-110">(<span id="TotalInbox1" runat="server"></span> unread messages)</span>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="messagebar-item-right">
                                                <div class="inline position-relative">
                                                    <a href="#" data-toggle="dropdown" class="dropdown-toggle">Sort 
                                                        &nbsp;<i class="ace-icon fa fa-caret-down bigger-125"></i>
                                                    </a>

                                                    <ul class="dropdown-menu dropdown-lighter dropdown-menu-right dropdown-100">
                                                        <li>
                                                            <a href="#">
                                                                <i class="ace-icon fa fa-check green"></i>
                                                                Date
                                                            </a>
                                                        </li>

                                                        <li>
                                                            <a href="#">
                                                                <i class="ace-icon fa fa-check invisible"></i>
                                                                From
                                                            </a>
                                                        </li>

                                                        <li>
                                                            <a href="#">
                                                                <i class="ace-icon fa fa-check invisible"></i>
                                                                Subject
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>

                                            <div class="nav-search minimized">
                                                <form class="form-search">
                                                    <span class="input-icon">
                                                        <input type="text" autocomplete="off" class="input-small nav-search-input" placeholder="Search inbox ..." />
                                                        <i class="ace-icon fa fa-search nav-search-icon"></i>
                                                    </span>
                                                </form>
                                            </div>
                                        </div>
                                    </div>

                                    <div id="id-message-item-navbar" class="hide message-navbar clearfix">
                                        <div class="message-bar">
                                            <div class="message-toolbar">
                                            </div>
                                        </div>

                                        <div>
                                            <div class="messagebar-item-left">
                                                <a href="#" class="btn-back-message-list">
                                                    <i class="ace-icon fa fa-arrow-left blue bigger-110 middle"></i>
                                                    <b class="bigger-110 middle">Back</b>
                                                </a>
                                            </div>

                                            <div class="messagebar-item-right">
                                                <i class="ace-icon fa fa-clock-o bigger-110 orange"></i>
                                                <span class="grey">Today, 7:15 pm</span>
                                            </div>
                                        </div>
                                    </div>


                                    <asp:ListView ID="ListView1" runat="server" OnPagePropertiesChanging="ListView1_PagePropertiesChanging"
                                        OnItemDataBound="ListView1_ItemDataBound">
                                        <LayoutTemplate>
                                            <div class="message-list-container">
                                                <div class="message-list" id="message_list" runat="server">
                                                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                                </div>
                                            </div>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <div class="message-item message-unread">
                                                <span id="Object" class="sender" title='<%# Eval("Object") %>'><%# Eval("Object") %> </span>
                                                <span id="Time" class="time"><%# Eval("Time") %></span>
                                                <span id="TimeInDetails" class="hide"><%# Eval("TimeInDetails") %></span>
                                                <span class="summary">
                                                    <span id="NoCount" class="badge badge-warning"><b><%# Eval("NoCount") %></b></span>
                                                    &nbsp;&nbsp;
                                                    <span id="Title" class="text"><%# Eval("Title") %></span>
                                                </span>
                                            </div>
                                        </ItemTemplate>
                                    </asp:ListView>

                                    <div class="message-footer clearfix">
                                        <div class="pull-left"> <span id="TotalInbox2" runat="server"></span> messages total </div>

                                        <div class="pull-right">
                                            <asp:DataPager ID="DataPager1" PagedControlID="ListView1" runat="server" class="btn-group btn-group-sm">
                                                <Fields>
                                                    <asp:NextPreviousPagerField PreviousPageText="<" FirstPageText="|<" ShowPreviousPageButton="true"
                                                        ShowFirstPageButton="true" ShowNextPageButton="false" ShowLastPageButton="false"
                                                        ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false"
                                                        RenderDisabledButtonsAsLabels="false" />
                                                    <asp:NumericPagerField ButtonType="Link" CurrentPageLabelCssClass="btn btn-primary disabled"
                                                        RenderNonBreakingSpacesBetweenControls="false" NumericButtonCssClass="btn btn-default" ButtonCount="10"
                                                        NextPageText="..." NextPreviousButtonCssClass="btn btn-default" />
                                                    <asp:NextPreviousPagerField NextPageText=">" LastPageText=">|" ShowNextPageButton="true"
                                                        ShowLastPageButton="true" ShowPreviousPageButton="false" ShowFirstPageButton="false"
                                                        ButtonCssClass="btn btn-default" RenderNonBreakingSpacesBetweenControls="false"
                                                        RenderDisabledButtonsAsLabels="false" />
                                                </Fields>
                                            </asp:DataPager>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /.tab-content -->
                    </div>
                    <!-- /.tabbable -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

            <div class="hide message-content" id="id-message-content">
                <div class="message-header clearfix">
                    <div class="pull-left">
                        <span class="blue bigger-125">Clik to open this message </span>

                        <div class="space-4"></div>

                        <i class="ace-icon fa fa-star orange2"></i>
                        &nbsp;<img id="avatar" class="middle" runat="server" width="32" />
                        &nbsp;<a href="#" class="sender"><asp:LoginName ID="LoginName1" runat="server" /></a>
                        &nbsp;<i class="ace-icon fa fa-clock-o bigger-110 orange middle"></i>
                        <span id="DetailsTime" class="time grey"></span>
                    </div>
                </div>

                <div class="hr hr-double"></div>

                <div class="message-body">
                    <asp:GridView ID="gvDetail" runat="server" AutoGenerateColumns="false" AllowSorting="true" CssClass="table table-bordered table-striped table-hover">
                    </asp:GridView>
                </div>
                <div class="hr hr-double"></div>
            </div>
            <!-- /.message-content -->

        <!-- PAGE CONTENT ENDS -->
        </div>
        <!-- /.col -->
    </div><!-- /.row -->


</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ScriptContent" runat="server">

    <!-- page specific plugin scripts start -->
    <script src="<%=Page.ResolveUrl("~/assets/js/jquery.hotkeys.index.min.js")%>"></script>
    <script src="<%=Page.ResolveUrl("~/assets/js/bootstrap-wysiwyg.min.js")%>"></script>
    <!-- page specific plugin scripts end -->

    <script type="text/javascript">

        var baseUrl = '<%= Page.ResolveClientUrl("~/") %>';
        function LoadInboxImage(src) {
            var url = src.replace("~/", baseUrl);
            $('#MainContent_avatar').attr('src', url);
        }

        function DisplayInboxDetails() {
            var _title, _object, _timedetails;

            $('.message-list .message-item .text').on('click', function () {
                var message = $(this).closest('.message-item');

                //////
                _title = message.find('span#Title').html();
                _object = message.find('span#Object').html();
                _timedetails = message.find('span#TimeInDetails').html();
                //////

                //if message is open, then close it
                if (message.hasClass('message-inline-open')) {
                    message.removeClass('message-inline-open').find('.message-content').remove();
                    return;
                }
                //remove unread class
                if (message.hasClass('message-unread')) {
                    message.removeClass('message-unread');
                }
                //close opened inline
                var close_others = $('.message-list .message-item .text').closest('.message-item');
                if (close_others.hasClass('message-inline-open')) {
                    close_others.removeClass('message-inline-open').find('.message-content').remove();
                }

                $('.message-container').append('<div class="message-loading-overlay"><i class="fa-spin ace-icon fa fa-spinner orange2 bigger-160"></i></div>');
                setTimeout(function () {
                    $('.message-container').find('.message-loading-overlay').remove();
                    message
                        .addClass('message-inline-open')
                        .append('<div class="message-content" />')
                    var content = message.find('.message-content:last').html($('#id-message-content').html());

                    //remove scrollbar elements
                    content.find('.scroll-track').remove();
                    content.find('.scroll-content').children().unwrap();

                    content.find('.message-body').ace_scroll({
                        size: 450,
                        mouseWheelLock: true,
                        styleClass: 'scroll-visible'
                    });

                    //////
                    content.find('span#DetailsTime').html(_timedetails);
                    //////

                }, 500 + parseInt(Math.random() * 500));

                //ajax start
                $.ajax({
                    type: "POST",
                    url: "MailInbox.aspx/PopulateInboxDetails",
                    data: '{title:"' + _title + '",details:"' + _object + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $("#<%=gvDetail.ClientID%>").empty();

			            if (data.d.length > 0) {
			                $("#<%=gvDetail.ClientID%>").append("<tr><th>Title</th><th>Object</th><th>Detail</th><th>Modified By</th><th>Last Modified</th></tr>");

			                for (var i = 0; i < data.d.length; i++)
			                {
			                    var milli = data.d[i].LastModDateTime.replace(/\/Date\((-?\d+)\)\//, '$1');
			                    var date = new Date(parseInt(milli));

				                $("#<%=gvDetail.ClientID%>").append("<tr><td>" +
							        data.d[i].Title + "</td> <td>" +
							        data.d[i].Object + "</td> <td>" +
							        data.d[i].Detail + "</td> <td>" +
							        data.d[i].ModifiedBy + "</td> <td>" +
							        formatDate(date) + "</td></tr>");
					        }
                        }
			        },
		            error: function (result) {
		                alert("Ffffuucck! Error pulak.");
		            }
		        });
	            //ajax end
	        });
        }

        function formatDate(date) {
            var monthNames = [
              "January", "February", "March",
              "April", "May", "June", "July",
              "August", "September", "October",
              "November", "December"
            ];

            var day = date.getDate();
            var monthIndex = date.getMonth();
            var year = date.getFullYear();

            date.setHours(10, 30, 53, 400);

            return day + ' ' + monthNames[monthIndex] + ' ' + year + ', ' + 
                date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
        }

        $(document).ready(function () {
            DisplayInboxDetails();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
            DisplayInboxDetails();
        });
    </script>

</asp:Content>
