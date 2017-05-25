using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using OBSecurity;
using System.Reflection;

namespace BP.Classes
{
    public class MailHelper
    {
        public static bool SendHtmlFormattedEmail(string recepientEmail, string subject, string body)
        {
            try
            {
                string AdminMail = WebConfigurationManager.AppSettings["AdminMail"];
                string AdminMailPwd = Security.Decrypt(WebConfigurationManager.AppSettings["AdminMailPwd"]);

                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(AdminMail, AdminMailPwd);
                client.UseDefaultCredentials = false;
                client.Credentials = credentials;

                using (MailMessage msg = new MailMessage())
                {
                    msg.From = new MailAddress(AdminMail);
                    msg.To.Add(new MailAddress(recepientEmail));
                    msg.Subject = subject;
                    msg.IsBodyHtml = true;
                    //msg.Body = string.Format("<html><head></head><body>New Password : <b>" + NewPassword + "</b></body>");
                    msg.Body = body;
                    client.Send(msg);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool SendMail(object obj, string password)
        {
            string Email = string.Empty, FullName = string.Empty, UserName = string.Empty;
            PropertyInfo[] props = obj.GetType().GetProperties();

            foreach (var prop in props.Select(x=> x.Name))
            {
                switch (prop)
                {
                    case "UserEmail":   { Email = PropVal(obj, prop); break; }
                    case "FullName" :   { FullName = PropVal(obj, prop); break; }
                    case "UserName" :   { UserName = PropVal(obj, prop); break; }
                }
            }

            string body = PopulateBody(FullName, UserName, password);
            if (!string.IsNullOrEmpty(body))
            {
                if (SendHtmlFormattedEmail(Email, "MyBudget - Accounts Information!", body))
                {
                    return true;
                }
            }

            return false;
        }

        protected static string PropVal(object obj, string prop)
        {
            return (string)obj.GetType().GetProperty(prop).GetValue(obj, null);
        }

        protected static string PopulateBody(string FullName, string UserName, string Password)
        {
            string body = string.Empty;
            using (System.IO.StreamReader reader = new System.IO.StreamReader( System.Web.HttpContext.Current.Server.MapPath("~/MailTemplates/email-contrast.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{FullName}", FullName);
            body = body.Replace("{UserName}", UserName);
            body = body.Replace("{Password}", Password);
            return body;
        }
    }
}