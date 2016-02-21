namespace AcademyPlatform.Web.Umbraco.Services
{
    using System.Linq;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    
    using nuPickers;

    using Course = AcademyPlatform.Models.Courses.Course;

    public class CoursesContentService : ICoursesContentService
    {
        private readonly ICoursesService _courses;

        public CoursesContentService(ICoursesService courses)
        {
            _courses = courses;
        }

        public IPublishedContent GetCoursePublishedContentByNiceUrl(string niceUrl)
        {
            IPublishedContent coursePublishedContent = UmbracoContext.Current.ContentCache.GetAtRoot().DescendantsOrSelf(nameof(Models.Umbraco.DocumentTypes.Course)).SingleOrDefault(x => x.UrlName == niceUrl);
            return coursePublishedContent;
        }

        public Course GetCourseByNiceUrl(string niceUrl)
        {
            IPublishedContent coursePublishedContent = GetCoursePublishedContentByNiceUrl(niceUrl);
            if (coursePublishedContent == null)
            {
                return null;
            }

            int courseId = GetCourseId(coursePublishedContent);
            Course course = _courses.GetById(courseId);
            return course;
        }

        public int GetCourseId(IPublishedContent content)
        {
            return int.Parse(content.GetPropertyValue<Picker>(nameof(Models.Umbraco.DocumentTypes.Course.CourseId)).PickedKeys.First());
        }
    }
}