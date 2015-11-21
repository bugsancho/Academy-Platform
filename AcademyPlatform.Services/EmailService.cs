namespace AcademyPlatform.Services
{
    using System.Net;
    using System.Net.Mail;

    using AcademyPlatform.Services.Contracts;

    public class EmailService : IEmailService
    {
        private SmtpClient _smtp;

        public EmailService()
        {
            _smtp = new SmtpClient();
        }

        public bool SendMail(string recipient, string body, string subject)
        {
            //using (var message = new MailMessage(from ))
            //{
            //    message.Body = body;
            //    //_smtp.Send(message);
            //}
            return true;
        }
    }
}
