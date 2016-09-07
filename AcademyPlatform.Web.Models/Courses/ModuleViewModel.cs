namespace AcademyPlatform.Web.Models.Courses
{
    using System.Collections.Generic;

    public class ModuleViewModel
    {
        public ModuleViewModel()
        {
            LectureLinks = new List<LectureLinkViewModel>();
        }

        public string Name { get; set; }

        public ICollection<LectureLinkViewModel> LectureLinks { get; set; }
    }
}