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

    [RequireHttps]
    public class AccountController : RenderMvcController
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
                                                                                  { "ValidationCode", registerViewModel.Email.Encrypt() }
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
        public ActionResult Validate(ValidateAccountViewModel model)
        {
            if (!ModelState.IsValid)
            {

                ModelState.AddModelError(string.Empty, "Моля, попълнете валидационният код!");
                return View(model);
            }
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
                    var decrypted = model.ValidationCode.Decrypt();
                    if (decrypted == model.Email)
                    {
                        _members.ApproveUser(model.Email);
                        return Redirect("/");
                    }
                }
                catch (CryptographicException)
                {
                    ModelState.AddModelError(nameof(model.ValidationCode), "Валидационният код не е валиден");

                }
            }



            return View(model);

        }
    }


}