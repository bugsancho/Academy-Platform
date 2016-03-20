namespace AcademyPlatform.Web.Models.Common
{
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    public class FileViewModel : DocumentTypeBase
    {
        public string FileExtension { get; set; }

        public int Size { get; set; }
    }
}
