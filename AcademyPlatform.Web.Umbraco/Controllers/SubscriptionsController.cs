namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.Common;
    using AcademyPlatform.Web.Umbraco.DocumentTypeModels;
    using AcademyPlatform.Web.Umbraco.UmbracoModels.DocumentTypes;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    [Authorize]
    public class SubscriptionsController : UmbracoController
    {
        private readonly ISubscriptionsService _subscriptions;

        public SubscriptionsController(ISubscriptionsService subscriptions)
        {
            _subscriptions = subscriptions;
        }

        [HttpGet]
        public ActionResult JoinCourse(string courseUrl)
        {
            if (ModelState.IsValid)
            {
                var coursePublishedContent = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(Course)).FirstOrDefault(x => x.UrlName == courseUrl);
            
                var courseId = coursePublishedContent.GetPropertyValue<int>(nameof(Course.CourseId));

                var username = User.Identity.Name;
                _subscriptions.JoinCourse(username, courseId);

            }
            var urlParts = Request.Url.AbsolutePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            return Redirect("/student/" + urlParts.Last());
        }

        [HttpPost]
        public ActionResult JoinCourse(JoinCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var coursePublishedContent = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(Course)).FirstOrDefault(x => x.UrlName == model.CourseUrl);
            
                var courseId = coursePublishedContent.GetPropertyValue<int>(nameof(Course.CourseId));

                var username = User.Identity.Name;
                try
                {
                    _subscriptions.JoinCourse(username, courseId);
                    var studentPageContent = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(StudentPage)).FirstOrDefault();
                    return Redirect(studentPageContent.Url + model.CourseUrl);
                }
                catch (CourseNotFoundException)
                {
                    ModelState.AddModelError(string.Empty, "Не можахме да намерим търсеният курс!");
                }

            }

            return View();
        }
    }
}