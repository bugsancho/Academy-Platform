namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Models.Payments;

    public interface IMessageService
    {
        void SendInquiryRecievedMessage(Inquiry inquiry);

        void SendAccountValidationMessage(User user, string validationLink);

        void SendForgotPasswordMessage(User user, string newPassword);
        
        void SendPaymentApprovedMessage(User user, Course course, Payment payment,string courseUrl, string coursePictureUrl);

        void SendExamAvailableMessage(User user, Course course,string assessmentUrl);

        void SendExamSuccessfulMessage(User user, Course course,string certificateUrl, string certificatePictureUrl);

        void SendFreeCourseSignUpMessage(User user, Course course,string courseUrl, string coursePictureUrl);

        void SendPaidCourseSignUpMessage(User user, Course course, CourseSubscription subscription,string courseUrl, string coursePictureUrl);
    }
}