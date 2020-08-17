using IncidentManagement.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using IncidentManagement.Models;

namespace IncidentManagement.HelperServices
{
    public class LoginService
    {
       public static string BaseUri = ConfigurationManager.AppSettings["baseUri"] + ConfigurationManager.AppSettings["rootSite"];

        public async static Task<LoginModel> Login(LoginModel login)
        {
            LoginModel returnmessage = new LoginModel();          

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.PostAsJsonAsync("Login/AuthenticateUser", login).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoginModel>();
                    returnmessage = result;
                }
            }
            return returnmessage;
        }
        public async static Task<List<ListItemModel>> UserRoleList(string UserName)
        {
            var returnmessage = new List<ListItemModel>();

            using (HttpClient client = new System.Net.Http.HttpClient())
            {
                client.BaseAddress = new Uri(BaseUri);
                HttpResponseMessage response = client.GetAsync(string.Format("Login/{0}/UserRole", UserName)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<List<ListItemModel>>();
                    foreach (var p in result)
                    {
                        returnmessage.Add(new ListItemModel() { Id = p.Id, Value = p.Value });
                    }
                }
            }
            return returnmessage;
        }
    }
}