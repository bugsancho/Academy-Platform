namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    public class AssessmentsService : IAssessmentsService
    {
        private readonly IRepository<AssessmentRequest> _assessmentRequests;
        private readonly IRepository<AssessmentSubmission> _assessmentSubmissions;
        private readonly IRepository<CourseSubscription> _subscriptions;
        private readonly IUserService _users;
        private readonly IMessageService _messageService;
        private readonly ILecturesService _lecturesService;
        private readonly ITaskRunner _taskRunner;
        private readonly IApplicationSettings _applicationSettings;

        private DateTime NextAvailableAssessmentAttempt(DateTime date) => date.AddHours(_applicationSettings.AssessmentLockoutTime);

        public AssessmentsService(IRepository<AssessmentRequest> assessmentRequests, IUserService users, IRepository<AssessmentSubmission> assessmentSubmissions, IMessageService messageService, ITaskRunner taskRunner, ILecturesService lecturesService, IApplicationSettings applicationSettings, IRepository<CourseSubscription> subscriptions)
        {
            _assessmentRequests = assessmentRequests;
            _users = users;
            _assessmentSubmissions = assessmentSubmissions;
            _messageService = messageService;
            _taskRunner = taskRunner;
            _lecturesService = lecturesService;
            _applicationSettings = applicationSettings;
            _subscriptions = subscriptions;
        }

        public void CreateAssesmentRequest(AssessmentRequest request)
        {
            //TODO validation
            _assessmentRequests.Add(request);
            _assessmentRequests.SaveChanges();
        }

        public AssessmentRequest GetAssessmentRequest(int requestId)
        {
            AssessmentRequest request = _assessmentRequests.GetById(requestId);
            return request;
        }

        public AssessmentRequest GetLatestForUser(string username, int assessmentId)
        {
            AssessmentRequest request =
                _assessmentRequests.All()
                                   .Where(x => x.User.Username == username && x.AssessmentExternalId == assessmentId && x.IsCompleted == false)
                                   .OrderByDescending(x => x.CreatedOn)
                                   .FirstOrDefault();
            return request;
        }

        public AssessmentEligibilityStatus GetEligibilityStatus(string username, int courseId)
        {
            bool hasActiveSubscription = _subscriptions.All().FirstOrDefault(x => x.User.Username == username && x.CourseId == courseId)?.Status == SubscriptionStatus.Active;
            List<int> unvisitedLectures;
            if (hasActiveSubscription && _lecturesService.HasVisitedAllLectures(username, courseId, out unvisitedLectures))
            {
                AssessmentSubmission latestSubmission = _assessmentSubmissions.All()
                                  .Where(x =>
                                      x.AssessmentRequest.User.Username == username
                                      && x.CourseId == courseId).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

                if (latestSubmission != null)
                {
                    if (latestSubmission.IsSuccessful)
                    {
                        return AssessmentEligibilityStatus.AlreadyCompleted;
                    }
                    if (NextAvailableAssessmentAttempt(latestSubmission.CreatedOn) > DateTime.UtcNow)
                    {
                        return AssessmentEligibilityStatus.Lockout;
                    }
                }

                return AssessmentEligibilityStatus.Eligible;
            }

            return AssessmentEligibilityStatus.NotEligible;
        }

        public bool HasSuccessfulSubmission(string username, int courseId)
        {
            bool hasSuccessfulSubmission = _assessmentSubmissions.All()
                                  .Any(x =>
                                      x.AssessmentRequest.User.Username == username
                                      && x.CourseId == courseId
                                      && x.IsSuccessful);

            return hasSuccessfulSubmission;
        }

        public void CreateAssessmentSubmission(AssessmentSubmission submission)
        {
            if (submission == null)
            {
                throw new ArgumentNullException(nameof(submission));
            }

            //TODO validation
            AssessmentRequest request = submission.AssessmentRequest ?? _assessmentRequests.GetById(submission.AssessmentRequestId);
            request.IsCompleted = true;
            _assessmentRequests.Update(request);
            _assessmentRequests.SaveChanges();

            _assessmentSubmissions.Add(submission);
            _assessmentSubmissions.SaveChanges();

            if (submission.IsSuccessful)
            {
                _messageService.SendExamSuccessfulMessage(request.User, submission.Course);
            }
            else
            {
                _taskRunner.Schedule<IMessageService>(x => x.SendExamAvailableMessage(request.User, submission.Course), NextAvailableAssessmentAttempt(submission.CreatedOn));//TODO add config setting
            }
        }
    }
}
