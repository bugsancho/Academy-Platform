namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Web.Mvc;

    using AcademyPlatform.Web.Infrastructure.Filters;

    using global::Umbraco.Core;

    public class FiltersConfig : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            GlobalFilters.Filters.Add(new ExceptionFilter());
            //GlobalFilters.Filters.Add(new RequireHttpsAttribute());
        }
    }
}