namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
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
        private readonly ILecturesService _lectures;
        private readonly IAssessmentsService _assessments;
        private readonly IRepository<CourseSubscription> _courseSubscriptions;

        private readonly ICoursesService _coursesService;

        public SubscriptionsService(IRepository<User> users, IRepository<CourseSubscription> courseSubscriptions, IRepository<Course> courses, ICoursesService coursesService, IUserService usersService, ILecturesService lectures, IAssessmentsService assessments)
        {
            _users = users;
            _courseSubscriptions = courseSubscriptions;
            _courses = courses;
            _coursesService = coursesService;
            _usersService = usersService;
            _lectures = lectures;
            _assessments = assessments;
        }

        public IEnumerable<CourseProgress> GetCoursesProgress(string username)
        {
            var user = _usersService.GetByUsername(username);
            var userSubscriptions = _courseSubscriptions.All().Where(x => x.UserId == user.Id).ToList();
            List<CourseProgress> courseProgresses = new List<CourseProgress>(userSubscriptions.Count);
            foreach (var subscription in userSubscriptions)
            {
                var courseProgress = new CourseProgress();
                courseProgress.CourseId = subscription.CourseId;
                courseProgress.TotalLecturesCount = _lectures.GetLecturesCount(subscription.CourseId);
                courseProgress.SubscriptionStatus = subscription.Status;

                if (subscription.Status == SubscriptionStatus.Active)
                {
                    courseProgress.VisitedLecturesCount = _lectures.GetLectureVisitsCount(username, subscription.CourseId);
                    courseProgress.AssessmentPassed = _assessments.HasSuccessfulSubmission(username, subscription.CourseId);
                }
                courseProgresses.Add(courseProgress);
            }

            return courseProgresses;
        }

        public bool HasActiveSubscription(string username, int courseId)
        {
            SubscriptionStatus status = GetSubscriptionStatus(username, courseId);
            return status == SubscriptionStatus.Active;
        }

        public bool IsEligibleForAssessment(string username, int courseId)
        {
            bool hasActiveSubscription = HasActiveSubscription(username, courseId);
            List<int> unvisitedLectures;
            if (hasActiveSubscription && _lectures.HasVisitedAllLectures(username, courseId, out unvisitedLectures))
            {
                return true;
            }

            return false;
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
