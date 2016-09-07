namespace AcademyPlatform.Web.Infrastructure.Filters
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AcademyPlatform.Web.Infrastructure.Extensions;

    public class ForceHttpAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            // Note: Child actions are not allowed to perform redirect actions.
            // Note: Ajax requests should not be changed.
			if (!filterContext.IsChildAction &&
                !filterContext.HttpContext.Request.IsAjaxRequest() &&
				!RequestIsTransferred(filterContext.HttpContext.Request) &&
                filterContext.HttpContext.Request.Url.Scheme == "https")
            {
                var requireHttpsAttributeType = typeof (RequireHttpsAttribute);

                var requiresHttpsOnController = filterContext.ActionDescriptor.ControllerDescriptor
                    .GetCustomAttributes(requireHttpsAttributeType, true).Any();
                var requiresHttpsOnAction = filterContext.ActionDescriptor
                    .GetCustomAttributes(requireHttpsAttributeType, true).Any();

                if (!requiresHttpsOnController && !requiresHttpsOnAction)
                {
	                filterContext.Result = new RedirectResult(filterContext.HttpContext.Request.GetHttpOriginalUrl());
                }
            }
        }

	    private static bool RequestIsTransferred(HttpRequestBase request)
	    {
		    string originalUrl = request.RawUrl;
		    string currentUrl = request.Url.ToString().Substring(request.Url.GetLeftPart(UriPartial.Authority).Length);
			
		    return originalUrl != currentUrl;
	    }
    }
}
