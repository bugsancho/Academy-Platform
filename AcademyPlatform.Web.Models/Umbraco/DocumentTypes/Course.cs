namespace AcademyPlatform.Web.Models.Umbraco.DocumentTypes
{
    using System.Collections.Generic;

    using AcademyPlatform.Web.Models.Common;

    public class Course : DocumentTypeBase
    {
        public Course()
        {
            Modules = new List<Module>();
            Files = new List<FileViewModel>();
        }

        public int CourseId { get; set; }

        public int Certificate { get; set; }

        public ImageViewModel CoursePicture { get; set; }

        public ImageViewModel PartnerLogo { get; set; }

        public ImageViewModel SampleCertificate { get; set; }

        public PageLink PartnerPage { get; set; }

        public int LicenseTerms { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string Features { get; set; }

        public string ModulesNames { get; set; }

        public int NumberOfHours { get; set; }

        public string Assessment { get; set; }

        public int FeedbackForm { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
