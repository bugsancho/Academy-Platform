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

        public int  Certificate { get; set; }

        public ImageViewModel CoursePicture { get; set; }

        public int LicenseTerms { get; set; }

        public string ShortDescription { get; set; }

        public string DetailedDescription { get; set; }

        public string Assessment { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public IEnumerable<FileViewModel> Files { get; set; }
    }
}
