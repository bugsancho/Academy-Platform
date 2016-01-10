namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using Course = AcademyPlatform.Models.Courses.Course;

    [Authorize]
    [EnsurePublishedContentRequest(2055)]
    public class SubscriptionsController : UmbracoController
    {
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICoursesService _courses;
        private readonly ICoursesContentService _coursesContent;
        private readonly IUserService _users;

        public SubscriptionsController(ISubscriptionsService subscriptions, ICoursesService courses, IUserService users, ICoursesContentService coursesContent)
        {
            _subscriptions = subscriptions;
            _courses = courses;
            _users = users;
            _coursesContent = coursesContent;
        }

        [HttpGet]
        public ActionResult JoinCourse(string courseNiceUrl)
        {
            Course course = _coursesContent.GetCourseByNiceUrl(courseNiceUrl);
            if (course == null)
            {
                return HttpNotFound();
            }
            string username = User.Identity.Name;
            if (_subscriptions.HasActiveSubscription(username, course.Id))
            {
                IPublishedContent studentPageContent = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(StudentPage)).FirstOrDefault();
                return Redirect(studentPageContent.Url + courseNiceUrl);
            }

            JoinCourseViewModel viewModel = new JoinCourseViewModel();
            viewModel.RequiresBillingInfo = _courses.IsPaidCourse(course);
            if (viewModel.RequiresBillingInfo)
            {
                User user = _users.GetByUsername(username);
                viewModel.BillingInfo = new BillingInfoViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };
            }

            IPublishedContent legalPage = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(LegalContent)).FirstOrDefault();
            int licenseAgreement = legalPage.GetPropertyValue<int>(nameof(LegalContent.LicenseTerms));
            if (licenseAgreement != default(int))
            {
                IPublishedContent licenseAgreementPage = Umbraco.TypedContent(licenseAgreement);
                viewModel.LicenseTerms = licenseAgreementPage.GetPropertyValue<string>(nameof(ContentPage.Content));
            }



            return View(viewModel);
        }

        [HttpPost]
        public ActionResult JoinCourse(JoinCourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                Course course = _coursesContent.GetCourseByNiceUrl(model.CourseNiceUrl);
                if (course == null)
                {
                    return HttpNotFound();
                }

                bool requiresBillingInfo = _courses.IsPaidCourse(course);

                string username = User.Identity.Name;
                _subscriptions.JoinCourse(username, course.Id);

                IPublishedContent studentPageContent = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(StudentPage)).FirstOrDefault();
                return Redirect(studentPageContent.Url + model.CourseNiceUrl);
            }

            return JoinCourse(model.CourseNiceUrl);
        }
    }
}