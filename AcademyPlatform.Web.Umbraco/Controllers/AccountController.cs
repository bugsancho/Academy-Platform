namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.Security;

    using AcademyPlatform.Web.Infrastructure.Extensions;
    using AcademyPlatform.Web.Infrastructure.Filters;
    using AcademyPlatform.Web.Umbraco.UmbracoModels.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.ViewModels;

    using ClientDependency.Core;

    using global::Umbraco.Core;
    using global::Umbraco.Web;

    using Services.Contracts;
    using Models.Account;

    using global::Umbraco.Web.Mvc;

    using nuPickers;

    using Zone.UmbracoMapper;

    //[RequireHttps]
    [EnsurePublishedContentRequest(2055)]
    public class AccountController : SurfaceController
    {
        private readonly IMembersService _members;
        private readonly IEmailService _email;
        private readonly IUmbracoMapper _mapper;

        public AccountController(IMembersService members, IEmailService email, IUmbracoMapper mapper)
        {
            _members = members;
            _email = email;
            _mapper = mapper;
        }

        [HttpGet]
        [RequireAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [RequireAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                if (registerViewModel.Password != registerViewModel.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Password), "Паролите не съвпадат!");
                    return View(registerViewModel);
                }

                _members.CreateUser(registerViewModel.Email, registerViewModel.Password, registerViewModel.FirstName, registerViewModel.LastName, true, out status);

                if (status == MembershipCreateStatus.Success)
                {
                    var validationLink = Url.RouteUrl("Validate", new RouteValueDictionary
                                                                              {
                                                                                  { "Email", registerViewModel.Email },
                                                                                  { "ValidationCode", _members.GenerateValidationCode(registerViewModel.Email) }
                                                                              });
                    validationLink = validationLink.ReplaceFirst("?", "/?");

                    var accountSection = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(AccountSection)).SingleOrDefault();
                    if (accountSection != null)
                    {
                        var accountValidationMailId =
                            accountSection.GetPropertyValue<Picker>(nameof(AccountSection.AccountValidationEmail)).SavedValue;
                        var accountValidationMailNode = Umbraco.Content(accountValidationMailId);
                        if (accountValidationMailNode != null)
                        {
                            var emailTemplate = new EmailTemplate();
                            _mapper.Map(accountValidationMailNode, emailTemplate);



                            emailTemplate.Content = emailTemplate.Content.Replace("{{firstName}}", registerViewModel.FirstName);
                            emailTemplate.Content = emailTemplate.Content.Replace("{{validationLink}}", validationLink);

                            return Redirect(validationLink);
                            //_email.SendMail(registerViewModel.Email, emailTemplate.Content, emailTemplate.Subject);
                        }
                    }

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(Url.ToSafeReturnUrl(returnUrl));
                    }
                }
            }

            return View(registerViewModel);
        }


        [HttpGet]
        [RequireAnonymous]
        public ActionResult Validate(string email, string validationCode)
        {
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
                var member = _members.GetUser(model.Email);
                if (member == null)
                {
                    ModelState.AddModelError(string.Empty, "Потребител с това име не съществува в системата");
                }
                else if (member.IsApproved)
                {
                    return Redirect("/");
                }
                else
                {
                    try
                    {
                        _members.ApproveUser(model.Email, model.ValidationCode);
                        return Redirect("/");
                    }
                    catch (ArgumentException e)
                    {
                        ModelState.AddModelError(string.Empty, e.Message);
                    }
                }
            }

            return View(model);

        }
    }


}