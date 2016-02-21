namespace AcademyPlatform.Web.Models.Courses
{
    using AcademyPlatform.Web.Models.Common;

    public class LectureLinkViewModel : PageLink
    {
        public bool IsVisited { get; set; }

        public bool IsDemo { get; set; }

        public bool IsCurrent { get; set; }
    }
}
