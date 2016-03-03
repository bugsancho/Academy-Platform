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

    using nuPickers;

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
                            "{0}?returnUrl={1}",
                            pcr.PublishedContent.UrlWithDomain(),
                            HttpUtility.UrlEncode(pcr.RoutingContext.UmbracoContext.HttpContext.Request.RawUrl));

                        pcr.SetRedirect(redirectUrl);
                    }
                };

            ContentService.Saving += (sender, args) =>
                {
                    AddOrUpdateLecture(args.SavedEntities, x => x.Published && (x.Parent()?.Published ?? false));
                };

            //Adding a Save post-event because during the Saving event, new items still have no Id so syncing won't work.
            ContentService.Saved += (sender, args) =>
                {
                    AddOrUpdateLecture(args.SavedEntities, x => x.Published && (x.Parent()?.Published ?? false));
                };

            ContentService.UnPublishing += (sender, args) =>
                {
                    AddOrUpdateLecture(args.PublishedEntities, x => false);
                };

            ContentService.RollingBack += (sender, args) =>
                {
                    AddOrUpdateLecture(Enumerable.Repeat(args.Entity, 1), x => x.Published && (x.Parent()?.Published ?? false));
                };

            ContentService.Moving += (sender, args) =>
                {
                    AddOrUpdateLecture(args.MoveInfoCollection.Select(x => x.Entity), x => x.Published && (x.Parent()?.Published ?? false));
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

        private void AddOrUpdateLecture(IEnumerable<IContent> content, Func<IContent, bool> isActive)
        {
            IContent lectureContent = content.FirstOrDefault(x => x.ContentType.Alias == nameof(DocumentTypes.Lecture));
            IContent moduleContent = content.FirstOrDefault(x => x.ContentType.Alias == nameof(DocumentTypes.Module));
            if (lectureContent != null && lectureContent.HasIdentity)
            {
                var lecturesService = DependencyResolver.Current.GetService(typeof(ILecturesService)) as ILecturesService;
                if (lecturesService == null)
                {
                    throw new InvalidOperationException("LecturesService failed to instantiate");
                }

                Lecture lecture = lecturesService.GetLectureByExternalId(lectureContent.Id);
                if (lecture == null)
                {
                    lecture = new Lecture
                    {
                        ExternalId = lectureContent.Id,
                        Title = lectureContent.Name
                    };
                }
                if (lectureContent.Parent()?.ContentType.Alias == nameof(DocumentTypes.Course))
                {
                    Picker courseIdPicker = lectureContent.Parent().Properties[(nameof(DocumentTypes.Course.CourseId))].Value as Picker;
                    if (courseIdPicker == null)
                    {
                        throw new ArgumentException($"Course with Id: {lectureContent.Parent().Id} doesn't have a CourseId field present");
                    }

                    lecture.CourseId = int.Parse(courseIdPicker.PickedKeys.First());
                }

                lecture.IsActive = isActive(lectureContent);
                lecturesService.AddOrUpdate(lecture);
            }
            else if (moduleContent != null)
            {
                var lecturesService = DependencyResolver.Current.GetService(typeof(ILecturesService)) as ILecturesService;
                if (lecturesService == null)
                {
                    throw new InvalidOperationException("LecturesService failed to instantiate");
                }

                bool isModuleActive = isActive(moduleContent);
                foreach (var lect in moduleContent.Descendants().Where(x => x.ContentType.Alias == nameof(DocumentTypes.Lecture)))
                {
                    Lecture lecture = lecturesService.GetLectureByExternalId(lect.Id);
                    lecture.IsActive = isModuleActive && isActive(lect);
                    lecturesService.AddOrUpdate(lecture);
                }

            }
        }
    }
}