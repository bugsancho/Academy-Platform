namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AcademyPlatform.Common.Validators;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;

    public interface ICoursesService
    {
        IQueryable<Course> GetActiveCourses();

        IQueryable<Course> GetCoursesByUserId(string userId);

        bool CreateCourse(Course course);

        ICollection<ValidationResult> GetErrors(Course course);
    }
}