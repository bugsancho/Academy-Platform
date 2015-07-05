using AcademyPlatform.Data.Repositories;

namespace AcademyPlatform.Services
{
    using System;
    using System.Linq;
    using AcademyPlatform.Models.Courses;

    public interface ICoursesService
    {
        bool DeleteCourse(Course course);

        bool UpdateCourse(Course course);

        Course GetCourseById(int id);

        Course GetCourseByPrettyUrl(string url);

        IQueryable<Course> GetActiveCourses();

        IQueryable<Course> GetCoursesByUserId(string userId);

        bool CreateCourse(Course course);

    }
}