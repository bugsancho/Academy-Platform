namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Courses;

    public interface ISubscriptionsService
    {
        void JoinCourse(string username, int courseId);

        bool HasActiveSubscription(string username, int courseId);

        bool IsLectureVisited(string username, int lectureId);

        void TrackLectureVisit(string username, int lectureId);

        SubscriptionStatus GetSubscriptionStatus(string username, int courseId);
    }
}