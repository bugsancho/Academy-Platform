using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademyPlatform.Web.Umbraco.Startup
{
    using AcademyPlatform.Web.Umbraco.Common;

    using global::Umbraco.Core;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    public class UmbracoEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
		{
			//The Umbraco implementation of Public Access restriction does an internal redirect to the login page PublishedContent.
			//We want that to be a redirect with returnUrl.
			PublishedContentRequest.Prepared += delegate(object sender, EventArgs args)
			{
				var pcr = (PublishedContentRequest) sender;
				if (pcr.HasPublishedContent && !pcr.IsInitialPublishedContent && !pcr.Is404 &&
				    !pcr.IsInternalRedirectPublishedContent
				    && pcr.PublishedContent.DocumentTypeAlias == "Login")
				{
					string redirectUrl = string.Format("{0}?{1}={2}", 
						pcr.PublishedContent.UrlWithDomain(),
						"returnUrl", 
						HttpUtility.UrlEncode(pcr.RoutingContext.UmbracoContext.HttpContext.Request.RawUrl));

					pcr.SetRedirect(redirectUrl);
				}
			};
		}
    }
}