namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Assessments;

    public interface IAssessmentsService
    {
        void CreateAssesmentRequest(AssessmentRequest request);

        void CreateAssessmentSubmission(AssessmentSubmission submission);

        AssessmentRequest GetAssessmentRequest(int requestId);

        AssessmentRequest GetLatestForUser(string username, int assessmentId);

        bool HasSuccessfulSubmission(string username, int courseId);
    }
}