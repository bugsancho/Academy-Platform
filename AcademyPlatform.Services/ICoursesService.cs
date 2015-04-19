using AcademyPlatform.Data.Repositories;

namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AcademyPlatform.Models.Courses;

    public interface ICoursesService
    {
        bool UpdateCourse(Course course);

        Course GetCourseById(int id);

        IQueryable<Course> GetActiveCourses();

        IQueryable<Course> GetCoursesByUserId(string userId);

        bool CreateCourse(Course course);

        ICollection<ValidationResult> GetErrors(Course course);
    }
}