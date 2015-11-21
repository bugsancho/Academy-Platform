namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Extensions;
    using AcademyPlatform.Web.Infrastructure.Filters;
    using AcademyPlatform.Web.Models.Account;

    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    public class LoginController : RenderMvcController
    {
        private readonly IMembersService _members;

        public LoginController(IMembersService members)
        {
            _members = members;
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
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //TODO add support for persistent login. Currently it's always 3 days
                if (_members.Login(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(Url.ToSafeReturnUrl(returnUrl));
                    }

                    return Redirect("/");
                }

                ModelState.AddModelError(string.Empty, "Не успяхме да намерим потребител с това име и парола.");
            }

            return View(loginViewModel);
        }
    }
}