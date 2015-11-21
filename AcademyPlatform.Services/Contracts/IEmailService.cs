namespace AcademyPlatform.Services.Contracts
{
    public interface IEmailService
    {
        bool SendMail(string recipient, string body, string subject);
    }
}