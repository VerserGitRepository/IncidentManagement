using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncidentManagement.HelperServices
{
    public class UserRole
    {
        public static bool UserCanEdit()
        {
            bool Returnflag = false;
            if (HttpContext.Current.Session["FullUserName"] != null)
            {
                 if (HttpContext.Current.Session["Administrator"] != null && HttpContext.Current.Session["Administrator"].ToString() == "Administrator")
                {
                    Returnflag = true;
                }               
            }
            return Returnflag;
        }
    }
}