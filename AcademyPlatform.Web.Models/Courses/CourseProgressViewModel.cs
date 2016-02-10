namespace AcademyPlatform.Web.Models.Courses
{
    using AcademyPlatform.Models.Courses;

    public class CourseProgressViewModel
    {
        public string CourseUrl { get; set; }

        public string CourseName { get; set; }

        public CourseProgress CourseProgress { get; set; }

        public string AssessmentUrl { get; set; }

        public string AwaitingPaymentUrl { get; set; }
    }
}
