namespace AcademyPlatform.Web.Models.Account
{
    using System.Collections.Generic;

    using AcademyPlatform.Web.Models.Courses;

    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public IEnumerable<CourseProgressViewModel> ProgressViewModels { get; set; }
    }
}
