namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;

    public class CoursesService : ICoursesService
    {
        private readonly IRepository<Course> courses;

        private readonly IRepository<User> users;

        //private readonly IValidator validator;

        public CoursesService(IRepository<Course> courses, IRepository<User> users)
        {
            //this.validator = validator;
            this.courses = courses;
            this.users = users;
        }

        public IQueryable<Course> GetActiveCourses()
        {
            return this.courses.All().Where(c => c.StartDate < DateTime.Now && c.EndDate < DateTime.Now);
        }

        public IQueryable<Course> GetCoursesByUserId(string userId)
        {
            return this.users.GetById(userId).Courses.AsQueryable();
        }

        public bool CreateCourse(Course course)
        {
            //if (!validator.Validate(course))
            //{
            //    return false;
            //}

            try
            {
                this.courses.Add(course);
                this.courses.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public ICollection<ValidationResult> GetErrors(Course course)
        {
            return null;
            //return validator.GetValidationResults(course);
        }
    }
}