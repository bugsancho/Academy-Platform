namespace AcademyPlatform.Models.Courses
{
    //TODO move to reflect the fact that this is not a DB entity
    public class CourseProgress
    {
        public int CourseId { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public int VisitedLecturesCount { get; set; }

        public int TotalLecturesCount { get; set; }

        public bool AssessmentPassed { get; set; }

    }
}
