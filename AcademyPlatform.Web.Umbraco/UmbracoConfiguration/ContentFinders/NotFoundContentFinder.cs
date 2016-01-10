namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.ContentFinders
{
    using System.Linq;

    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;
     
    public class NotFoundContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            var site = contentRequest.RoutingContext.UmbracoContext.ContentCache.GetAtRoot();

            var notFoundNode = site
                .DescendantsOrSelf(nameof(ErrorPage))
                .FirstOrDefault(x =>
                    x.GetPropertyValue<int>(nameof(ErrorPage.ErrorCode)) == 404);

            contentRequest.PublishedContent = notFoundNode;
            contentRequest.SetIs404();

            return contentRequest.PublishedContent != null;
        }
    }
}