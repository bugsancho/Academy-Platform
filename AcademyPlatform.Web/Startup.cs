using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AcademyPlatform.Web.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace AcademyPlatform.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
