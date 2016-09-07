namespace AcademyPlatform.Web.Validators.Subscriptions
{
    using System;
    using AcademyPlatform.Web.Models.Payments;
    using FluentValidation;


    public class PaymentEditViewModelValidator : AbstractValidator<PaymentEditViewModel>
    {
        public PaymentEditViewModelValidator()
        {
            RuleFor(x => x.BankAccount).NotEmpty().WithMessage("Моля въведете банкова сметка.");
            RuleFor(x => x.Details).NotEmpty().WithMessage("Моля въведете детайли за транзакцията");
            RuleFor(x => x.PaymentDate).NotEmpty().WithMessage("Моля въвдете дата на получаване на транзакцията - във формат ДД/ММ/ГГГГ");
            //RuleFor(x => x.PaymentDate).LessThanOrEqualTo(DateTime.Now)
            //    .WithMessage("Датата не може да бъде след сегашния момент.");
            RuleFor(x => x.Total).NotEmpty().WithMessage("Моля въвдете сумата на транзакцията");
            RuleFor(x => x.Total).GreaterThan(0).WithMessage("Моля въведете стойност по-голяма от 0.00");
        }
    }
}
