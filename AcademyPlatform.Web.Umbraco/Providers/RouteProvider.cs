namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;

    public class RouteProvider : IRouteProvider
    {
        public string GetRouteByName(string routeName, object routeValues)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        public string GetValidateAccountRoute(string email, string validationCode)
        {
            string validationPath = GetRouteByName("Validate", new { email, validationCode });
            string validationLink = new UriBuilder("https", Host, 443, validationPath).Uri.AbsoluteUri;
            return validationLink;
        }

        public string GetForgotPasswordRoute(string email, string validationCode)
        {
            string validationPath = GetRouteByName("Validate", new { email, validationCode });
            string validationLink = new UriBuilder("https", Host, 443, validationPath).Uri.AbsoluteUri;
            return validationLink;
        }

        public string Host => HttpContext.Current.Request.Url.Host;
    }
}