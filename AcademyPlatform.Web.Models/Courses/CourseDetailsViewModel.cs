namespace AcademyPlatform.Web.Models.Courses
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Courses;

    public class CourseDetailsViewModel
    {
        public CourseDetailsViewModel()
        {
            Modules = new List<ModuleViewModel>();
        }

        public int CourseId { get; set; }

        public string Title { get; set; }

        public string CoursesPageUrl { get; set; }

        public string JoinCourseUrl { get; set; }

        public string AssessmentUrl { get; set; }

        public string ProfileUrl { get; set; }

        public string ImageUrl { get; set; }

        public Category Category { get; set; }

        public string LecturerName { get; set; }

        public string DetailedDescription { get; set; }

        public ICollection<ModuleViewModel> Modules { get; set; }

        public string ShortDescription { get; set; }

        public bool HasActiveSubscription { get; set; }

        public AssessmentEligibilityStatus AssessmentEligibilityStatus { get; set; }

        public string ErrorMessage { get; set; }
    }
}
