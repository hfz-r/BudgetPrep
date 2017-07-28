using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using BP;
using System.Web.UI.WebControls;

namespace BP
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //custom datapager
            WebControl.DisabledCssClass = "customDisabledClassName";
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
//            try
//            { 
//            }
//            catch (System.Security.Cryptography.CryptographicException)
//            {
//                FormsAuthentication.SignOut();
//            }
        }
    }
}
