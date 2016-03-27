namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Emails;

    public interface IMessageService
    {
        void SendInquiryRecievedMessage(Inquiry inquiry);

        void SendAccountValidationMessage(User user, string validationLink);

        void SendForgotPasswordMessage(User user, string newPassword);
        
        void SendPaymentApprovedMessage(User user, Course course);

        void SendExamAvailableMessage(User user, Course course);

        void SendExamSuccessfulMessage(User user, Course course);

        void SendFreeCourseSignUpMessage(User user, Course course);

        void SendPaidCourseSignUpMessage(User user, Course course);
    }
}