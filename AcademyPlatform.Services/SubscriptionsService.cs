namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class SubscriptionsService : ISubscriptionsService
    {
        private readonly IRepository<User> _users;
        private readonly IUserService _usersService;
        private readonly IRepository<Course> _courses;
        private readonly ILecturesService _lectures;
        private readonly IAssessmentsService _assessments;
        private readonly IMessageService _messageService;
        private readonly IRouteProvider _routeProvider;
        private readonly IRepository<CourseSubscription> _courseSubscriptions;
        private readonly IRepository<Payment> _payments;

        private readonly ICoursesService _coursesService;
        private readonly IOrdersService _ordersService;

        public SubscriptionsService(IRepository<User> users, IRepository<CourseSubscription> courseSubscriptions, IRepository<Course> courses, ICoursesService coursesService, IUserService usersService, ILecturesService lectures, IAssessmentsService assessments, IMessageService messageService, IRepository<Payment> payments, IRouteProvider routeProvider, IOrdersService ordersService)
        {
            _users = users;
            _courseSubscriptions = courseSubscriptions;
            _courses = courses;
            _coursesService = coursesService;
            _usersService = usersService;
            _lectures = lectures;
            _assessments = assessments;
            _messageService = messageService;
            _payments = payments;
            _routeProvider = routeProvider;
            _ordersService = ordersService;
        }

        public IEnumerable<CourseProgress> GetCoursesProgress(string username)
        {
            var user = _usersService.GetByUsername(username);
            var userSubscriptions = _courseSubscriptions.All().Where(x => x.UserId == user.Id).ToList();
            List<CourseProgress> courseProgresses = new List<CourseProgress>(userSubscriptions.Count);
            foreach (var subscription in userSubscriptions)
            {
                var courseProgress = new CourseProgress
                {
                    CourseId = subscription.CourseId,
                    TotalLecturesCount = _lectures.GetLecturesCount(subscription.CourseId),
                    SubscriptionStatus = subscription.Status,
                    AssessmentEligibilityStatus = _assessments.GetEligibilityStatus(username, subscription.CourseId)
                };

                if (subscription.Status == SubscriptionStatus.Active)
                {
                    courseProgress.VisitedLecturesCount = _lectures.GetLectureVisitsCount(username, subscription.CourseId);
                }

                if (courseProgress.AssessmentEligibilityStatus == AssessmentEligibilityStatus.Lockout)
                {
                    courseProgress.LockoutLift = _assessments.GetNextAssessmentAttemptDate(
                        username,
                        subscription.CourseId);
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

        public CourseSubscription GetSubscription(string username, int courseId)
        {
            return _courseSubscriptions.All().FirstOrDefault(x => x.User.Username == username && x.CourseId == courseId);
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

                    subscription.Order = _ordersService.Generate(course, user);
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
                string courseUrl = _routeProvider.GetCourseRoute(subscription.CourseId);
                string coursePictureUrl = _routeProvider.GetCoursePictureRoute(subscription.CourseId);
                if (isPaidCourse)
                {
                    _messageService.SendPaidCourseSignUpMessage(user, course, subscription, courseUrl, coursePictureUrl);
                }
                else
                {
                    _messageService.SendFreeCourseSignUpMessage(user, course, courseUrl, coursePictureUrl);
                }
            }

            return subscription.Status;
        }

        public void AddPayment(Payment payment)
        {
            _payments.Add(payment);
            //TODO UoW
            _payments.SaveChanges();
            CourseSubscription subscription = _courseSubscriptions.GetById(payment.SubscriptionId);
            subscription.ApprovedDate = DateTime.Now;
            subscription.Status = SubscriptionStatus.Active;
            var order = subscription.Order;
            order.Payment = payment;
            order.Status = OrderStatusType.Completed;
            _courseSubscriptions.Update(subscription);
            //TODO implement proper UoW pattern
            _courseSubscriptions.SaveChanges();
            
            string courseUrl = _routeProvider.GetCourseRoute(subscription.CourseId);
            string coursePictureUrl = _routeProvider.GetCoursePictureRoute(subscription.CourseId);
            _messageService.SendPaymentApprovedMessage(subscription.User, subscription.Course, payment, courseUrl, coursePictureUrl);
        }
    }
}
