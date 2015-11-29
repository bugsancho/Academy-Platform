namespace AcademyPlatform.Web.Models.Courses
{
    using System.Collections.Generic;

    public class LectureViewModel
    {
        public LectureViewModel()
        {
            OtherLectures = new List<LectureLinkViewModel>();
        }
        public string Content { get; set; }

        public ICollection<LectureLinkViewModel> OtherLectures { get; set; }

        public LectureLinkViewModel PreviousLecture { get; set; }

        public LectureLinkViewModel NextLecture { get; set; }

    }
}
