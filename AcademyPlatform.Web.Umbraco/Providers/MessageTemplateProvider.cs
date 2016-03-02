namespace AcademyPlatform.Web.Umbraco.Providers
{
    using System.Linq;

    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;

    using nuPickers;

    using Zone.UmbracoMapper;

    public class MessageTemplateProvider : IMessageTemplateProvider
    {
        private readonly IUmbracoMapper _mapper;
        private IPublishedContent _accountSection;
        private IPublishedContent _coursesSection;

        public MessageTemplateProvider(IUmbracoMapper mapper)
        {
            _mapper = mapper;
        }

        private IPublishedContent AccountSection
        {
            get
            {
                if (_accountSection == null)
                {
                    _accountSection =
                        UmbracoContext.Current.PublishedContentRequest.PublishedContent.AncestorOrSelf(nameof(Site))
                               .DescendantsOrSelf(nameof(Models.Umbraco.DocumentTypes.AccountSection))
                               .SingleOrDefault();
                }

                return _accountSection;
            }
        }

        private IPublishedContent Courses
        {
            get
            {
                if (_coursesSection == null)
                {
                    _coursesSection =
                        UmbracoContext.Current.PublishedContentRequest.PublishedContent.AncestorOrSelf(nameof(Site))
                               .DescendantsOrSelf(nameof(Models.Umbraco.DocumentTypes.Courses))
                               .SingleOrDefault();
                }

                return _coursesSection;
            }
        }

        public MessageTemplate GetAccountValidationTemplate()
        {
            return GetAccountMessageTemplate(nameof(Models.Umbraco.DocumentTypes.AccountSection.AccountValidationEmail));
        }

        public MessageTemplate GetForgotPasswordTemplate()
        {
            return GetAccountMessageTemplate(nameof(Models.Umbraco.DocumentTypes.AccountSection.ForgotPasswordEmail));
        }

        public MessageTemplate GetCourseSignUpTemplate()
        {
            return GetCoursesMessageTemplate(nameof(Models.Umbraco.DocumentTypes.Courses.SignUpForCourseTemplate));
        }

        public MessageTemplate GetPaymentApprovedTemplate()
        {
            return GetCoursesMessageTemplate(nameof(Models.Umbraco.DocumentTypes.Courses.PaymentApprovedTemplate));
        }

        public MessageTemplate GetExamAvailableTemplate()
        {
            return GetCoursesMessageTemplate(nameof(Models.Umbraco.DocumentTypes.Courses.ExamAvailableTemplate));
        }

        public MessageTemplate GetExamSuccessfulTemplate()
        {
            return GetCoursesMessageTemplate(nameof(Models.Umbraco.DocumentTypes.Courses.ExamSuccessfullTemplate));
        }


        private MessageTemplate GetAccountMessageTemplate(string templateName)
        {
            return GetMessageTemplate(AccountSection, templateName);
        }

        private MessageTemplate GetCoursesMessageTemplate(string templateName)
        {
            return GetMessageTemplate(Courses, templateName);
        }

        private MessageTemplate GetMessageTemplate(IPublishedContent sourceContent, string templateName)
        {
            int messageTemplateId = sourceContent.GetPropertyValue<Picker>(templateName).PickedKeys.Select(int.Parse).Single();
            IPublishedContent messageTemplateNode = UmbracoContext.Current.ContentCache.GetById(messageTemplateId);
            MessageTemplate messageTemplate = new MessageTemplate();
            _mapper.Map(messageTemplateNode, messageTemplate);
            return messageTemplate;
        }
    }
}