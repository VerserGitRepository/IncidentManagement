using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IncidentManagement.Startup))]
namespace IncidentManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
