namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Models.Courses;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web.Mvc;

    [Authorize]
    public class ProfileController : UmbracoController
    {
        private readonly IUserService _users;
        private readonly ISubscriptionsService _subscriptions;

        public ProfileController(IUserService users, ISubscriptionsService subscriptions)
        {
            _users = users;
            _subscriptions = subscriptions;
        }

        public ActionResult Index()
        {
            var user = _users.GetByUsername(User.Identity.Name);
            var profileViewModel = new ProfileViewModel();
            profileViewModel.Username = user.Username;
            profileViewModel.FirstName = user.FirstName;
            profileViewModel.MiddleName = user.MiddleName;
            profileViewModel.LastName = user.LastName;
            profileViewModel.Company = user.Company;

            List<CourseProgressViewModel> progressViewModels = new List<CourseProgressViewModel>();
            IEnumerable<CourseProgress> courseProgresses = _subscriptions.GetCoursesProgress(User.Identity.Name);
            foreach (CourseProgress courseProgress in courseProgresses)
            {
                CourseProgressViewModel viewModel = new CourseProgressViewModel();
                IPublishedContent course = Umbraco.TypedContentAtXPath($"//Course[courseId={courseProgress.CourseId}]").FirstOrDefault();
                viewModel.CourseUrl = course.Url;
                viewModel.CourseName = course.Name;
                viewModel.CourseProgress = courseProgress;
                viewModel.AssessmentUrl = Url.RouteUrl("Assessment", new { courseNiceUrl = course.UrlName });
                viewModel.AwaitingPaymentUrl = Url.RouteUrl("AwaitingPayment", new { courseNiceUrl = course.UrlName });
                progressViewModels.Add(viewModel);
            }

            profileViewModel.ProgressViewModels = progressViewModels;
            return View(profileViewModel);
        }
    }
}