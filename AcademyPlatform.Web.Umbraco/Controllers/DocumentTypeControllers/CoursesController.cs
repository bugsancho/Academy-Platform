namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;

    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    public class CoursesController : RenderMvcController
    {
        private readonly ICoursesService _courses;

        public CoursesController(ICoursesService courses)
        {
            _courses = courses;
        }

        public override ActionResult Index(RenderModel model)
        {
            return base.Index(model);
        }
    }
}