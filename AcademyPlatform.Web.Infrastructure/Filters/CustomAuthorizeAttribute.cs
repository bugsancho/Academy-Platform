namespace AcademyPlatform.Web.Infrastructure.Filters
{
    using System;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
                     new { action = "ComingSoon", Controller = "Home", Area = string.Empty }));
                // base.HandleUnauthorizedRequest(filterContext);
            }
        }
    }
}
