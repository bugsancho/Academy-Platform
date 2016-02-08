namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;

    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    public class Routing : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            RouteTable.Routes.MapRoute("Register", "register", new { Controller = "Account", Action = "Register" });
            RouteTable.Routes.MapRoute("Validate", "validate/{email}/{validationCode}", new { Controller = "Account", Action = "Validate", ValidationCode = UrlParameter.Optional });
            RouteTable.Routes.MapRoute("ForgotPassword", "forgot-password", new { Controller = "Account", Action = "ForgotPassword" });
            RouteTable.Routes.MapRoute("ChangePassword", "change-password", new { Controller = "Account", Action = "ChangePassword" });
            RouteTable.Routes.MapRoute("ResendValidationEmail", "resend-validation-email/{email}", new { Controller = "Account", Action = "ResendValidationEmail", Email = UrlParameter.Optional });
            RouteTable.Routes.MapRoute("LogOut", "logout", new { Controller = "Account", Action = "LogOut" });

            RouteTable.Routes.MapRoute("JoinCourse", "join/{courseNiceUrl}", new { Controller = "Subscriptions", Action = "JoinCourse" });
            RouteTable.Routes.MapUmbracoRoute("Exam", "assessment/{courseNiceUrl}", new { Controller = "Assessment", Action = "Assessment" }, new CourseNodeProvider());
            RouteTable.Routes.MapRoute("AwaitingPayment", "awaiting-payment/{courseNiceUrl}", new { Controller = "Subscriptions", Action = "AwaitingPayment" });



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

    public class CourseNodeProvider : UmbracoVirtualNodeRouteHandler
    {
        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            string courseUrl = (string)requestContext.RouteData.Values["courseNiceUrl"];
            IPublishedContent rootNode = umbracoContext.ContentCache.GetAtRoot().FirstOrDefault();
            if (rootNode == null)
            {
                throw new ApplicationException("No root content found in Umbraco database");
            }

            IPublishedContent courseNode = rootNode.DescendantsOrSelf(nameof(Course)).FirstOrDefault(x => x.UrlName == courseUrl);
            return courseNode;
        }
    }
}