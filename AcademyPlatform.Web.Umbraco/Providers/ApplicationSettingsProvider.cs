namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System.Configuration;

    using AcademyPlatform.Services.Contracts;

    public class ApplicationSettingsProvider : IApplicationSettings
    {
        public int AssessmentLockoutTime => int.Parse(ConfigurationManager.AppSettings[nameof(AssessmentLockoutTime)]);
    }
}