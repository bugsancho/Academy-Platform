namespace AcademyPlatform.Web.Umbraco.Common
{
    public static class DocumentTypes
    {

        public static class Seo
        {
            public const string Alias = "Seo";

            public const string Title = "title";

            public const string Description = "description";

            public const string Keywords = "keywords";
        }

        public static class Content
        {
            public const string Alias = "ContentPage";

            public const string ContentSection = "content";

            public const string SubHeader = "subHeader";
        }

        public static class Courses
        {
            public const string Alias = "Courses";
        }

        public static class Course
        {
            public const string Alias = "Course";

            public const string CourseId = "courseId";

            public const string CourseTitle = "courseTitle";

            public const string CoursePicture = "coursePicture";

            public const string DetailedDescription = "detailedDescription";

            public const string ShortDescription = "shortDescription";
        }
    }
}