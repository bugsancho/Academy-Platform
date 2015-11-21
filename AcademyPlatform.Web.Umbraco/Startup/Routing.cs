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

            RouteTable.Routes.MapRoute("Validate",
                "validate/{email}/",
                new
                {
                    Controller = "Account",
                    Action = "Validate"
                });

            RouteTable.Routes.MapRoute("Join Course",
                "joincourse/{courseId}",
                new
                {
                    Controller = "Subscriptions",
                    Action = "JoinCourse"
                });

            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.MapHttpRoute(
                "Default_Api",
                "api/{controller}/{action}/{id}",
                new { Id = UrlParameter.Optional }, 
                null);

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