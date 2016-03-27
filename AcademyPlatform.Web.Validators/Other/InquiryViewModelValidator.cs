namespace AcademyPlatform.Web.Validators.Other
{
    using AcademyPlatform.Web.Models.Other;

    using FluentValidation;

    public class InquiryViewModelValidator : AbstractValidator<InquiryViewModel>
    {
        public InquiryViewModelValidator()
        {
            RuleFor(x => x.CustomerName).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Subject).NotEmpty();
            RuleFor(x => x.Message).NotEmpty();
        }
    }
}
