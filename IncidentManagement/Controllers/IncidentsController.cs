using IncidentManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IncidentManagement.Controllers
{
    public class IncidentsController : Controller
    {
        public IEnumerable<Incident> result = null;
        private IncidentManagementEntities db = new IncidentManagementEntities();
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["FullUserName"] != null)
            {
                Session["ErrorMessage"] = null;
                return View(db.Incidents.Where(s => s.Status.ToUpper().Trim() == "OPEN").ToList().OrderByDescending(s => s.INC));
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Login");
            }

        }
        [HttpPost]
        public ActionResult Index(string submitButton, string ticketstatusId, string TestCategoryID, string ticketprtyId, string RequestTypeID, int incnum = 0)
        {
            if (Session["FullUserName"] != null)
            {
                if (incnum != 0)
                {
                    result = db.Incidents.Where(s => s.INC == incnum).ToList();
                }
                if (!String.IsNullOrEmpty(ticketstatusId))
                {
                    if (result != null)
                    {
                        result = result.Where(s => s.Status.ToUpper().Trim() == ticketstatusId).ToList().OrderByDescending(s => s.INC);
                    }
                    else
                    {
                        result = db.Incidents.Where(s => s.Status.ToUpper().Trim() == ticketstatusId).ToList().OrderByDescending(s => s.INC);
                    }
                }
                if (!String.IsNullOrEmpty(ticketprtyId))
                {
                    if (result != null)
                    {
                        result = result.Where(s => s.Priority.ToUpper().Trim() == ticketprtyId).ToList().OrderByDescending(s => s.INC);
                    }
                    else
                    {
                        result = db.Incidents.Where(s => s.Priority.ToUpper().Trim() == ticketprtyId).ToList().OrderByDescending(s => s.INC);
                    }
                }
                if (!String.IsNullOrEmpty(TestCategoryID))
                {
                    if (result != null)
                    {
                        result = result.Where(s => s.TestCategory.ToUpper().Trim() == TestCategoryID).ToList().OrderByDescending(s => s.INC);
                    }
                    else
                    {
                        result = db.Incidents.Where(s => s.TestCategory.ToUpper().Trim() == TestCategoryID).ToList().OrderByDescending(s => s.INC);
                    }
                }
                if (!String.IsNullOrEmpty(RequestTypeID))
                {
                    if (result != null)
                    {
                        result = result.Where(s => s.RequestType == RequestTypeID).ToList().OrderByDescending(s => s.INC).ToList();
                    }
                    else
                    {
                        result = db.Incidents.Where(s => s.RequestType == RequestTypeID).ToList().OrderByDescending(s => s.INC).ToList();
                    }
                }

                if (result == null)
                {
                    result = db.Incidents.ToList().OrderByDescending(s => s.INC);
                }

                if (result != null && submitButton == "ExportToExcel")
                {
                    GridView gv = new GridView();
                    gv.DataSource = result;
                    gv.DataBind();
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment; filename=Tickets-Summary.xls");
                    Response.ContentType = "application/ms-excel";
                    Response.Charset = "";
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter htw = new System.Web.UI.HtmlTextWriter(sw);
                    gv.RenderControl(htw);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
                return View(result);
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult Details(int? INC)
        {
            if (Session["FullUserName"] != null)
            {
                if (INC == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Incident incident = db.Incidents.Find(INC);
                if (incident == null)
                {
                    return HttpNotFound();
                }
                return View(incident);
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return Redirect("https://customers.verser.com.au/Incidentmanagement/");
            }
        }

        public ActionResult Create()
        {
            if (Session["FullUserName"] != null)
            {
                Session["ErrorMessage"] = null;
                return View();
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public ActionResult Create( Incident incident, string submitButton)
        {
            //[Bind(Include = "RequestType,TestCategory,DateCreated,UserName,Location,FeedBackSource,Priority,TestDetails,Investigation,DevNotes,FilePath,Email,Orders")]
            if (Session["FullUserName"] != null)
            {
                Session["ErrorMessage"] = null;
                string _filename;
                incident.Status = "OPEN";
                HttpPostedFileBase file = Request.Files["uploadfile"];

                if (file.ContentLength > 0)
                {
                    _filename = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _filename);
                    file.SaveAs(_path);
                    incident.FilePath = _filename;
                }
                incident.DateCreated = DateTime.Now;
                db.Incidents.Add(incident);
                db.SaveChanges();
                //  string MailGDL = ConfigurationManager.AppSettings["MailGDL"];

                List<string> emailAddresses = new List<string>();
                string sendTo = incident.Email;
                emailAddresses.Add("ittickets@verser.com.au");
                emailAddresses.Add(incident.Email);
                if (emailAddresses.Count != 0)
                {
                    if (emailAddresses.Count > 1)
                    {
                        sendTo = string.Join(",", emailAddresses.AsEnumerable());
                    }
                    else
                    {
                        sendTo = incident.Email;
                    }
                }
                string templatePath = Path.Combine(Server.MapPath("~/EmailTemplates"));
                if (incident != null)
                {
                    Dictionary<string, string> replacements = new Dictionary<string, string>();
                    replacements.Add("UserName", incident.UserName);
                    replacements.Add("Priority", incident.Priority);
                    replacements.Add("INC", incident.INC.ToString());
                    replacements.Add("TestDetails", incident.TestDetails);
                    try
                    {
                        Helper.MailHelper.SendMail(String.Format("{0}\\{1}", templatePath, "TikcetCreated.htm"), sendTo, null, String.Format("Incident Created : {0}", incident.INC), replacements);
                    }
                    catch (Exception ex)
                    {
                        string LogError = String.Format("Sending email failed {0}", ex.Message);
                        //Logger.LogError(String.Format("Sending email failed {0}", ex.Message), "Email", DataWorkspace);
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpGet]
        public FileResult Download(string file)
        {
            var FileVirtualPath = "~/UploadedFiles/" + file;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }
        public ActionResult Edit(int? id)
        {
            if (Session["FullUserName"] != null)
            {
                Session["ErrorMessage"] = null;
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Incident incident = db.Incidents.Find(id);
                if (incident == null)
                {
                    return HttpNotFound();
                }
                return View(incident);
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,INC,Enviroment,TestCategory,DateCreated,UserName,Location,SoftwareVersion,TestCase,FeedBackSource,Priority,TestDetails,Status,TesterNote,Investigation,DevNotes,Email,FilePath")] Incident incident)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incident).State = EntityState.Modified;
                incident.DateCreated = DateTime.Now;
                db.SaveChanges();
                SendEmail(incident.Email, +incident.INC + " -Ticket Status Now Changed to :" + incident.Status, "Your Ticket Status Changed to " + incident.Status + "</br></br> Issue Details:" + incident.TestDetails + "</br> </br> Developer Note:" + incident.DevNotes + "</br> </br>Please check in Ticket System for More details. https://customers.verser.com.au/Incidentmanagement/");
                return RedirectToAction("Index");
            }
            return View(incident);
        }
        public static string SendEmail(string recipient, string subject, string body)
        {
            string SmtpClientIP = ConfigurationManager.AppSettings["SmtpClientIP"];
            int SmtpClientPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpClientPort"]);
            string SmtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            string SmtpFromMail = ConfigurationManager.AppSettings["SmtpFromMail"];

            SmtpClient smtout = new SmtpClient(SmtpClientIP, SmtpClientPort);
            smtout.EnableSsl = true;
            smtout.Credentials = new System.Net.NetworkCredential(SmtpUser, SmtpPassword);

            if (string.IsNullOrEmpty(recipient))
            {
                recipient = "support@verser.com.au";
            }
            MailMessage email = new MailMessage();
            MailAddress froma = new MailAddress(SmtpFromMail);
            //if (file != null)
            //{
            //    System.Net.Mail.Attachment attachment;
            //    attachment = new System.Net.Mail.Attachment(file);
            //    email.Attachments.Add(attachment);
            //}
            email.From = froma;
            email.To.Add(recipient);
            email.Subject = subject;
            email.IsBodyHtml = true; //just in case you want to send as html if it is regular text then false.
            email.Body = body;
            try
            {
                smtout.Send(email);
                return "Email sent successfully";
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
        }
        public ActionResult Delete(int? id)
        {
            if (Session["FullUserName"] != null && Session["FullUserName"].Equals("VerserTicketAdmin"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Incident incident = db.Incidents.Find(id);
                if (incident == null)
                {
                    return HttpNotFound();
                }
                return View(incident);
            }
            else
            {
                Session["ErrorMessage"] = "Invalid User ID Or Password";
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Incident incident = db.Incidents.Find(id);
            db.Incidents.Remove(incident);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public string ApprovedRequest(int TicketId)
        {
            if (TicketId > 0)
            {
                var requestData = db.Incidents.Where(s => s.INC == TicketId).FirstOrDefault();
                if (requestData != null)
                {
                    requestData.Status = "APPROVED";
                    if (requestData.Orders ==null )
                    {
                        requestData.Orders = "00";
                    }
                    db.Entry(requestData).State = EntityState.Modified;                   
                    db.SaveChanges();
                    return "Success";
                }
            }
            return "Failed";
        }
        [HttpPost]
        public string RejectedRequest(int TicketId)
        {
            if (TicketId > 0)
            {
                var requestData = db.Incidents.Where(s => s.INC == TicketId).FirstOrDefault();

                if (requestData != null)
                {
                    requestData.Status = "REJECTED";
                    if (requestData.Orders == null)
                    {
                        requestData.Orders = "00";
                    }
                    db.Entry(requestData).State = EntityState.Modified;
                    db.SaveChanges();
                    return "Success";
                }
            }
            return "Failed";
        }
    }
}
