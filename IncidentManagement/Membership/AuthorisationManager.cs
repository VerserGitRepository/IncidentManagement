using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Configuration;
namespace IncidentManagement.Membership
{
    public class LoggedInEventArgs : EventArgs
    {
        public bool IsLoggedIn { get; set; }
    }
    public delegate void LoggedInUserChangedEventHandler(LoggedInEventArgs e);
    //public class AuthorisationManager
    //{
    //    private static User loggedInUser;
    //    private static SecurityDataService.SecurityData securityContext;

    //    public static event LoggedInUserChangedEventHandler LoggedInUserChanged;

    //    private static void OnLoggedInUserChanged(LoggedInEventArgs e)
    //    {
    //        if (LoggedInUserChanged != null)
    //        {
    //            LoggedInUserChanged(e);
    //        }
    //    }

    //    public static Membership.User LoggedInUser
    //    {
    //        get { return loggedInUser; }
    //        private set
    //        {
    //            loggedInUser = value;

    //            OnLoggedInUserChanged(new LoggedInEventArgs { IsLoggedIn = value != null });
    //        }
    //    }
    //    static AuthorisationManager()
    //    {
    //        securityContext = new SecurityDataService.SecurityData(new Uri(ConfigurationManager.AppSettings["securityUri"]));
    //        securityContext.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["user"], ConfigurationManager.AppSettings["pass"]);
    //        securityContext.IgnoreResourceNotFoundException = true;
    //    }

    //    public static bool AuthenticateUser(string userName, string password)
    //    {
    //        SecurityDataService.UserRegistration jmsUser = securityContext.UserRegistrations.Where(x => x.UserName == userName).SingleOrDefault();

    //        if (jmsUser == null)
    //        {
    //            LoggedInUser = null;
    //            return false;
    //        }

    //        JMSDataService.JMSData jmsContext = new JMSDataService.JMSData(new Uri(ConfigurationManager.AppSettings["jmsUri"]));
    //        jmsContext.Credentials = new NetworkCredential(userName, password);

    //        try
    //        {
    //            //Perform an arbitrary data operation to test supplied credentials
    //            var testPriceList = jmsContext.Carriers.FirstOrDefault();
    //            //jmsContext.SaveChanges();

    //            User user = new User();
    //            user.UserName = userName;
    //            user.FullName = jmsUser.FullName;
    //            user.Password = password;

    //            var roles = securityContext.RoleAssignments.Expand("Role").Where(x => x.UserName == userName);

    //            foreach (var role in roles)
    //            {
    //                Role newRole = new Role { RoleName = role.RoleName };

    //                foreach (var permission in securityContext.RolePermissions.Expand("Permission").Where(x => x.RoleName == role.RoleName))
    //                {
    //                    newRole.Permissions.Add(permission.Permission.Name);
    //                }

    //                user.Roles.Add(newRole);
    //            }

    //            LoggedInUser = user;

    //            return true;
    //        }
    //        catch (System.Data.Services.Client.DataServiceQueryException ex)
    //        {
    //            if (ex.Response.StatusCode == 401)
    //            {
    //                LoggedInUser = null;
    //            }

    //            return false;
    //        }
    //    }

    //    public static void Logout()
    //    {
    //        LoggedInUser = null;
    //    }
    //}
}
