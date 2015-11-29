namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions
{
    using System.Web;

    using global::Umbraco.Core;
    using global::Umbraco.Web;

    public static class UmbracoContextExtensions
    {
         public static UmbracoContext GetOrCreateContext()
        {
            if (UmbracoContext.Current == null)
            {
                UmbracoContext.EnsureContext(new HttpContextWrapper(HttpContext.Current), ApplicationContext.Current);
            }

            return UmbracoContext.Current;
        }
    }
}