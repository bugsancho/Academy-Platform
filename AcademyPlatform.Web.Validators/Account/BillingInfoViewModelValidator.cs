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
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Моля попълнето полето \"Малко име\".");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Моля попълнето полето  \"Фамилно име\".");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Моля попълнето полето  \"Личен адрес\".");
            RuleFor(x => x.City).NotEmpty().WithMessage("Моля попълнето полето  \"Град\".");
            RuleFor(x => x.Company).NotEmpty().WithMessage("Моля попълнето полето  \"Име на организацията\".");
            RuleFor(x => x.CompanyId).NotEmpty().WithMessage("Моля попълнето полето  \"БУЛСТАТ (ЕИК)\".");
            RuleFor(x => x.CompanyId).Matches("^[0-9]*$").WithMessage("Моля въвдете валиден Булстат (ЕИК).");
        }
    }
}
