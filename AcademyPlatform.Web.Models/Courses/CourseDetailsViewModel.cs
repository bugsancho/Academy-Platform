namespace AcademyPlatform.Web.Models.Courses
{
    using AcademyPlatform.Models.Courses;

    public class CourseDetailsViewModel
    {
         public int CourseId { get; set; }

         public string Title { get; set; }

        public string CoursesPageUrl { get; set; }

        public string JoinCourseUrl { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }

        public string LecturerName { get; set; }

        public string DetailedDescription { get; set; }
    }
}
