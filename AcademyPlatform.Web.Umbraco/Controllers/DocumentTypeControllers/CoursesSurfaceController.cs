namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using Services.Contracts;
    using Models.Common;
    using Models.Courses;

    using DocumentTypes = DocumentTypeModels;
    using UmbracoConfiguration;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CoursesSurfaceController : SurfaceController
    {
        private readonly ICoursesService _courses;

        private readonly IUmbracoMapper _mapper;


        public CoursesSurfaceController(ICoursesService courses, IUmbracoMapper mapper)
        {
            _courses = courses;
            _mapper = mapper;
        }

        [ChildActionOnly]
        public ActionResult RenderCoursesGrid()
        {
            var coursesContentCollection = Umbraco.TypedContentAtRoot().DescendantsOrSelf(nameof(DocumentTypes.Course));
            var coursesContentViewModels = new List<DocumentTypes.Course>();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .MapCollection(coursesContentCollection, coursesContentViewModels, new Dictionary<string, PropertyMapping>
                    {
                      {
                          "CourseUrl", new PropertyMapping
                              {
                                  SourceProperty = "Url",
                              }
                      }
                    });

            var courses = _courses.GetActiveCourses();

            var coursesViewModels = courses.Join(
                coursesContentViewModels,
                course => course.Id,
                coursesContent => coursesContent.CourseId,
                (course, coursesContent) => new CoursesListViewModel
                {
                    Title = course.Title,
                    CourseUrl = coursesContent.CourseUrl,
                    ImageUrl = coursesContent.CoursePicture.Url,
                    ShortDescription = coursesContent.ShortDescription,
                    Category = course.Category
                }).ToList();

            //TODO Find out why(if) distinct works without implementing IEquitable<T>
            ViewBag.Categories = coursesViewModels.Select(x => x.Category).Distinct();
            return PartialView(coursesViewModels);
        }


    }


}