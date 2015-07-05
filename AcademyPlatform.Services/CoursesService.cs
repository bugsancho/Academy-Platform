namespace AcademyPlatform.Services
{
    using System;
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
            return this.courses.All();
        }

        public IQueryable<Course> GetCoursesByUserId(string userId)
        {
            return this.users.GetById(userId).Courses.AsQueryable();
        }



        public Course GetCourseById(int id)
        {
            return this.courses.GetById(id);
        }

        public Course GetCourseByPrettyUrl(string url)
        {
            return this.courses.All().Where(x => x.PrettyUrl == url).FirstOrDefault();
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
        public bool UpdateCourse(Course course)
        {
            //if (!validator.Validate(course))
            //{
            //    return false;
            //}
            try
            {
                this.courses.Update(course);
                this.courses.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteCourse(Course course)
        {
            try
            {
                this.courses.Delete(course);
                this.courses.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}