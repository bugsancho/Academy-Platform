namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models;

    public interface IMessageService
    {
        void SendAccountValidationMessage(User user, string validationLink);

        void SendForgotPasswordMessage(User user, string newPassword);

        void SendCourseSignUpMessage();

        void SendPaymentApprovedMessage();

        void SendExamAvailableMessage();

        void SendExamSuccessfulMessage();
    }
}