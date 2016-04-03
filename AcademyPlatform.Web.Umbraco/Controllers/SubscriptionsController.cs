namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Other;
    using AcademyPlatform.Web.Models.Payments;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using AutoMapper;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using Course = AcademyPlatform.Models.Courses.Course;

    [Authorize]
    //TODO use virtual node route handler
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
            SubscriptionStatus subscriptionStatus = _subscriptions.GetSubscriptionStatus(username, course.Id);
            if (subscriptionStatus == SubscriptionStatus.Active)
            {
                IPublishedContent coursesPage = _coursesContent.GetCoursePublishedContentByNiceUrl(courseNiceUrl);
                return Redirect(coursesPage.Url);
            }
            else if (subscriptionStatus == SubscriptionStatus.AwaitingPayment)
            {
                return RedirectToAction(nameof(AwaitingPayment), new { courseNiceUrl = courseNiceUrl });
            }

            JoinCourseViewModel viewModel = new JoinCourseViewModel
            {
                CourseName = course.Title,
                RequiresBillingInfo = _courses.IsPaidCourse(course)
            };

            if (viewModel.RequiresBillingInfo)
            {
                User user = _users.GetByUsername(username);
                viewModel.BillingInfo = Mapper.Map<BillingInfoViewModel>(user.BillingInfo);
            }

            IPublishedContent legalPage = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(LegalContent)).FirstOrDefault();
            int licenseAgreement = legalPage.GetPropertyValue<int>(nameof(LegalContent.LicenseTerms));
            if (licenseAgreement != default(int))
            {
                IPublishedContent licenseAgreementPage = Umbraco.TypedContent(licenseAgreement);
                viewModel.LicenseTermsUrl = licenseAgreementPage.Url;
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

                string username = User.Identity.Name;
                if (model.BillingInfo != null)
                {
                    var user = _users.GetByUsername(username);
                    user.BillingInfo = Mapper.Map(model.BillingInfo, user.BillingInfo);
                    _users.UpdateUser(user);
                }

                SubscriptionStatus status = _subscriptions.JoinCourse(username, course.Id);

                if (status == SubscriptionStatus.Active)
                {
                    IPublishedContent coursesPage = _coursesContent.GetCoursePublishedContentByNiceUrl(model.CourseNiceUrl);
                    return Redirect(coursesPage.Url);
                }
                else
                {
                    return RedirectToAction(nameof(AwaitingPayment), new { courseNiceUrl = model.CourseNiceUrl });
                }

            }

            return JoinCourse(model.CourseNiceUrl);
        }

        [HttpGet]
        public ActionResult AwaitingPayment(string courseNiceUrl)
        {
            Course course = _coursesContent.GetCourseByNiceUrl(courseNiceUrl);
            string username = User.Identity.Name;
            SubscriptionStatus status = _subscriptions.GetSubscriptionStatus(username, course.Id);

            if (status != SubscriptionStatus.AwaitingPayment)
            {
                return Redirect("/");
            }

            AwaitingPaymentViewModel viewModel = new AwaitingPaymentViewModel
            {
                CourseName = course.Title,
                CourseId = course.Id,
                Username = username
            };

            return View(viewModel);
        }
    }
}