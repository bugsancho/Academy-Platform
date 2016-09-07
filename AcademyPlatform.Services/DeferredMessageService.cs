namespace AcademyPlatform.Services
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class DeferredMessageService : IMessageService
    {
        private readonly ITaskRunner _taskRunner;

        public DeferredMessageService(ITaskRunner taskRunner)
        {
            _taskRunner = taskRunner;
        }

        public void SendInquiryRecievedMessage(Inquiry inquiry)
        {
            _taskRunner.Run<IMessageService>(x => x.SendInquiryRecievedMessage(inquiry));
        }

        public void SendAccountValidationMessage(User user, string validationLink)
        {
            _taskRunner.Run<IMessageService>(x => x.SendAccountValidationMessage(user, validationLink));
        }

        public void SendForgotPasswordMessage(User user, string newPassword)
        {
            _taskRunner.Run<IMessageService>(x => x.SendForgotPasswordMessage(user, newPassword));
        }

        public void SendPaymentApprovedMessage(User user, Course course, Payment payment, string courseUrl, string coursePictureUrl)
        {
            _taskRunner.Run<IMessageService>(x => x.SendPaymentApprovedMessage(user, course, payment, courseUrl, coursePictureUrl));
        }

        public void SendExamAvailableMessage(User user, Course course, string assessmentUrl)
        {
            _taskRunner.Run<IMessageService>(x => x.SendExamAvailableMessage(user, course, assessmentUrl));
        }

        public void SendExamSuccessfulMessage(User user, Course course, string certificateUrl, string certificatePictureUrl)
        {
            _taskRunner.Run<IMessageService>(x => x.SendExamSuccessfulMessage(user, course, certificateUrl, certificatePictureUrl));
        }

        public void SendFreeCourseSignUpMessage(User user, Course course, string courseUrl, string coursePictureUrl)
        {
            _taskRunner.Run<IMessageService>(x => x.SendFreeCourseSignUpMessage(user, course, courseUrl, coursePictureUrl));
        }

        public void SendPaidCourseSignUpMessage(User user, Course course, CourseSubscription subscription, string courseUrl, string coursePictureUrl)
        {
            _taskRunner.Run<IMessageService>(x => x.SendPaidCourseSignUpMessage(user, course, subscription, courseUrl, coursePictureUrl));
        }


    }
}
