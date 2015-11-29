namespace AcademyPlatform.Services.Contracts
{
    public interface ISubscriptionsService
    {
        void JoinCourse(string username, int courseId);
        void JoinCourse(int userId, int courseId);

        bool HasSubscription(string username, int courseId);

        bool IsLectureVisited(string username, int lectureId);

        void TrackLectureVisit(string username, int lectureId);
    }
}