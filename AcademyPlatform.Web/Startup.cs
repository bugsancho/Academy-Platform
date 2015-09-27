using AcademyPlatform.Web;

using log4net.Config;

using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]
[assembly: XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace AcademyPlatform.Web
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
