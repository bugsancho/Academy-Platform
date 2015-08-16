namespace AcademyPlatform.Web.Infrastructure.Filters
{
    using System;
    using System.Net;
    using System.Web.Mvc;
    using System.Web.Routing;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.Forbidden);
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
