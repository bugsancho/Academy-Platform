namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions
{
    using System.Web;

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
                HttpContextWrapper httpContext = new HttpContextWrapper(HttpContext.Current);

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