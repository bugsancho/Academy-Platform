namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration.UrlProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    public class LectureUrlProvider : IUrlProvider
    {
        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == nameof(Lecture) && content.Parent != null && content.Parent.Parent != null)
            {
                var studentsPage = umbracoContext.ContentCache.GetAtRoot().DescendantsOrSelf(nameof(StudentPage)).FirstOrDefault();
                string url = string.Empty;
                if (studentsPage != null)
                {
                    url = studentsPage.Url;
                }

                //TODO Refactor
                url += content.Parent.Parent.UrlName + "/" + content.Parent.UrlName + "/" + content.UrlName;

                return url;
            }

            return null;
        }

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            return Enumerable.Empty<string>();
        }
    }
}