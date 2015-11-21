namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration
{
    using System.Collections.Generic;

    using AcademyPlatform.Web.Models.Assessments;
    using AcademyPlatform.Web.Models.Common;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using Newtonsoft.Json;

    using Zone.UmbracoMapper;

    public class UmbracoMapperMappings
    {
        public static object MapMediaFile(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            var mediaobj = UmbracoContext.Current.MediaCache.GetById(contentToMapFrom.GetPropertyValue<int>(propName));
            return new ImageViewModel
            {
                Url = mediaobj.Url,
                FileExtension = mediaobj.GetPropertyValue<string>(global::Umbraco.Core.Constants.Conventions.Media.Extension),
                Width = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Width),
                Height = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Height),
                Size = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Bytes),
            };
        }

        public static object MapQuestionAnswer(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            if (contentToMapFrom.HasProperty(propName))
            {
                 var answers = contentToMapFrom.GetPropertyValue<string>(propName);
            return JsonConvert.DeserializeObject<IEnumerable<QuestionAnswer>>(answers);
            }
            return null;

        }
    }
}