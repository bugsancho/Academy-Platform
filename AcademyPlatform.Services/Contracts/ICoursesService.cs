namespace AcademyPlatform.Services.Contracts
{
    using System.Linq;

    using AcademyPlatform.Models.Courses;

    public interface ICoursesService
    {
        void Delete(Course course);

        void Update(Course course);

        Course GetById(int id);

        IQueryable<Course> GetActiveCourses();

        IQueryable<Course> GetCoursesByUserId(string userId);

        void Create(Course course);

    }
}