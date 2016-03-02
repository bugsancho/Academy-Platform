namespace AcademyPlatform.Services
{
    using System.Net.Mail;

    using AcademyPlatform.Services.Contracts;

    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtp;
        private readonly IMailSettingsProvider _mailSettings;

        public EmailService(IMailSettingsProvider mailSettings)
        {
            _mailSettings = mailSettings;
            _smtp = new SmtpClient();
        }

        public bool SendMail(string recipient, string subject, string body)
        {
            using (var message = new MailMessage(new MailAddress(_mailSettings.FromEmail, _mailSettings.FromEmailName), new MailAddress(recipient)))
            {
                if (_mailSettings.BccAllEmails)
                {
                    message.Bcc.Add(_mailSettings.BccEmail);
                }

                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                _smtp.Send(message);
            }

            return true;
        }
    }
}
