namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Extensions;
    using AcademyPlatform.Web.Infrastructure.Filters;
    using AcademyPlatform.Web.Models.Account;

    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    public class LoginController : RenderMvcController
    {
        private readonly IMembershipService _membership;

        public LoginController(IMembershipService membership)
        {
            _membership = membership;
        }

        [HttpGet]
        [RequireHttps]
        [RequireAnonymous]
        public ActionResult Login(RenderModel model)
        {

            return View();
        }

        [HttpPost]
        [RequireHttps]
        [RequireAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                //TODO add support for persistent login. Currently it's always 3 days
                try
                {
                    if (_membership.Login(loginViewModel.Email, loginViewModel.Password, true))
                    {
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(Url.ToSafeReturnUrl(returnUrl));
                        }

                        return Redirect("/");
                    }

                    ModelState.AddModelError(string.Empty, "Не успяхме да намерим потребител с това име и парола.");
                }
                catch (UserNotApprovedException)
                {
                    ModelState.AddModelError(string.Empty, "Потребителят не е порвърдил своя Email. Моля проверете пощата си за съобщение от Focus Academy и следвайте инструкциите");
                }
            }

            return View(loginViewModel);
        }
    }
}