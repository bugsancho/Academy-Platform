namespace AcademyPlatform.Web.Infrastructure.Sanitizers
{
    public interface IHtmlSanitizer
    {
        string Sanitize(string html);
    }
}
