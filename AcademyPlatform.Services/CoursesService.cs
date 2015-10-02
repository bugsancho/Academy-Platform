namespace AcademyPlatform.Services
{
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

        public IQueryable<Course> GetActiveCourses()
        {
            return this.courses.All();
        }

        public IQueryable<Course> GetCoursesByUserId(string userId)
        {
            return this.users.GetById(userId).Courses.AsQueryable();
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