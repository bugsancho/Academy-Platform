namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Assessments;
    using AcademyPlatform.Models.Certificates;

    public interface ICertificatesService
    {
        Certificate GenerateCertificate(string username, int courseId, AssessmentSubmission assessmentSubmission, CertificateGenerationInfo certificateGenerationInfo);

        Certificate GetByUniqueCode(string uniqueCode);
    }
}