namespace AcademyPlatform.Services.Contracts
{
    using System;

    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Certificates;

    public interface IAssessmentsService
    {
        void CreateAssesmentRequest(AssessmentRequest request);

        void CreateAssessmentSubmission(AssessmentSubmission submission, out Certificate certificate);

        AssessmentRequest GetAssessmentRequest(int requestId);

        AssessmentRequest GetLatestForUser(string username, int assessmentId);

        bool HasSuccessfulSubmission(string username, int courseId);

        AssessmentEligibilityStatus GetEligibilityStatus(string username, int courseId);

        DateTime? GetNextAssessmentAttemptDate(string username, int courseId);
    }
}