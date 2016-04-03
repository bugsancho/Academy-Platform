namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using AutoMapper;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web.Mvc;

    [Authorize]
    public class ProfileController : UmbracoController
    {
        private readonly IUserService _users;
        private readonly IMembershipService _membershipService;
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICoursesContentService _coursesContentService;

        public ProfileController(IUserService users, ISubscriptionsService subscriptions, IMembershipService membershipService, ICoursesContentService coursesContentService)
        {
            _users = users;
            _subscriptions = subscriptions;
            _membershipService = membershipService;
            _coursesContentService = coursesContentService;
        }

        public ActionResult Index()
        {
            var user = _users.GetByUsername(User.Identity.Name);
            var profileViewModel = Mapper.Map<ProfileViewModel>(user);

            List<CourseProgressViewModel> progressViewModels = new List<CourseProgressViewModel>();
            IEnumerable<CourseProgress> courseProgresses = _subscriptions.GetCoursesProgress(User.Identity.Name);
            foreach (CourseProgress courseProgress in courseProgresses)
            {
                CourseProgressViewModel viewModel = new CourseProgressViewModel();
                IPublishedContent course = _coursesContentService.GetCoursePublishedContentById(courseProgress.CourseId);
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(courseProgress.CourseId), $"Could not find published content with the specified course Id: {courseProgress.CourseId}");
                }

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

        public ActionResult GetChangePassword(ChangePasswordViewModel viewModel)
        {
            return ChangePasswordPartial(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostChangePassword(ChangePasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_membershipService.ChangePassword(User.Identity.Name, viewModel.OldPassword, viewModel.NewPassword))
                {
                    viewModel = new ChangePasswordViewModel
                    {
                        SuccessMessage = "Успешно променихте Вашата парола"
                    };
                }
                else
                {
                    ModelState.AddModelError(nameof(viewModel.OldPassword), "Невалидна парола");
                }
            }

            return ChangePasswordPartial(viewModel);
        }

        private ActionResult ChangePasswordPartial(ChangePasswordViewModel viewModel)
        {
            return PartialView("~/Views/Profile/_ProfilePartial.cshtml", viewModel);
        }
    }
}