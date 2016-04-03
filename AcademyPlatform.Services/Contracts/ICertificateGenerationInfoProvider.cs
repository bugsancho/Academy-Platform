namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Certificates;

    public interface ICertificateGenerationInfoProvider
    {
        CertificateGenerationInfo GetByCourseId(int courseId);
    }
}
