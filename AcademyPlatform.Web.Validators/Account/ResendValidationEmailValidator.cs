using System.Net.Mail;

namespace AcademyPlatform.Web.Validators.Account
{
    using FluentValidation;
    using AcademyPlatform.Web.Models.Account;

    public class ResendValidationEmailValidator : AbstractValidator<ResendValidationEmailViewModel>
    {
        public ResendValidationEmailValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Моля полълнете полето \"E-Mail\".");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Моля, въвдете валиден e-mail адрес.");
        }
    }
}
