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

        //TODO move to separate service
        public bool IsLectureVisited(string username, int lectureId)
        {
            User user = _users.All().FirstOrDefault(x => x.Username == username);
            return user != null && user.LectureVisits.Any(x => x.LectureId == lectureId);
        }

        public void TrackLectureVisit(string username, int lectureId)
        {
            User user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            var lectureVisit = user.LectureVisits.FirstOrDefault(x => x.LectureId == lectureId);
            if (lectureVisit == null)
            {
                user.LectureVisits.Add(new LectureVisit { User = user, LectureId = lectureId, LastVisitDate = DateTime.Now });
            }
            else
            {
                lectureVisit.LastVisitDate = DateTime.Now;
            }

            _users.SaveChanges();
        }

        public bool HasSubscription(string username, int courseId)
        {
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            var course = _courses.GetById(courseId);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }
            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            if (_courseSubscriptions.All().Any(x => x.UserId == user.Id && x.CourseId == courseId))
            {
                return true;
            }

            return false;
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
                throw new CourseNotFoundException(courseId);
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
