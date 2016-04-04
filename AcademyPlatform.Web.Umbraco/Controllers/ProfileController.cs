namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using AcademyPlatform.Models;
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
        private readonly IUserService _usersService;
        private readonly IMembershipService _membershipService;
        private readonly ISubscriptionsService _subscriptions;
        private readonly ICoursesContentService _coursesContentService;
        private readonly ICertificatesService _certificatesService;

        public ProfileController(IUserService usersService, ISubscriptionsService subscriptions, IMembershipService membershipService, ICoursesContentService coursesContentService, ICertificatesService certificatesService)
        {
            _usersService = usersService;
            _subscriptions = subscriptions;
            _membershipService = membershipService;
            _coursesContentService = coursesContentService;
            _certificatesService = certificatesService;
        }

        public ActionResult Index()
        {
            ProfileViewModel profileViewModel = new ProfileViewModel();

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
            profileViewModel.Certificates = _certificatesService.GetCertificatesForUser(User.Identity.Name);
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
            return PartialView("~/Views/Profile/_PasswordPartial.cshtml", viewModel);
        }



        public ActionResult ProfileInfo()
        {
            User user = _usersService.GetByUsername(User.Identity.Name);
            UpdateProfileViewModel viewModel = Mapper.Map<UpdateProfileViewModel>(user);
            if (HttpContext.Items.Contains("SuccessMessage"))
            {
                viewModel.SuccessMessage = (string)HttpContext.Items["SuccessMessage"];
            }
            return PartialView("~/Views/Profile/_ProfilePartial.cshtml", viewModel);
        }

        public ActionResult UpdateProfile()
        {
            User user = _usersService.GetByUsername(User.Identity.Name);
            UpdateProfileViewModel viewModel = Mapper.Map<UpdateProfileViewModel>(user);
            return UpdateProfilePartial(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostUpdateProfile(UpdateProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = _usersService.GetByUsername(User.Identity.Name);
                Mapper.Map(viewModel, user);
                _usersService.UpdateUser(user);

                HttpContext.Items["SuccessMessage"] = "Успешно обновихте вашия профил!";
                return ProfileInfo();
            }

            return UpdateProfilePartial(viewModel);
        }

        private ActionResult UpdateProfilePartial(UpdateProfileViewModel viewModel)
        {
            return PartialView("~/Views/Profile/_UpdateProfilePartial.cshtml", viewModel);
        }
    }
}