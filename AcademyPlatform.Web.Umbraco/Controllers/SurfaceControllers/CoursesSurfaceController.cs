namespace AcademyPlatform.Web.Umbraco.Controllers.SurfaceControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CoursesSurfaceController : SurfaceController
    {
        private readonly ICoursesService _courses;

        private readonly ISubscriptionsService _subscriptions;

        private readonly IUmbracoMapper _mapper;

        public CoursesSurfaceController(ICoursesService courses, IUmbracoMapper mapper, ISubscriptionsService subscriptions)
        {
            _courses = courses;
            _mapper = mapper;
            _subscriptions = subscriptions;
        }

        [ChildActionOnly]
        public ActionResult RenderCoursesGrid(int numberOfCourses, bool shouldHideFilters)
        {
            IEnumerable<IPublishedContent> coursesContentCollection = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(Course)).OrderByDescending(x => x.CreateDate);
            if (numberOfCourses != default(int))
            {
                coursesContentCollection = coursesContentCollection.Take(numberOfCourses);
            }

            List<Course> coursesContentViewModels = new List<Course>();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .MapCollection(coursesContentCollection, coursesContentViewModels, new Dictionary<string, PropertyMapping>
                    {
                      {
                          "Url", new PropertyMapping
                              {
                                  SourceProperty = "Url",
                              }
                      }
                    });

            IEnumerable<AcademyPlatform.Models.Courses.Course> courses = _courses.GetActiveCourses().ToList();

            List<CoursesListViewModel> coursesViewModels = courses.Join(
                coursesContentViewModels,
                course => course.Id,
                coursesContent => coursesContent.CourseId,
                (course, coursesContent) => new CoursesListViewModel
                {
                    Title = course.Title,
                    CourseUrl = coursesContent.Url,
                    ImageUrl = coursesContent.CoursePicture.Url,
                    ShortDescription = coursesContent.ShortDescription,
                    Category = course.Category,
                    IsJoined = User.Identity.IsAuthenticated && _subscriptions.HasActiveSubscription(User.Identity.Name, course.Id),
                    JoinCourseUrl = Url.RouteUrl("JoinCourse", new { courseNiceUrl = coursesContent.UrlName }),

                }).ToList();

            //TODO Find out why(if) distinct works without implementing IEquitable<T>
            ViewBag.Categories = coursesViewModels.Select(x => x.Category).Distinct();
            ViewBag.ShouldHideFilters = shouldHideFilters;
            return PartialView(coursesViewModels);
        }

    }


}