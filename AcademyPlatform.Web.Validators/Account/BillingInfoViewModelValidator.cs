namespace AcademyPlatform.Web.Validators.Account
{
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using FluentValidation;

    public class BillingInfoViewModelValidator : AbstractValidator<BillingInfoViewModel>
    {
        public BillingInfoViewModelValidator()
        {
           // RuleFor(x => x.FirstName).NotNull().WithMessage("Моля попълнете името - nested");
        }
    }
}
