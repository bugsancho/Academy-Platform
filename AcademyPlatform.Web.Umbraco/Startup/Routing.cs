namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Web.Mvc;
    using System.Web.Routing;

    using global::Umbraco.Core;
    using global::Umbraco.Web;

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

            AreaRegistration.RegisterAllAreas();
            //RouteTable.Routes.MapRoute("Register",
            //    "register",
            //    new
            //    {
            //        Controller = "Account",
            //        Action = "Register"
            //    });
            //RouteTable.Routes.MapHttpRoute(
            //    "Admin_default",
            //    "umbraco/backoffice/admin/{action}/{id}",
            //    new { Controller = "Admin", action = "get", id = UrlParameter.Optional },
            //    namespaces: new[] { " AcademyPlatform.Web.Umbraco.Areas.Admin.Controllers" }

            //);
        }
    }
}