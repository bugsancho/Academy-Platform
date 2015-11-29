namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;

    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Infrastructure.Extensions;
    using AcademyPlatform.Web.Infrastructure.Filters;
    using AcademyPlatform.Web.Models.Account;
    using AcademyPlatform.Web.Umbraco.Common;

    using AcademyPlatform.Web.Umbraco.ViewModels;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using nuPickers;

    using Zone.UmbracoMapper;

    using reCAPTCHA.MVC;

    using DocumentTypes = AcademyPlatform.Web.Umbraco.UmbracoModels.DocumentTypes;
    using UmbracoContextExtensions = AcademyPlatform.Web.Umbraco.UmbracoConfiguration.Extensions.UmbracoContextExtensions;

    [RequireHttps]
    [EnsurePublishedContentRequest(2055)]
    public class AccountController : UmbracoController
    {
        private readonly IMembershipService _membership;
        private readonly IUserService _user;
        private readonly IEmailService _email;
        private readonly IUmbracoMapper _mapper;

        private IPublishedContent _accountSection;

        public IPublishedContent AccountSection
        {
            get
            {
                if (_accountSection == null)
                {
                    _accountSection =
                        Umbraco.TypedContentAtRoot()
                               .DescendantsOrSelf(nameof(DocumentTypes.AccountSection))
                               .SingleOrDefault();
                }

                return _accountSection;
            }
        }

        // Umbraco ignores routes that end with something that looks like a file extension.
        // Such as the '.com' domain extensions of emails used in the validate page
        // That's why we need to manually force the creation of UmbracoContext
        public AccountController(IMembershipService membership, IEmailService email, IUmbracoMapper mapper, IUserService user) : base(UmbracoContextExtensions.GetOrCreateContext())
        {
            _membership = membership;
            _email = email;
            _mapper = mapper;
            _user = user;
        }

        [HttpGet]
        [RequireAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [RequireAnonymous]
        [CaptchaValidator]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                if (registerViewModel.Password != registerViewModel.ConfirmPassword) //TODO add this validation on the model level
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Password), "Паролите не съвпадат!");
                    return View(registerViewModel);
                }

                _membership.CreateUser(registerViewModel.Email, registerViewModel.Password, registerViewModel.FirstName, registerViewModel.LastName, out status);

                if (status == MembershipCreateStatus.Success)
                {

                    SendAccountValidationEmail(registerViewModel.Email, registerViewModel.FirstName);
                    var thankYouPageId =
                        AccountSection.GetPropertyValue<int>(
                            nameof(DocumentTypes.AccountSection.RegistrationThankYouPage));
                    return new RedirectToUmbracoPageResult(thankYouPageId);

                }
                if (status == MembershipCreateStatus.DuplicateEmail || status == MembershipCreateStatus.DuplicateUserName)
                {
                    ModelState.AddModelError(nameof(registerViewModel.Email), "Имейлът вече се използва");
                }
            }
            else if (ModelState["ReCaptcha"].Errors.Any())
            {
                ModelState["ReCaptcha"].Errors.Clear();
                ModelState.AddModelError("ReCaptcha", "Моля, попълнете предизвикателството за да сме сигурни че не сте робот");
            }

            return View(registerViewModel);
        }


        [HttpGet]
        [RequireAnonymous]
        public ActionResult Validate(string email, string validationCode = null)
        {
            if (_membership.IsApproved(email))
            {
                return Redirect("/");
            }

            var model = new ValidateAccountViewModel { Email = email, ValidationCode = validationCode };
            if (!string.IsNullOrWhiteSpace(model.Email) && !string.IsNullOrWhiteSpace(model.ValidationCode))
            {
                return Validate(model);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAnonymous]
        public ActionResult Validate(ValidateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_membership.IsApproved(model.Email))
                    {
                        return Redirect("/");
                    }

                    if (_membership.ApproveUser(model.Email, model.ValidationCode))
                    {
                        _membership.Login(model.Email);
                        return Redirect("/");
                    }


                    ModelState.AddModelError(string.Empty, "Невалиден валидационен код");

                }
                catch (UserNotFoundException)
                {
                    ModelState.AddModelError(string.Empty, "Потребител с това име не съществува в системата");
                }
            }

            return View(model);
        }

        [HttpGet]
        [RequireAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAnonymous]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = _user.GetByUsername(model.Email);
            if (user != null)
            {
                // No null checks because there's no possible handing if the properties are not configured and we opt for the 500 page and error log to resolve the issue
                var accountSection = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(AccountSection)).SingleOrDefault();

                var forgotPasswordEmailId =
                    accountSection.GetPropertyValue<Picker>(nameof(DocumentTypes.AccountSection.ForgotPasswordEmail)).SavedValue;

                var forgotPasswordEmailNode = Umbraco.TypedContent(forgotPasswordEmailId);

                var emailTemplate = new DocumentTypes.EmailTemplate();
                _mapper.Map(forgotPasswordEmailNode, emailTemplate);

                emailTemplate.Content = emailTemplate.Content.Replace("{{firstName}}", user.FirstName);
                emailTemplate.Content = emailTemplate.Content.Replace("{{username}}", user.Username);
                emailTemplate.Content = emailTemplate.Content.Replace("{{password}}", _membership.ResetPassword(user.Username));

                _email.SendMail(user.Username, emailTemplate.Content, emailTemplate.Subject);

                return Redirect("/Login");

            }

            ModelState.AddModelError(nameof(ForgotPasswordViewModel.Email), "Не успяхме да намерим потребител с този Email");
            return View(model);
        }

        [HttpGet]
        [RequireAnonymous]
        public ActionResult ResendValidationEmail(string email = null)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                return ResendValidationEmail(new ResendValidationEmailViewModel { Email = email });
            }

            return View(new ResendValidationEmailViewModel { Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequireAnonymous]
        public ActionResult ResendValidationEmail(ResendValidationEmailViewModel model)
        {
            var user = _user.GetByUsername(model.Email);
            if (user != null)
            {
                if (user.IsApproved)
                {
                    return Redirect("/");
                }

                SendAccountValidationEmail(user.Username, user.FirstName);
                return RedirectToAction(nameof(Validate), nameof(AccountController).StripControllerSufix(), new { model.Email });
            }

            ModelState.AddModelError(nameof(ResendValidationEmailViewModel.Email), "Не успяхме да намерим потребител с този Email");
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO move this to model validation
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(ChangePasswordViewModel.NewPassword), "Паролите не съвпадат");
                }

                var changeSuccessful = _membership.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                if (changeSuccessful)
                {
                    return Redirect("/");
                }

                ModelState.AddModelError(nameof(ChangePasswordViewModel.OldPassword), "Грешна парола");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult LogOut()
        {
            _membership.LogOut();
            return Redirect("/");
        }

        private bool SendAccountValidationEmail(string email, string firstName)
        {
            var validationPath = Url.RouteUrl("Validate", new RouteValueDictionary
                                                                              {
                                                                                  { "Email", email },
                                                                                  { "ValidationCode",   _membership.GenerateValidationCode(email) }
                                                                              });

            var validationLink = new UriBuilder("https", Request.Url.Host, 80, validationPath);

            var accountValidationMailId =
                AccountSection.GetPropertyValue<Picker>(nameof(DocumentTypes.AccountSection.AccountValidationEmail)).SavedValue;
            var accountValidationMailNode = Umbraco.Content(accountValidationMailId);
            if (accountValidationMailNode != null)
            {
                var emailTemplate = new DocumentTypes.EmailTemplate();
                _mapper.Map(accountValidationMailNode, emailTemplate);

                emailTemplate.Content = emailTemplate.Content.Replace("{{firstName}}", firstName);
                // Umbraco's TinyMCE inserts forward slash ('/') at the begining of href's, so we have to manually remove it as part of the link insertion
                emailTemplate.Content = emailTemplate.Content.Replace("/{{validationLink}}", validationLink.Uri.AbsoluteUri);

                return _email.SendMail(email, emailTemplate.Content, emailTemplate.Subject);
            }


            return false;
        }
    }
}