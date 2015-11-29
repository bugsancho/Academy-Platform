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
            RouteTable.Routes.MapRoute("Register", "register", new { Controller = "Account", Action = "Register" });
            RouteTable.Routes.MapRoute("Validate", "validate/{email}/{validationCode}", new { Controller = "Account", Action = "Validate", ValidationCode = UrlParameter.Optional });
            RouteTable.Routes.MapRoute("ForgotPassword", "forgotpassword", new { Controller = "Account", Action = "ForgotPassword" });
            RouteTable.Routes.MapRoute("ChangePassword", "changepassword", new { Controller = "Account", Action = "ChangePassword" });
            RouteTable.Routes.MapRoute("ResendValidationEmail", "ResendValidationEmail/{email}", new { Controller = "Account", Action = "ResendValidationEmail" });
            RouteTable.Routes.MapRoute("LogOut", "logout", new { Controller = "Account", Action = "LogOut" });

            RouteTable.Routes.MapRoute("JoinCourse", "join/{courseUrl}", new { Controller = "Subscriptions", Action = "JoinCourse" });



            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapHttpRoute("Default_Api", "api/{controller}/{action}/{id}", new { Id = UrlParameter.Optional }, null);

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