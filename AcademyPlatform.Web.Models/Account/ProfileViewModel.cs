namespace AcademyPlatform.Web.Models.Account
{
    using System.Collections.Generic;

    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Web.Infrastructure.Mappings;
    using AcademyPlatform.Web.Models.Courses;

    public class ProfileViewModel : IMapFrom<User>
    {
        public IEnumerable<CourseProgressViewModel> ProgressViewModels { get; set; }

        public IEnumerable<Certificate> Certificates { get; set; }

        public IEnumerable<Order> Orders { get; set; }

    }
}
