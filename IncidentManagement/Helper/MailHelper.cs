using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Configuration;
using IncidentManagement.Controllers;
namespace IncidentManagement.Helper
{
    public class MailHelper
    {
        public static void SendMail(string templateName, string sendTo, string cc, string subject, Dictionary<string, string> replacements, MailPriority priority = MailPriority.Normal, bool writeAsFile = false)
        {
            MailDefinition md = new MailDefinition();
            md.BodyFileName = templateName;
            md.CC =cc;
            md.From = @"no-reply@verser.com.au";
            md.Subject = subject;
            md.IsBodyHtml = true;
            MailMessage msg = md.CreateMailMessage(@sendTo, null, new System.Web.UI.Control());
            foreach (var r in replacements)
            {
                string placeholder = String.Format(@"<%{0}%>", r.Key);
                msg.Body = msg.Body.Replace(placeholder, r.Value);
            }
            //string recipient, string subject,string body
            IncidentsController.SendEmail(sendTo,subject,msg.Body);
        }
      
    }
}