namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Extensions;
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
        public ActionResult Login(RenderModel model)
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_members.Login(loginViewModel.Email, loginViewModel.Password, loginViewModel.RememberMe))
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(Url.ToSafeReturnUrl(returnUrl));
                    }

                    return Redirect("/contact-us");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid username and pasword combination");
                }

            }

            return View(loginViewModel);
        }
    }
}