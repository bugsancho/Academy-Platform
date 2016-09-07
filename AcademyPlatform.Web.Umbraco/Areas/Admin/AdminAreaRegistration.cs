using System.Web.Mvc;

namespace AcademyPlatform.Web.Umbraco.Areas.Admin
{
    using AcademyPlatform.Web.Umbraco.Startup;

    using global::Umbraco.Web;

    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var route = context.Routes.MapUmbracoRoute(
                 "Admin_default",
                 "Umbraco/Backoffice/Admin/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional, area = AreaName },
                 new RootNodeRouteHandler()

             );

            route.DataTokens["Area"] = AreaName;
        }
    }
}