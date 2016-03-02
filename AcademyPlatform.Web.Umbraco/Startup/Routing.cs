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
            RootNodeRouteHandler rootNodeHandler = new RootNodeRouteHandler();
            RouteTable.Routes.MapUmbracoRoute("Register", "register", new { Controller = "Account", Action = "Register" }, rootNodeHandler);
            RouteTable.Routes.MapRoute("Validate", "validate/{email}/{validationCode}", new { Controller = "Account", Action = "Validate", ValidationCode = UrlParameter.Optional });
            RouteTable.Routes.MapUmbracoRoute("ForgotPassword", "forgot-password", new { Controller = "Account", Action = "ForgotPassword" }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("ChangePassword", "change-password", new { Controller = "Account", Action = "ChangePassword" }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("ResendValidationEmail", "resend-validation-email/{email}", new { Controller = "Account", Action = "ResendValidationEmail", Email = UrlParameter.Optional }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("LogOut", "logout", new { Controller = "Account", Action = "LogOut" }, rootNodeHandler);

            RouteTable.Routes.MapUmbracoRoute("JoinCourse", "join/{courseNiceUrl}", new { Controller = "Subscriptions", Action = "JoinCourse" }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("Assessment", "assessment/{courseNiceUrl}", new { Controller = "Assessment", Action = "Assessment" }, new CourseNodeProvider());
            RouteTable.Routes.MapUmbracoRoute("Profile", "profile", new { Controller = "Profile", Action = "Index" }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("Certificate", "certificate/{certificateUniqueCode}", new { Controller = "Certificate", Action = "Certificate" }, rootNodeHandler);
            RouteTable.Routes.MapUmbracoRoute("AwaitingPayment", "awaiting-payment/{courseNiceUrl}", new { Controller = "Subscriptions", Action = "AwaitingPayment" }, rootNodeHandler);



            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapHttpRoute("Default_Api", "umbraco/backoffice/api/{controller}/{action}/{id}", new { Id = UrlParameter.Optional }, null);

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
    public class RootNodeRouteHandler : UmbracoVirtualNodeRouteHandler
    {
        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            umbracoContext = umbracoContext ?? AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions.UmbracoContextExtensions.GetOrCreateContext();
            IPublishedContent rootNode = umbracoContext.ContentCache.GetAtRoot().FirstOrDefault();
            if (rootNode == null)
            {
                throw new ApplicationException("No root content found in Umbraco database");
            }

            return rootNode;
        }
    }

    public class CourseNodeProvider : RootNodeRouteHandler //TODO see if inheritance is the best way to achieve this functionality
    {
        protected override IPublishedContent FindContent(RequestContext requestContext, UmbracoContext umbracoContext)
        {
            IPublishedContent rootNode = base.FindContent(requestContext, umbracoContext);
            string courseUrl = (string)requestContext.RouteData.Values["courseNiceUrl"];
            IPublishedContent courseNode = rootNode.DescendantsOrSelf(nameof(Course)).FirstOrDefault(x => x.UrlName == courseUrl);
            return courseNode;
        }
    }
}