namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;

    using global::Umbraco.Web.Mvc;

    [Authorize]
    public class SubscriptionsController : SurfaceController
    {
        private readonly ISubscriptionsService _subscriptions;

        public SubscriptionsController(ISubscriptionsService subscriptions)
        {
            _subscriptions = subscriptions;
        }

        //[HttpPost]
        public ActionResult JoinCourse(int courseId)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                _subscriptions.JoinCourse(username, courseId);

            }

            return View();
        }
    }
}