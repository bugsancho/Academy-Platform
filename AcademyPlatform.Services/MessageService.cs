namespace AcademyPlatform.Services
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Services.Contracts;

    public class MessageService : IMessageService
    {
        private readonly IMessageTemplateProvider _templateProvider;
        private readonly IEmailService _emailService;

        public MessageService(IMessageTemplateProvider templateProvider, IEmailService emailService)
        {
            _templateProvider = templateProvider;
            _emailService = emailService;
        }

        public void SendAccountValidationMessage(User user, string validationLink)
        {
            MessageTemplate template = _templateProvider.GetAccountValidationTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            // Umbraco's TinyMCE inserts forward slash ('/') at the begining of href's, so we have to manually remove it as part of the link insertion
            template.Body = template.Body.Replace("/{{validationLink}}", validationLink);

            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendForgotPasswordMessage(User user, string newPassword)
        {
            MessageTemplate template = _templateProvider.GetForgotPasswordTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            template.Body = template.Body.Replace("{{username}}", user.Username);
            template.Body = template.Body.Replace("{{password}}", newPassword);

            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendFreeCourseSignUpMessage(User user, Course course)
        {
            MessageTemplate template = _templateProvider.GetFreeCourseSignUpTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            //TODO add placeholders
            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendPaidCourseSignUpMessage(User user, Course course)
        {
            MessageTemplate template = _templateProvider.GetPaidCourseSignUpTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            //TODO add placeholders
            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendPaymentApprovedMessage(User user, Course course)
        {
            MessageTemplate template = _templateProvider.GetPaymentApprovedTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            //TODO add placeholders
            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendExamAvailableMessage(User user, Course course)
        {
            MessageTemplate template = _templateProvider.GetExamAvailableTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            //TODO add placeholders
            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }

        public void SendExamSuccessfulMessage(User user, Course course)
        {
            MessageTemplate template = _templateProvider.GetExamSuccessfulTemplate();
            template.Body = template.Body.Replace("{{firstName}}", user.FirstName);
            //TODO add placeholders
            _emailService.SendMail(user.Username, template.Subject, template.Body);
        }
    }
}
