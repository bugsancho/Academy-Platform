[assembly: Microsoft.Owin.OwinStartup("AcademyPlatformOwinStartup", typeof(AcademyPlatform.Web.Umbraco.Startup.AcademyPlatformOwinStartup))]
namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Web.Security;

    using global::Umbraco.Core.Configuration;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    using Hangfire;
    using Hangfire.Common;
    using Hangfire.Dashboard;
    using Hangfire.Server;
    using Hangfire.SqlServer;

    using Microsoft.Owin;

    using Newtonsoft.Json;

    using Owin;


    public class AcademyPlatformOwinStartup : UmbracoDefaultOwinStartup
    {
        public override void Configuration(IAppBuilder app)
        {
            base.Configuration(app);

            GlobalConfiguration.Configuration.UseLog4NetLogProvider();
            GlobalConfiguration.Configuration.UseSqlServerStorage("Hangfire", new SqlServerStorageOptions
                                                                                                              {
                                                                                                                  JobExpirationCheckInterval = TimeSpan.FromDays(7),
                                                                                                                  QueuePollInterval = TimeSpan.FromMinutes(1)
                                                                                                              });

            GlobalJobFilters.Filters.Add(new PrepareUmbracoRequestFilter());
            JobHelper.SetSerializerSettings(new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions { AuthorizationFilters = new IAuthorizationFilter[] { new RoleAuthorizationFilter() } });
        }
    }

    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            OwinContext owinContext = new OwinContext(owinEnvironment);
            return owinContext.Request.User.IsInRole("Hangfire");
            //TODO add authorization for local requests
            //string remoteIpAddress = owinContext.Request.RemoteIpAddress;
            //return !string.IsNullOrEmpty(remoteIpAddress) && (remoteIpAddress == "127.0.0.1" || remoteIpAddress == "::1" || remoteIpAddress == owinContext.Request.LocalIpAddress);
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