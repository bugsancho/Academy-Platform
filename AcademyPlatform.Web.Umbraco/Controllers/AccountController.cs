namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;

    using AcademyPlatform.Web.Infrastructure.Extensions;

    using Services.Contracts;
    using Models.Account;
    
    using global::Umbraco.Web.Mvc;

    public class AccountController : SurfaceController
    {
        private readonly IMembersService _members;

        public AccountController(IMembersService members)
        {
            _members = members;
        }
        
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus status;
                _members.CreateUser(registerViewModel.Email, registerViewModel.Password, false,out status);

                if (status == MembershipCreateStatus.Success)
                {
                    //TODO validate return url!!!!!
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                         return Redirect(returnUrl);
                    }

                    
                   
                }

            }

            return CurrentUmbracoPage();
        }
    }
}