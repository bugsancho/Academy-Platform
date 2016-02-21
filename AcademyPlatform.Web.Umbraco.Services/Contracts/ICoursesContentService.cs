namespace AcademyPlatform.Web.Umbraco.Services.Contracts
{
    using AcademyPlatform.Models.Courses;

    using global::Umbraco.Core.Models;

    public interface ICoursesContentService
    {
        IPublishedContent GetCoursePublishedContentByNiceUrl(string niceUrl);

        Course GetCourseByNiceUrl(string niceUrl);

        int GetCourseId(IPublishedContent content);
    }
}