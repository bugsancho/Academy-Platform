namespace AcademyPlatform.Common.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class DataAnotationsValidator : IValidator
    {
        public bool Validate(object instance)
        {
            return Validator.TryValidateObject(instance, new ValidationContext(instance), new List<ValidationResult>(), true);
        }

        public ICollection<ValidationResult> GetValidationResults(object instance)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(instance, new ValidationContext(instance), results, true);
            return results;
        }
    }
}