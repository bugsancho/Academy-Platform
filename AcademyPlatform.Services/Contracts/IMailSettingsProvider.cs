namespace AcademyPlatform.Services.Contracts
{
    public interface IMailSettingsProvider
    {
        string FromEmail { get; }

        string FromEmailName { get; }

        string BccEmail { get; }

        bool BccAllEmails { get; }
    }
}
