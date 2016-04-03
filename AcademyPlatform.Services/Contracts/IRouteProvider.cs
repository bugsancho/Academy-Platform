namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Certificates;

    public interface IRouteProvider
    {
        string Host { get; }

        string GetRouteByName(string routeName, object routeValues);

        string GetValidateAccountRoute(string email, string validationCode);

        string GetCertificateRoute(Certificate certificate);

        string GetCertificatePictureRoute(Certificate certificate);

        string GetCourseRoute(int courseId);

        string GetCoursePictureRoute(int courseId);

        string GetAssessmentRoute(AcademyPlatform.Models.Courses.Course course);
    }
}
