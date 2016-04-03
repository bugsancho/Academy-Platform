namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System.Web;

    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.Services.Contracts;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using nuPickers;

    using Zone.UmbracoMapper;

    public class CertificateGenerationInfoProvider : ICertificateGenerationInfoProvider
    {
        private readonly IUmbracoMapper _mapper;
        private readonly ICoursesContentService _coursesContentService;

        public CertificateGenerationInfoProvider(IUmbracoMapper mapper, ICoursesContentService coursesContentService)
        {
            _mapper = mapper;
            _coursesContentService = coursesContentService;
        }

        public CertificateGenerationInfo GetByCourseId(int courseId)
        {
            IPublishedContent courseContent = _coursesContentService.GetCoursePublishedContentById(courseId);
            int certificateId = (int)courseContent.GetPropertyValue<Picker>(nameof(Course.Certificate)).SavedValue;
            IPublishedContent certificateContent = UmbracoContext.Current.ContentCache.GetById(certificateId);
            CertificateGenerationInfo certificateGenerationInfo = new CertificateGenerationInfo();

            IPublishedContent certificateTemplate = UmbracoContext.Current.MediaCache.GetById(
                certificateContent.GetPropertyValue<int>(
                    nameof(AcademyPlatform.Web.Models.Umbraco.DocumentTypes.Certificate.CertificateTemplate)));

            _mapper.AddCustomMapping(
                    typeof(PlaceholderInfo).FullName,
                    UmbracoMapperMappings.MapPlaceholder)
                .Map(certificateContent, certificateGenerationInfo);

            certificateGenerationInfo.TemplateFilePath = HttpContext.Current.Server.MapPath(certificateTemplate.Url);
            certificateGenerationInfo.BaseFilePath = HttpContext.Current.Server.MapPath("/");

            return certificateGenerationInfo;
        }
    }
}