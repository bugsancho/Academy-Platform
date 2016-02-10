namespace AcademyPlatform.Web.Models.Umbraco.DocumentTypes
{
    public class Lecture : DocumentTypeBase
    {
        public string Content { get; set; }

        public int IsDemo { get; set; }
    }
}