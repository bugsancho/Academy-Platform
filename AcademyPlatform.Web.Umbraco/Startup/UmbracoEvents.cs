namespace AcademyPlatform.Web.Umbraco.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Models.Courses;
    using DocumentTypes = AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Core;
    using global::Umbraco.Core.Models;
    using global::Umbraco.Core.Services;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Routing;

    public class UmbracoEvents : ApplicationEventHandler
    {
        private readonly ILecturesService _lectures;

        public UmbracoEvents()
        {
            _lectures = DependencyResolver.Current.GetService(typeof(ILecturesService)) as ILecturesService;
        }

        protected override void ApplicationStarted(
            UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            //The Umbraco implementation of Public Access restriction does an internal redirect to the login page PublishedContent.
            //We want that to be a redirect with returnUrl.
            PublishedContentRequest.Prepared += delegate (object sender, EventArgs args)
                {
                    var pcr = (PublishedContentRequest)sender;
                    if (pcr.HasPublishedContent && !pcr.IsInitialPublishedContent && !pcr.Is404
                        && !pcr.IsInternalRedirectPublishedContent && pcr.PublishedContent.DocumentTypeAlias == "Login")
                    {
                        string redirectUrl = string.Format(
                            "{0}?{1}={2}",
                            pcr.PublishedContent.UrlWithDomain(),
                            "returnUrl",
                            HttpUtility.UrlEncode(pcr.RoutingContext.UmbracoContext.HttpContext.Request.RawUrl));

                        pcr.SetRedirect(redirectUrl);
                    }
                };

            ContentService.Saving += (sender, args) =>
                {
                    AddOrUpdateLecture(args.SavedEntities, x => x.Published);
                };

            //Adding a Save post-event because during the Saving event, new items still have no Id so syncing won't work.
            ContentService.Saved += (sender, args) =>
                {
                    AddOrUpdateLecture(args.SavedEntities, x => x.Published);
                };

            ContentService.UnPublishing += (sender, args) =>
                {
                    AddOrUpdateLecture(args.PublishedEntities, x => false);
                };

            ContentService.RollingBack += (sender, args) =>
                {
                    AddOrUpdateLecture(Enumerable.Repeat(args.Entity, 1), x => x.Published);
                };

            ContentService.Moving += (sender, args) =>
                {
                    AddOrUpdateLecture(args.MoveInfoCollection.Select(x => x.Entity), x => x.Published);
                };

            ContentService.Trashing += (sender, args) =>
                {
                    AddOrUpdateLecture(args.MoveInfoCollection.Select(x => x.Entity), x => false);
                };

            ContentService.Deleting += (sender, args) =>
                {
                    AddOrUpdateLecture(args.DeletedEntities, x => false);
                };
        }

        private Lecture GetLectureFromContent(IContent lectureContent)
        {
            //TODO handle trashed lectures.
            object courseId = lectureContent?.Ancestors()?.FirstOrDefault(x => x.ContentType.Alias == nameof(DocumentTypes.Course))?.Properties[nameof(DocumentTypes.Course.CourseId)]?.Value;
           
            Lecture lecture = new Lecture
            {
                CourseId = int.Parse(courseId.ToString()),
                Title = lectureContent.Name,
                ExternalId = lectureContent.Id
            };
            return lecture;
        }

        private void AddOrUpdateLecture(IEnumerable<IContent> content, Func<IContent, bool> isActive)
        {
            IContent lectureContent = content.FirstOrDefault(x => x.ContentType.Alias == nameof(DocumentTypes.Lecture));
            if (lectureContent != null && lectureContent.HasIdentity)
            {
                Lecture lecture = GetLectureFromContent(lectureContent);
                lecture.IsActive = isActive(lectureContent);
                var lectures = DependencyResolver.Current.GetService(typeof(ILecturesService)) as ILecturesService;
                lectures.AddOrUpdate(lecture);
            }
        }
    }
}