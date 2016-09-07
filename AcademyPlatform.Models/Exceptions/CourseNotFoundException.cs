namespace AcademyPlatform.Models.Exceptions
{
    using System;

    public class CourseNotFoundException : ApplicationException
    {
        public CourseNotFoundException(int courseId) : base($"Could not find course with id - {courseId}")
        {

        }
    }
}
