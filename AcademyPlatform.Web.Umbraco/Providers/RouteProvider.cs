namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using Certificate = AcademyPlatform.Models.Certificates.Certificate;

    public class RouteProvider : IRouteProvider
    {
        private readonly ICoursesContentService _coursesContentService;

        public RouteProvider(ICoursesContentService coursesContentService)
        {
            _coursesContentService = coursesContentService;
        }

        public string Host => HttpContext.Current.Request.Url.Host;

        public string GetRouteByName(string routeName, object routeValues)
        {
            UrlHelper urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        public string GetValidateAccountRoute(string email, string validationCode)
        {
            string validationPath = GetRouteByName("Validate", new { email, validationCode });
            string absoluteUrl = GetHttpsAbsoluteUrl(validationPath);
            return absoluteUrl;
        }

        public string GetAssessmentRoute(int courseId)
        {
            IPublishedContent courseContent = _coursesContentService.GetCoursePublishedContentById(courseId);
            string validationPath = GetRouteByName("Assessment", new { courseNiceUrl = courseContent.UrlName });
            string absoluteUrl = GetHttpsAbsoluteUrl(validationPath);
            return absoluteUrl;
        }

        public string GetCertificateRoute(Certificate certificate)
        {
            string certificateRoute = GetRouteByName("Certificate", new { certificateCode = certificate.Code });
            string absoluteUrl = GetHttpsAbsoluteUrl(certificateRoute);
            return absoluteUrl;
        }

        public string GetCertificatePictureRoute(Certificate certificate)
        {
            //TODO figure out a smarter way to retrieve image url
            string certificatePictureRoute = GetCertificateRoute(certificate) + ".jpeg";
            return certificatePictureRoute;
        }

        public string GetCourseRoute(int courseId)
        {
            IPublishedContent courseContent = _coursesContentService.GetCoursePublishedContentById(courseId);
            return GetHttpsAbsoluteUrl(courseContent.Url);
        }

        public string GetCoursePictureRoute(int courseId)
        {
            IPublishedContent courseContent = _coursesContentService.GetCoursePublishedContentById(courseId);
            int pictureId = courseContent.GetPropertyValue<int>(nameof(Course.CoursePicture));
            IPublishedContent picture = UmbracoContext.Current.MediaCache.GetById(pictureId);
            return GetHttpsAbsoluteUrl(picture.Url);
        }

        private string GetHttpsAbsoluteUrl(string route)
        {
            string httpsLink = new UriBuilder("https", Host, 443, route).Uri.AbsoluteUri;
            return httpsLink;
        }
    }
}