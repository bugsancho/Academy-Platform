namespace AcademyPlatform.Models.Courses
{
    using System;

    using AcademyPlatform.Models.Assessments;

    //TODO move to reflect the fact that this is not a DB entity
    public class CourseProgress
    {
        public int CourseId { get; set; }

        public SubscriptionStatus SubscriptionStatus { get; set; }

        public AssessmentEligibilityStatus AssessmentEligibilityStatus { get; set; }

        public int VisitedLecturesCount { get; set; }

        public int TotalLecturesCount { get; set; }

        public DateTime? LockoutLift { get; set; }

    }
}
