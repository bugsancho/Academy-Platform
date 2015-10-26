namespace AcademyPlatform.Services.Contracts
{
    public interface ISubscriptionsService
    {
        void JoinCourse(string username, int courseId);
        void JoinCourse(int userId, int courseId);
    }
}