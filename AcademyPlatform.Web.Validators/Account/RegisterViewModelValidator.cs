namespace AcademyPlatform.Web.Validators.Account
{
    using AcademyPlatform.Web.Models.Account;

    using FluentValidation;

    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.FirstName).NotNull();
            RuleFor(x => x.LastName).NotNull();
            RuleFor(x => x.Password).NotNull();
            RuleFor(x => x.Password).Length(6, 100).WithMessage("Моля изберете парола по-дълга от {MinLength} символа");
            RuleFor(x => x.ConfirmPassword).NotNull();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.ConfirmPassword).WithMessage("Паролите не съвпадат!");
            RuleFor(x => x.AcceptLicenseTerms).Equal(true).WithMessage("Трябва да приемете общите условия");
        }
    }
}