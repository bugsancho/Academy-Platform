namespace AcademyPlatform.Web.Models.Courses
{
    using System.Collections.Generic;

    public class LectureViewModel
    {
        public LectureViewModel()
        {
            Modules = new List<ModuleViewModel>();
        }

        public int Id { get; set; }

        public string Content { get; set; }

        public ICollection<ModuleViewModel> Modules { get; set; }

        public LectureLinkViewModel PreviousLecture { get; set; }

        public LectureLinkViewModel NextLecture { get; set; }

    }
}
