namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;

    public interface ISubscriptionsService
    {
        SubscriptionStatus JoinCourse(string username, int courseId);

        bool HasActiveSubscription(string username, int courseId);

        SubscriptionStatus GetSubscriptionStatus(string username, int courseId);

        IEnumerable<CourseProgress> GetCoursesProgress(string username);

        bool IsEligibleForAssessment(string username, int courseId);

        void AddPayment(Payment payment);
    }
}