namespace AcademyPlatform.Web
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    using AcademyPlatform.Web.Infrastructure.Mappings;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Resources;

    using FluentValidation;
    using FluentValidation.Mvc;

    using log4net.Config;

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new NinjectValidationFactory()));
            ValidatorOptions.ResourceProviderType = typeof(ErrorMessages);
            var autoMapperConfig = new AutoMapperConfig(Assembly.GetAssembly(typeof(CourseViewModel)));
            autoMapperConfig.Execute();

            XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
        }

        protected void Application_RequestEnd()
        {
            Trace.WriteLine(string.Format("{0}:{1}",
         (DateTime.Now - HttpContext.Current.Timestamp).TotalMilliseconds,
         HttpContext.Current.Request.RawUrl));
        }
    }
}