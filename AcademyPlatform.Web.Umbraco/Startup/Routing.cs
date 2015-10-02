namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using global::Umbraco.Core;

    public class Routing : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteTable.Routes.MapRoute("Register",
                "register",
                new
                    {
                        Controller = "Account",
                        Action = "Register"
                    });
        }
    }
}