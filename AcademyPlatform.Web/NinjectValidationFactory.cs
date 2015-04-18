namespace AcademyPlatform.Web
{
    using System;
    using System.Linq;
    using FluentValidation;

    public class NinjectValidationFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            return ObjectFactory.TryGetInstance(validatorType) as IValidator;
        }
    }
}