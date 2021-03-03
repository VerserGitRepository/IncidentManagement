using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IncidentManagement.Controllers
{
    public class TicketRemedyController : Controller
    {
        // GET: TicketRemedy
        public ActionResult Index()
        {
            if (Session["UserName"] !=null && Session["Administrator"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Login");
        }
    }
}