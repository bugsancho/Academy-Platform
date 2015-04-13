namespace AcademyPlatform.Common.Validators
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public interface IValidator
    {
        bool Validate(object instance);

        ICollection<ValidationResult> GetValidationResults(object instance);
    }
}