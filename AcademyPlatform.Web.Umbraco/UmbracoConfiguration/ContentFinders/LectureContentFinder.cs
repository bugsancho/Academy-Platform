namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.ContentFinders
{
    using System;
    using System.Linq;

    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Core;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    public class LectureContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            if (contentRequest != null)
            {
                var url = contentRequest.Uri.AbsoluteUri;

                var path = contentRequest.Uri.GetAbsolutePathDecoded();
                var urlParts = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                if (urlParts.Length == 4)
                {
                    var rootNodes = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetAtRoot();
                    var lectureNode = rootNodes
                                 .DescendantsOrSelf(nameof(Lecture))
                                 .FirstOrDefault(x => x.UrlName == urlParts.Last());
                    if (lectureNode != null)
                    {
                        contentRequest.PublishedContent = lectureNode;
                    }
                }

                return contentRequest.PublishedContent != null;
            }

            return false;
        }

    }
}