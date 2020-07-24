using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IncidentManagement.Membership
{
    public class User
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public List<Role> Roles { get; set; }

        public User()
        {
            this.Roles = new List<Role>();
        }

        public bool IsInRole(string roleName)
        {
            var role = this.Roles.Where(x => x.RoleName == roleName).SingleOrDefault();

            return role != null;
        }

        public bool HasPermission(string permissionName)
        {
            var permission = this.Roles.SelectMany(x => x.Permissions.Where(y => y.ToLower() == permissionName.ToLower())).FirstOrDefault();

            return permission != null;
        }

    }
    public class Role
    {
        public string RoleName { get; set; }
        public List<string> Permissions { get; set; }

        public Role()
        {
            Permissions = new List<string>();
        }
    }
}