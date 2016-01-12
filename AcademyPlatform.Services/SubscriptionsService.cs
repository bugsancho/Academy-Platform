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
        private readonly IUserService _usersService;
        private readonly IRepository<Course> _courses;
        private readonly IRepository<CourseSubscription> _courseSubscriptions;

        private readonly ICoursesService _coursesService;

        public SubscriptionsService(IRepository<User> users, IRepository<CourseSubscription> courseSubscriptions, IRepository<Course> courses, ICoursesService coursesService, IUserService usersService)
        {
            _users = users;
            _courseSubscriptions = courseSubscriptions;
            _courses = courses;
            _coursesService = coursesService;
            _usersService = usersService;
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

            LectureVisit lectureVisit = user.LectureVisits.FirstOrDefault(x => x.LectureId == lectureId);
            if (lectureVisit == null)
            {
                user.LectureVisits.Add(new LectureVisit { User = user, LectureId = lectureId, LastVisitDate = DateTime.Now });
            }
            else
            {
                lectureVisit.LastVisitDate = DateTime.Now;
            }

            //TODO implement proper Unit of Work 
            _users.SaveChanges();
        }

        public bool HasActiveSubscription(string username, int courseId)
        {
            SubscriptionStatus status = GetSubscriptionStatus(username, courseId);
            return status == SubscriptionStatus.Active;
        }

        public SubscriptionStatus GetSubscriptionStatus(string username, int courseId)
        {
            User user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            Course course = _courses.GetById(courseId);
            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            CourseSubscription subscription = _courseSubscriptions.All().FirstOrDefault(x => x.UserId == user.Id && x.CourseId == courseId);
            if (subscription != null)
            {
                return subscription.Status;
            }

            return SubscriptionStatus.None;
        }

        public SubscriptionStatus JoinCourse(string username, int courseId)
        {
            User user = _usersService.GetByUsername(username);
            Course course = _courses.GetById(courseId);

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            if (course == null)
            {
                throw new CourseNotFoundException(courseId);
            }

            CourseSubscription subscription = _courseSubscriptions.All().FirstOrDefault(x => x.UserId == user.Id && x.CourseId == courseId);
            if (subscription == null)
            {
                bool isPaidCourse = _coursesService.IsPaidCourse(course);
                subscription = new CourseSubscription
                {
                    UserId = user.Id,
                    CourseId = courseId,
                    RequestedDate = DateTime.Now
                };

                if (isPaidCourse)
                {
                    subscription.Status = SubscriptionStatus.AwaitingPayment;
                    subscription.SubscriptionType = SubscriptionType.Paid;
                }
                else
                {
                    subscription.Status = SubscriptionStatus.Active;
                    subscription.SubscriptionType = SubscriptionType.Free;
                    subscription.ApprovedDate = DateTime.Now;
                }

                _courseSubscriptions.Add(subscription);
                //TODO implement proper Unit of Work 
                _courseSubscriptions.SaveChanges();
            }

            return subscription.Status;
        }
    }
}
