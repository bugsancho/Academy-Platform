namespace AcademyPlatform.Web
{
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using AcademyPlatform.Web.Infrastructure.Mappings;
    using FluentValidation.Mvc;
    using FluentValidation;
    using AcademyPlatform.Web.Resources;

    public class MvcApplication : System.Web.HttpApplication
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
            var autoMapperConfig = new AutoMapperConfig(Assembly.GetAssembly(typeof(AcademyPlatform.Web.Models.Courses.CourseViewModel)));
            autoMapperConfig.Execute();
        }
    }
}