namespace AcademyPlatform.Web.Validators.Account
{
    using AcademyPlatform.Web.Models.Account;

    using FluentValidation;

    public class BillingInfoViewModelValidator : AbstractValidator<BillingInfoViewModel>
    {
        public BillingInfoViewModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Моля попълнетe полето \"Малко име\".");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Моля попълнетe полето  \"Фамилно име\".");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Моля попълнетe полето  \"Личен адрес\".");
            RuleFor(x => x.City).NotEmpty().WithMessage("Моля попълнетe полето  \"Град\".");
            RuleFor(x => x.Company).NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.CompanyId)).WithMessage("Моля попълнетe полето  \"Име на организацията\".");
            RuleFor(x => x.CompanyId).NotEmpty().When(x => !string.IsNullOrWhiteSpace(x.Company)).WithMessage("Моля попълнетe полето  \"БУЛСТАТ (ЕИК)\".");
            RuleFor(x => x.CompanyId).Matches("^\\d{9,13}$").WithMessage("Моля въвдете валиден Булстат (ЕИК) - 9 или 13 цифрен код").When(x => !string.IsNullOrWhiteSpace(x.CompanyId));
        }
    }
}
