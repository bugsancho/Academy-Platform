namespace AcademyPlatform.Services
{
    using System.Net.Mail;

    using AcademyPlatform.Services.Contracts;

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtp;

        public EmailService()
        {
            _smtp = new SmtpClient();
        }

        public bool SendMail(string recipient, string body, string subject)
        {
            using (var message = new MailMessage("alexander.todorov@soft-solutions.bg", recipient))
            {
                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                _smtp.Send(message);
            }

            return true;
        }
    }
}
