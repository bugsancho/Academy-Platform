namespace AcademyPlatform.Web.Umbraco.Startup
{
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration.ContentFinders;

    using global::Umbraco.Core;
    using global::Umbraco.Web.Routing;

    public class ContentFindersConfig : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, StudentCourseContentFinder>();
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, LectureContentFinder>();
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNotFoundHandlers, NotFoundContentFinder>();

            ContentFinderResolver.Current.RemoveType<ContentFinderByNotFoundHandlers>();
            ContentLastChanceFinderResolver.Current.SetFinder(new NotFoundContentFinder());
        }
    }
}