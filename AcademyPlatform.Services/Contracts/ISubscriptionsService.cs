namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Courses;

    public interface ISubscriptionsService
    {
        SubscriptionStatus JoinCourse(string username, int courseId);

        bool HasActiveSubscription(string username, int courseId);

        SubscriptionStatus GetSubscriptionStatus(string username, int courseId);

        IEnumerable<CourseProgress> GetCoursesProgress(string username);
    }
}