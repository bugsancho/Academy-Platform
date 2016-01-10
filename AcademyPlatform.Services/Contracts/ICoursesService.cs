namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Models.Courses;

    public interface ICoursesService
    {
        void Delete(Course course);

        void Update(Course course);

        Course GetById(int id);

        void Create(Course course);

        IEnumerable<Course> GetActiveCourses();

        IEnumerable<Course> GetCoursesByUserId(string userId);

        IEnumerable<Course> GetAll();

        bool IsPaidCourse(Course course);
    }
}