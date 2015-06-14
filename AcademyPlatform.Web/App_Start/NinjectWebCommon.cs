[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AcademyPlatform.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(AcademyPlatform.Web.App_Start.NinjectWebCommon), "Stop")]

namespace AcademyPlatform.Web.App_Start
{
    using System;
    using System.Reflection;
    using System.Web;
    using AcademyPlatform.Data;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Services;
    using FluentValidation;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                ObjectFactory.InitializeKernel(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetAssembly(typeof(AcademyPlatform.Models.Courses.Validators.CourseValidator)))
                .ForEach(match => kernel.Bind(match.InterfaceType).To(match.ValidatorType));
            AssemblyScanner.FindValidatorsInAssembly(Assembly.GetAssembly(typeof(AcademyPlatform.Web.Models.Courses.Validators.CreateCourseViewModelValidator)))
                .ForEach(match => kernel.Bind(match.InterfaceType).To(match.ValidatorType));
            kernel.Bind(typeof(IRepository<>)).To(typeof(EfRepository<>));
            kernel.Bind<IAcademyPlatformDbContext>().To<AcademyPlatformDbContext>();
            kernel.Bind<ICoursesService>().To<CoursesService>();
        }
    }
}
