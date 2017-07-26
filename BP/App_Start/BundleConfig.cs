using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace BP
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254726
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                  "~/Scripts/WebForms/WebForms.js",
                  "~/Scripts/WebForms/WebUIValidation.js",
                  "~/Scripts/WebForms/MenuStandards.js",
                  "~/Scripts/WebForms/Focus.js",
                  "~/Scripts/WebForms/GridView.js",
                  "~/Scripts/WebForms/DetailsView.js",
                  "~/Scripts/WebForms/TreeView.js",
                  "~/Scripts/WebForms/WebParts.js"));

            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            //bundles.Add(new ScriptBundle("~/bundles/AjaxControlToolkit")
            //    .IncludeDirectory("~/Scripts/AjaxControlToolkit", "*.js", true));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            /********************************************************************************************************/

//////////////* masterpage - start *///////////////
            //CSS
            bundles.Add(new StyleBundle("~/css/site-css")
            //<!-- page specific plugin styles start -->
            .Include("~/assets/css/jquery-ui.custom.min.css")
            //<!-- form-elements -->
            .Include("~/assets/css/chosen.min.css")
            .Include("~/assets/css/bootstrap-datepicker3.min.css")
            .Include("~/assets/css/bootstrap-timepicker.min.css")
            .Include("~/assets/css/daterangepicker.min.css")
            .Include("~/assets/css/bootstrap-datetimepicker.min.css")
            .Include("~/assets/css/bootstrap-colorpicker.min.css")
            //<!-- elements -->
            .Include("~/assets/css/jquery.gritter.min.css")
            //<!-- form-wizard -->
            .Include("~/assets/css/select2.min.css")
            //<!-- text fonts -->
            .Include("~/assets/css/fonts.googleapis.com.css"));

            //SCRIPTS
            bundles.Add(new ScriptBundle("~/scripts/site-scripts")
            //<!-- jquery -->
            .Include("~/assets/js/jquery-ui.custom.min.js")
            .Include("~/assets/js/jquery.ui.touch-punch.min.js")
            //<!-- form-elements-->
            .Include("~/assets/js/chosen.jquery.min.js")
            .Include("~/assets/js/spinbox.min.js")
            .Include("~/assets/js/bootstrap-datepicker.min.js")
            .Include("~/assets/js/bootstrap-timepicker.min.js")
            .Include("~/assets/js/moment.min.js")
            .Include("~/assets/js/daterangepicker.min.js")
            .Include("~/assets/js/bootstrap-datetimepicker.min.js")
            .Include("~/assets/js/bootstrap-colorpicker.min.js")
            .Include("~/assets/js/jquery.knob.min.js")
            .Include("~/assets/js/autosize.min.js")
            .Include("~/assets/js/jquery.inputlimiter.min.js")
            .Include("~/assets/js/jquery.maskedinput.min.js")
            .Include("~/assets/js/bootstrap-tag.min.js")
            //<!-- elements -->
            .Include("~/assets/js/jquery.easypiechart.min.js")
            .Include("~/assets/js/jquery.gritter.min.js")
            .Include("~/assets/js/spin.js")
            //<!-- form-wizard -->
            .Include("~/assets/js/wizard.min.js")
            .Include("~/assets/js/bootbox.js")
            .Include("~/assets/js/select2.min.js")
            //<!-- tables -->
            .Include("~/assets/js/jquery.dataTables.min.js")
            .Include("~/assets/js/jquery.dataTables.bootstrap.min.js")
            .Include("~/assets/js/dataTables.buttons.min.js")
            .Include("~/assets/js/buttons.flash.min.js")
            .Include("~/assets/js/buttons.html5.min.js")
            .Include("~/assets/js/buttons.print.min.js")
            .Include("~/assets/js/buttons.colVis.min.js")
            .Include("~/assets/js/dataTables.select.min.js")
            .Include("~/assets/js/jszip.min.js")
            .Include("~/assets/js/pdfmake.min.js")
            .Include("~/assets/js/vfs_fonts.js"));
//////////////* masterpage - end *///////////////

            /* page-specific - start */
            //using at (SCRIPTS): Dashboard (CHART etc.)
            bundles.Add(new ScriptBundle("~/scripts/Dashboard")
            .Include("~/assets/js/jquery-ui.custom.min.js")
            .Include("~/assets/js/jquery.ui.touch-punch.min.js")
            .Include("~/assets/js/jquery.easypiechart.min.js")
            .Include("~/assets/js/jquery.sparkline.index.min.js")
            .Include("~/assets/js/jquery.flot.min.js")
            .Include("~/assets/js/jquery.flot.pie.min.js")
            .Include("~/assets/js/jquery.flot.resize.min.js"));
            /* page-specific - end */

            //ignore *.min
            bundles.IgnoreList.Clear();
        }
    }
}