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

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            /********************************************************************************************************/

            /* master - start */
            //Ace Template script bundling
            bundles.Add(new ScriptBundle("~/scripts/ace-scripts")
            .Include("~/assets/js/jquery-2.1.4.min.js")
            .Include("~/assets/js/bootstrap.min.js")
            .Include("~/assets/js/ace-extra.min.js")
            .Include("~/assets/js/ace-elements.min.js")
            .Include("~/assets/js/ace.min.js"));

            //Ace Template style bundling
            bundles.Add(new StyleBundle("~/css/ace-styles")
            .Include("~/assets/css/bootstrap.min.css")
            .Include("~/assets/css/fonts.googleapis.com.css")
            .Include("~/assets/css/ace.min.css")
            .Include("~/assets/css/ace-skins.min.css")
            .Include("~/assets/css/ace-rtl.min.css"));
            /* master - end */

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

            //using at (STYLES): FORM-ELEMENTS 
            bundles.Add(new StyleBundle("~/styles/Form-Elements_Styles")
            .Include("~/assets/css/jquery-ui.custom.min.css")
            .Include("~/assets/css/bootstrap-datepicker3.min.css")
            .Include("~/assets/css/bootstrap-timepicker.min.css")
            .Include("~/assets/css/daterangepicker.min.css")
            .Include("~/assets/css/bootstrap-datetimepicker.min.css")
            .Include("~/assets/css/bootstrap-colorpicker.min.css"));

            //using at (SCRIPTS): FORM-ELEMENTS 
            bundles.Add(new ScriptBundle("~/scripts/Form-Elements_Scripts")
            .Include("~/assets/js/jquery-ui.custom.min.js")
		    .Include("~/assets/js/jquery.ui.touch-punch.min.js")
		    .Include("~/assets/js/chosen.jquery.min.js")
            .Include("~/assets/js/jquery.knob.min.js")
            .Include("~/assets/js/jquery.inputlimiter.min.js")
            .Include("~/assets/js/jquery.maskedinput.min.js")
		    .Include("~/assets/js/spinbox.min.js")
		    .Include("~/assets/js/bootstrap-datepicker.min.js")
		    .Include("~/assets/js/bootstrap-timepicker.min.js")
		    .Include("~/assets/js/moment.min.js")
		    .Include("~/assets/js/daterangepicker.min.js")
		    .Include("~/assets/js/bootstrap-datetimepicker.min.js")
		    .Include("~/assets/js/bootstrap-colorpicker.min.js")
		    .Include("~/assets/js/autosize.min.js")
		    .Include("~/assets/js/bootstrap-tag.min.js"));

            //using at (STYLES): FORM-WIZARD 
            bundles.Add(new StyleBundle("~/styles/Form-Wizard_Styles")
            .Include("~/assets/css/select2.min.css"));

            //using at (SCRIPTS): FORM-WIZARD 
            bundles.Add(new ScriptBundle("~/scripts/Form-Wizard_Scripts")
            .Include("~/assets/js/jquery.validate.min.js")
            .Include("~/assets/js/jquery-additional-methods.min.js")
            .Include("~/assets/js/wizard.min.js")
            .Include("~/assets/js/bootbox.js")
            .Include("~/assets/js/select2.min.js"));
            /* page-specific - end */

            //ignore *.min
            bundles.IgnoreList.Clear();
        }
    }
}