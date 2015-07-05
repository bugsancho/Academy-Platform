namespace AcademyPlatform.Web.Infrastructure.Sanitizers
{
    public class HtmlSanitizer : IHtmlSanitizer
    {
        private static readonly Ganss.XSS.HtmlSanitizer sanitizer;

        static HtmlSanitizer()
        {
            sanitizer = new Ganss.XSS.HtmlSanitizer();
        }

        public string Sanitize(string html)
        {
            return sanitizer.Sanitize(html);
        }
    }
}
