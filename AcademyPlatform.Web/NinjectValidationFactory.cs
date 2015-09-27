namespace AcademyPlatform.Web
{
    using System;

    using FluentValidation;

    public class NinjectValidationFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return ObjectFactory.TryGetInstance(validatorType) as IValidator;
        }
    }
}