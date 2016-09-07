namespace AcademyPlatform.Web.Validators.Account
{
    using AcademyPlatform.Web.Models.Account;

    using FluentValidation;

    public class ChangePasswordViewModelValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordViewModelValidator()
        {
            RuleFor(x => x.OldPassword).NotNull();
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.NewPassword).Equal(x => x.ConfirmPassword).WithMessage("Паролите не съвпадат!");
            RuleFor(x => x.NewPassword).Length(6, 100).WithMessage("Моля изберете парола по-дълга от {MinLength} символа");
            RuleFor(x => x.ConfirmPassword).NotNull();
        }
    }
}
