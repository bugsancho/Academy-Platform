namespace AcademyPlatform.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;

    public class CoursesService : ICoursesService
    {
        private readonly IRepository<Course> courses;

        private readonly IRepository<User> users;

        public CoursesService(IRepository<Course> courses, IRepository<User> users)
        {
            this.courses = courses;
            this.users = users;
        }

        public IEnumerable<Course> GetAll()
        {
            return this.courses.All().OrderBy(x => x.Id);
        }

        public IEnumerable<Course> GetActiveCourses()
        {
            return this.courses.AllIncluding(x => x.Category);
        }

        public IEnumerable<Course> GetCoursesByUserId(string userId)
        {
            return this.users.GetById(userId).Courses;
        }

        public Course GetById(int id)
        {
            return this.courses.GetById(id);
        }


        public void Create(Course course)
        {
            this.courses.Add(course);
            this.courses.SaveChanges();
        }
        public void Update(Course course)
        {
            this.courses.Update(course);
            this.courses.SaveChanges();

        }

        public void Delete(Course course)
        {
            this.courses.Delete(course);
            this.courses.SaveChanges();

        }
    }
}