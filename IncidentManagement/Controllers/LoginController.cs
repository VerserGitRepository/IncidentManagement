﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IncidentManagement.Models;
using System.Threading.Tasks;
using IncidentManagement.HelperServices;

namespace IncidentManagement.Controllers
{
    public class LoginController : Controller
    {       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string UserName, string Password)
        {
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                Session["FullUserName"] = null;
                Session["ErrorMessage"] = " Username Or Password Empty!";
                return View("Login");
            }
            LoginModel logindetails = new LoginModel { UserName = UserName, Password = Password };
            Task<LoginModel> userReturn = LoginService.Login(logindetails);
            if (userReturn.Result.IsLoggedIn == true)
            {
                Session["FullUserName"] = logindetails.UserName;
                Session["UserName"] = UserName;
                Session["ErrorMessage"] = null;
                var userrRoles = LoginService.UserRoleList(UserName);
                if (userrRoles.Result.Count > 0)
                {
                    var usrRoles = userrRoles.Result.Where(u => u.Value == "Administrator").FirstOrDefault();
                    if (usrRoles !=null)
                    {
                        Session["Administrator"] = "Administrator";
                    }                   
                }               
                return RedirectToAction("Index", "Incidents");
            }
            else
            {
                Session["FullUserName"] = null;
                Session["ErrorMessage"] = "Invalid UserName Or Password";
                return View("Login");
            }
        }
        public ActionResult Logout(LoginModel login)
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Login");
        }
    }
}