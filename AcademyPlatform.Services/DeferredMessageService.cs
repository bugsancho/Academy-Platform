namespace AcademyPlatform.Services
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    public class DeferredMessageService : IMessageService
    {
        private readonly ITaskRunner _taskRunner;

        public DeferredMessageService(ITaskRunner taskRunner)
        {
            _taskRunner = taskRunner;
        }

        public void SendAccountValidationMessage(User user, string validationLink)
        {
            _taskRunner.Run<IMessageService>(x => x.SendAccountValidationMessage(user, validationLink));
        }

        public void SendForgotPasswordMessage(User user, string newPassword)
        {
            _taskRunner.Run<IMessageService>(x => x.SendForgotPasswordMessage(user, newPassword));
        }

        public void SendPaymentApprovedMessage(User user, Course course)
        {
            _taskRunner.Run<IMessageService>(x => x.SendPaymentApprovedMessage(user, course));
        }

        public void SendExamAvailableMessage(User user, Course course)
        {
            _taskRunner.Run<IMessageService>(x => x.SendExamAvailableMessage(user, course));
        }

        public void SendExamSuccessfulMessage(User user, Course course)
        {
            _taskRunner.Run<IMessageService>(x => x.SendExamSuccessfulMessage(user, course));
        }

        public void SendFreeCourseSignUpMessage(User user, Course course)
        {
            _taskRunner.Run<IMessageService>(x => x.SendFreeCourseSignUpMessage(user, course));
        }

        public void SendPaidCourseSignUpMessage(User user, Course course)
        {
            _taskRunner.Run<IMessageService>(x => x.SendPaidCourseSignUpMessage(user, course));
        }
    }
}
