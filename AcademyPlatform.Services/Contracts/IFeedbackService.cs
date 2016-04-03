namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Courses;

    public interface IFeedbackService
    {
        void Create(Feedback feedback);

        bool UserHasSentFeedback(string username, int courseId);
    }
}