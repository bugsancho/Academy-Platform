namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Mvc;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Services.Contracts;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;

    using global::Umbraco.Core;
    using global::Umbraco.Web;

    using Zone.UmbracoMapper;

    public class AutofacConfig : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => UmbracoContext.Current).AsSelf();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly);


            builder.RegisterAssemblyTypes(typeof(ICoursesService).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IRepository<>).Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(IUmbracoMapper).Assembly).AsImplementedInterfaces();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>));
            builder.RegisterType(typeof(UmbracoMapper)).As(typeof(IUmbracoMapper));

            var container = builder.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}