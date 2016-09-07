namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.ContentFinders
{
    using System;
    using System.Linq;

    using AcademyPlatform.Services;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Core;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    public class StudentCourseContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest != null)
            {
                var url = contentRequest.Uri.AbsoluteUri;

                var path = contentRequest.Uri.GetAbsolutePathDecoded();
                var urlParts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (urlParts.Length == 2)
                {
                    var rootNodes = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetAtRoot();
                    var courseNode = rootNodes
                                 .DescendantsOrSelf(nameof(Course))
                                 .FirstOrDefault(x => x.UrlName == urlParts.Last());
                    if (courseNode != null)
                    {
                        if (contentRequest.RoutingContext.UmbracoContext.HttpContext.User.Identity.IsAuthenticated)
                        {
                            var username = contentRequest.RoutingContext.UmbracoContext.HttpContext.User.Identity.Name;
                        }
                        contentRequest.PublishedContent = courseNode;
                        var templateSet = contentRequest.TrySetTemplate("StudentCoursePage");
                        
                    }
                }

                return contentRequest.PublishedContent != null;
            }

            return false;
        }

    }
}