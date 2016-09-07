namespace AcademyPlatform.Services
{
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<Feedback> _feedback;

        public FeedbackService(IRepository<Feedback> feedback)
        {
            _feedback = feedback;
        }

        public void Create(Feedback feedback)
        {
            _feedback.Add(feedback);
            _feedback.SaveChanges();
        }

        public bool UserHasSentFeedback(string username, int courseId)
        {
            bool feedbackSent = _feedback.All().Any(x => x.User.Username == username && x.CourseId == courseId);
            return feedbackSent;

        }
    }
}
