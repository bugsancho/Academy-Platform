﻿namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System;
    using System.Web.Mvc;

    using FluentValidation;
    using FluentValidation.Mvc;

    using global::Umbraco.Core;

    using Resources;

    public class FluentValidationConfig
    {
        public class FluentValidationSetup : ApplicationEventHandler
        {
            protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
            {
                FluentValidationModelValidatorProvider.Configure(
                    config =>
                    {
                        config.ValidatorFactory = new DependencyResolverValidationFactory();
                    });

                 ValidatorOptions.ResourceProviderType = typeof(ErrorMessages);
            }

            private class DependencyResolverValidationFactory : ValidatorFactoryBase
            {
                public override IValidator CreateInstance(Type validatorType)
                {
                    var validator = DependencyResolver.Current.GetService(validatorType) as IValidator;
                    return validator;
                }
            }
        }
    }
}