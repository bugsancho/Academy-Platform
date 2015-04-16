namespace AcademyPlatform.Web
{
    using System.Reflection;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using AcademyPlatform.Web.Infrastructure.Mappings;
    using FluentValidation;
    using FluentValidation.Mvc;

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

            ModelValidatorProviders.Providers.Clear();
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(new NinjectValidationFactory()));
            
            //FluentValidationModelValidatorProvider.Configure(provider => provider.ValidatorFactory = new NinjectValidationFactory());

            //AssemblyScanner.FindValidatorsInAssemblyContaining<IValidator>()
            //               .ForEach(result =>
            //               {
            //                   For(result.InterfaceType).Singleton().Use(result.ValidatorType);
            //               });

            var autoMapperConfig = new AutoMapperConfig(Assembly.GetExecutingAssembly());
            autoMapperConfig.Execute();
        }
    }
}