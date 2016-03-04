[assembly: Microsoft.Owin.OwinStartup("AcademyPlatformOwinStartup", typeof(AcademyPlatform.Web.Umbraco.Startup.AcademyPlatformOwinStartup))]
namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Web.Security;

    using global::Umbraco.Core.Configuration;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    using Hangfire;
    using Hangfire.Common;
    using Hangfire.Server;

    using Owin;


    public class AcademyPlatformOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);

            GlobalConfiguration.Configuration.UseLog4NetLogProvider();
            GlobalConfiguration.Configuration.UseSqlServerStorage("Hangfire");
            
            GlobalJobFilters.Filters.Add(new PrepareUmbracoRequestFilter());
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }

    public class PrepareUmbracoRequestFilter : JobFilterAttribute, IServerFilter
    {
        public void OnPerforming(PerformingContext filterContext)
        {
            AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions.UmbracoContextExtensions.GetOrCreateContext();

            UmbracoContext.Current.PublishedContentRequest = new PublishedContentRequest(
                UmbracoContext.Current.HttpContext.Request.Url,
                UmbracoContext.Current.RoutingContext,
                UmbracoConfig.For.UmbracoSettings().WebRouting,
                s => Roles.Provider.GetRolesForUser(s));

            UmbracoContext.Current.PublishedContentRequest.Prepare();
        }

        public void OnPerformed(PerformedContext filterContext)
        {
        }
    }
}