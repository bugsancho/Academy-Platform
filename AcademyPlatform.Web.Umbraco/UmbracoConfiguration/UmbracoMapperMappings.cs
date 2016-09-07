namespace AcademyPlatform.Web.Umbraco.UmbracoConfiguration
{
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Surveys;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using nuPickers;

    using Newtonsoft.Json;

    using Zone.UmbracoMapper;

    public class UmbracoMapperMappings
    {
        public static object MapMediaFile(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            IPublishedContent mediaobj = UmbracoContext.Current.MediaCache.GetById(contentToMapFrom.GetPropertyValue<int>(propName));
            return new ImageViewModel
            {
                Url = mediaobj.Url,
                FileExtension = mediaobj.GetPropertyValue<string>(global::Umbraco.Core.Constants.Conventions.Media.Extension),
                Width = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Width),
                Height = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Height),
                Size = mediaobj.GetPropertyValue<int>(global::Umbraco.Core.Constants.Conventions.Media.Bytes),
            };
        }

        public static object MapPageLink(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            IPublishedContent content = UmbracoContext.Current.ContentCache.GetById(contentToMapFrom.GetPropertyValue<int>(propName));
            return new PageLink
            {
                Url = content.Url,
                Name = content.Name
            };
        }

        public static object MapQuestionAnswer(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            if (contentToMapFrom.HasProperty(propName))
            {
                string answers = contentToMapFrom.GetPropertyValue<string>(propName);
                return JsonConvert.DeserializeObject<IEnumerable<QuestionAnswer>>(answers);
            }

            return null;
        }

        public static object MapPlaceholder(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            if (contentToMapFrom.HasProperty(propName))
            {
                string placeholderInfo = contentToMapFrom.GetPropertyValue<string>(propName);
                return JsonConvert.DeserializeObject<PlaceholderInfo>(placeholderInfo);
            }

            return null;
        }

        public static object MapPicker(IUmbracoMapper mapper, IPublishedContent contentToMapFrom, string propName, bool isRecursive)
        {
            if (contentToMapFrom.HasProperty(propName))
            {
                Picker picker = contentToMapFrom.GetPropertyValue<Picker>(propName);
                return int.Parse(picker.PickedKeys.First());
            }

            return null;

        }
    }
}