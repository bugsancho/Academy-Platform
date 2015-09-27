namespace AcademyPlatform.Web.Infrastructure.Helpers
{
    using System.Data.Entity.Infrastructure.Pluralization;

    //TODO Consider replacing Entity Framework reference with another service. Possibly https://github.com/MehdiK/Humanize

    public static class PluralizationHelper
    {
        private static readonly IPluralizationService PluralizationService = new EnglishPluralizationService();

        public static string Pluralize(string word)
        {
            return PluralizationService.Pluralize(word);
        }

        public static string Singularize(string word)
        {
            return PluralizationService.Singularize(word);
        }
    }
}
