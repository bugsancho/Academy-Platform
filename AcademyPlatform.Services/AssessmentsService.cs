namespace AcademyPlatform.Services
{
    using System;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Services.Contracts;

    public class AssessmentsService : IAssessmentsService
    {
        private readonly IRepository<AssessmentRequest> _assessmentRequests;
        private readonly IRepository<AssessmentSubmission> _assessmentSubmissions;
        private readonly IUserService _users;

        public AssessmentsService(IRepository<AssessmentRequest> assessmentRequests, IUserService users, IRepository<AssessmentSubmission> assessmentSubmissions)
        {
            _assessmentRequests = assessmentRequests;
            _users = users;
            _assessmentSubmissions = assessmentSubmissions;
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
            //var user = _users.GetByUsername(username);
            //if (user == null)
            //{
            //    throw new UserNotFoundException(username);
            //}

            AssessmentRequest request =
                _assessmentRequests.All()
                                   .Where(x => x.User.Username == username && x.AssessmentExternalId == assessmentId && x.IsCompleted == false)
                                   .OrderByDescending(x => x.CreatedOn)
                                   .FirstOrDefault();
            return request;
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
        }
    }
}
