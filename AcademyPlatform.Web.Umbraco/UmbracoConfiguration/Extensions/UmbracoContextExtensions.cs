namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions
{
    using System.IO;
    using System.Web;
    using System.Web.Hosting;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Configuration;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;
    using global::Umbraco.Web.Security;

    public static class UmbracoContextExtensions
    {
        public static UmbracoContext GetOrCreateContext()
        {
            if (UmbracoContext.Current == null)
            {
                HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current ?? new HttpContext(new SimpleWorkerRequest("/", "", new StringWriter())));

                UmbracoContext.EnsureContext(
                    httpContext,
                    ApplicationContext.Current,
                    new WebSecurity(httpContext, ApplicationContext.Current),
                    UmbracoConfig.For.UmbracoSettings(),
                    UrlProviderResolver.Current.Providers,
                    false);
            }

            return UmbracoContext.Current;
        }
    }
}