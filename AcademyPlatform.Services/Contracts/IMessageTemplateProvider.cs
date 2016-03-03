namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Emails;

    public interface IMessageTemplateProvider
    {
        MessageTemplate GetAccountValidationTemplate();

        MessageTemplate GetForgotPasswordTemplate();
        
        MessageTemplate GetPaymentApprovedTemplate();

        MessageTemplate GetExamAvailableTemplate();

        MessageTemplate GetExamSuccessfulTemplate();

        MessageTemplate GetPaidCourseSignUpTemplate();

        MessageTemplate GetFreeCourseSignUpTemplate();
    }
}
