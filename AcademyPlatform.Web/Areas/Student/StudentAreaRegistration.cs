using System.Web.Mvc;

namespace AcademyPlatform.Web.Areas.Student
{
    public class StudentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Student";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Student_cyr",
            //    "Студент/Курсове/Детайли/{id}",
            //    new { controller = "Courses", action = "Details", id = UrlParameter.Optional }
            //);
            context.MapRoute(
                "Student_default",
                "Student/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}