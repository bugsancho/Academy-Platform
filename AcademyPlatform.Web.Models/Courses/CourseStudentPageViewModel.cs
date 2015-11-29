namespace AcademyPlatform.Web.Models.Courses
{
    using System.Collections.Generic;

    public class CourseStudentPageViewModel
    {
        public CourseStudentPageViewModel()
        {
            Modules = new List<ModuleViewModel>();
        }

        public string Title { get; set; }

        public string LecturerName { get; set; }

        public string DetailedDescription { get; set; }

        public ICollection<ModuleViewModel> Modules { get; set; }
    }
}
