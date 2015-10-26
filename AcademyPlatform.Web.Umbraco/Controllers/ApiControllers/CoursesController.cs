namespace AcademyPlatform.Web.Umbraco.Controllers.ApiControllers
{
    using System.Linq;
    using System.Web.Http;

    using AcademyPlatform.Services.Contracts;

    using global::Umbraco.Web.WebApi;

    public class CoursesController : UmbracoApiController
    {
        private readonly ICoursesService _courses;

        public CoursesController(ICoursesService courses)
        {
            _courses = courses;
        }

        //Authorize attribute will never pass with current route. Keeping this for future use
        [UmbracoAuthorize]
        public IHttpActionResult GetAll()
        {
            return Json(_courses.GetAll().Select(x => new { Id= x.Id, Title = x.Title }));
        }
    }
}