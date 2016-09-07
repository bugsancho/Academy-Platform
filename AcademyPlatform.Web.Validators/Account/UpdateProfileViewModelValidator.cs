namespace AcademyPlatform.Web.Validators.Account
{
    using AcademyPlatform.Web.Models.Account;

    using FluentValidation;

    public class UpdateProfileViewModelValidator : AbstractValidator<UpdateProfileViewModel>
    {
        public UpdateProfileViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotNull().WithMessage("Моля, попълнете Вашето име");
            RuleFor(x => x.MiddleName).NotNull().WithMessage("Моля, попълнете Вашето презиме");
            RuleFor(x => x.LastName).NotNull().WithMessage("Моля, попълнете Вашата фамилия");
        }
    }
}
