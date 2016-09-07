namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System.Reflection;
    using System.Web.Mvc;

    using AcademyPlatform.Common.Providers;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Services;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Mappings;
    using AcademyPlatform.Web.Infrastructure.Sanitizers;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.Providers;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using Autofac;
    using Autofac.Integration.Mvc;
    using Autofac.Integration.WebApi;

    using FluentValidation;

    using global::Umbraco.Core;
    using global::Umbraco.Web;

    using Zone.UmbracoMapper;
    using Hangfire;

    public class AutofacConfig : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            var builder = new ContainerBuilder();

            builder.Register(c => UmbracoContext.Current).AsSelf().InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetExecutingAssembly()).InstancePerDependency();
            builder.RegisterControllers(typeof(UmbracoApplication).Assembly).InstancePerLifetimeScope();
            builder.RegisterApiControllers(typeof(UmbracoApplication).Assembly).InstancePerLifetimeScope();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerLifetimeScope();

            builder.RegisterFluentValidators(typeof(AcademyPlatform.Web.Validators.AssemblyInfo).Assembly);

            builder.RegisterAssemblyTypes(typeof(ICoursesService).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(ICoursesContentService).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRepository<>).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IHtmlSanitizer).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IRandomProvider).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(IUmbracoMapper).Assembly).AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(UmbracoMapper)).As(typeof(IUmbracoMapper)).InstancePerLifetimeScope();

            builder.RegisterType<ApplicationSettingsProvider>().As<IApplicationSettings>().InstancePerLifetimeScope();
            builder.RegisterType(typeof(MailSettingsProvider)).As(typeof(IMailSettingsProvider)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(TaskRunner)).As(typeof(ITaskRunner)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(RouteProvider)).As(typeof(IRouteProvider)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(MessageTemplateProvider)).As(typeof(IMessageTemplateProvider)).InstancePerLifetimeScope();
            builder.RegisterType(typeof(CertificateGenerationInfoProvider)).As<ICertificateGenerationInfoProvider>().InstancePerLifetimeScope();

            builder.RegisterType<DeferredMessageService>().As<IMessageService>();

            IContainer container = builder.Build();
            System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            Hangfire.GlobalConfiguration.Configuration.UseAutofacActivator(container.BeginLifetimeScope(b => b.RegisterType<MessageService>().As<IMessageService>()));

            //TODO MOVE
            AutoMapperConfig autoMapperConfig = new AutoMapperConfig(Assembly.GetAssembly(typeof(CourseEditViewModel)));
            autoMapperConfig.Execute();
        }


    }

    public static class BuilderExtensions
    {
        public static void RegisterFluentValidators(this ContainerBuilder builder, Assembly assembly)
        {
            AssemblyScanner.FindValidatorsInAssembly(assembly).ForEach(
                result =>
                {
                    builder.RegisterType(result.ValidatorType).As(result.InterfaceType);
                });
        }
    }
}