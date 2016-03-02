namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System.Configuration;

    using AcademyPlatform.Services.Contracts;

    public class MailSettingsProvider : IMailSettingsProvider
    {
        public string FromEmail => ConfigurationManager.AppSettings[nameof(FromEmail)];

        public string FromEmailName => ConfigurationManager.AppSettings[nameof(FromEmailName)];

        public string BccEmail => ConfigurationManager.AppSettings[nameof(BccEmail)];

        public bool BccAllEmails => bool.Parse(ConfigurationManager.AppSettings[nameof(BccAllEmails)]);

    }
}