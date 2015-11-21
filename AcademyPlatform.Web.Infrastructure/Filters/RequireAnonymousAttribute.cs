namespace AcademyPlatform.Web.Infrastructure.Filters
{
    using System.Web.Mvc;

    public class RequireAnonymousAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("~/");
            }
        }
    }
}
