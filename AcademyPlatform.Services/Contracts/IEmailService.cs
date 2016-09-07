namespace AcademyPlatform.Services.Contracts
{
    public interface IEmailService
    {
        bool SendMail(string recipient, string subject, string body);
    }
}