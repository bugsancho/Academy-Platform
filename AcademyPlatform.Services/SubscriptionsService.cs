namespace AcademyPlatform.Services
{
    using System;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;

    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly IRepository<User> _users;

        private readonly IRepository<Course> _courses;

        private readonly IRepository<CourseSubscription> _courseSubscriptions;

        public SubscriptionsService(IRepository<User> users, IRepository<CourseSubscription> courseSubscriptions, IRepository<Course> courses)
        {
            _users = users;
            _courseSubscriptions = courseSubscriptions;
            _courses = courses;
        }

        public void JoinCourse(string username, int courseId)
        {
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            JoinCourse(user.Id, courseId);
        }

        public void JoinCourse(int userId, int courseId)
        {
            var user = _users.GetById(userId);
            var course = _courses.GetById(courseId);

            if (user == null)
            {
                throw new UserNotFoundException(userId.ToString());
            }

            if (course == null)
            {
                throw new ArgumentException($"Could not find course with id: {courseId}", nameof(courseId));
            }

            var existingSubscription = _courseSubscriptions.All().FirstOrDefault(x => x.UserId == userId && x.CourseId == courseId);
            if (existingSubscription != null)
            {
                existingSubscription.Status = SubscriptionStatus.Active;
                _courseSubscriptions.Update(existingSubscription);
            }
            else
            {
                var subscription = new CourseSubscription
                {
                    UserId = userId,
                    CourseId = courseId,
                    SubscriptionDate = DateTime.Now,
                    Status = SubscriptionStatus.Active
                };

                _courseSubscriptions.Add(subscription);
            }

            _courseSubscriptions.SaveChanges();

        }
    }
}
