namespace AcademyPlatform.Models.Certificates
{
    public class CertificateGenerationInfo
    {
        public string TemplateFilePath { get; set; }

        public string BaseFilePath { get; set; }

        public PlaceholderInfo StudentName { get; set; }

        public PlaceholderInfo CourseName { get; set; }

        public PlaceholderInfo IssueDate { get; set; }

        public PlaceholderInfo QrCode { get; set; }

        public PlaceholderInfo CertificateNumber { get; set; }

        public PlaceholderInfo NumberOfHours { get; set; }

        public PlaceholderInfo ModuleNames { get; set; }

    }
}
