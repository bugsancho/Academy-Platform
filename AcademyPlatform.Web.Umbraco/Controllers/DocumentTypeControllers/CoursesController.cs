namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.Common;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Core.Models;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CoursesController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ICategoryService _categories;
        private readonly IUmbracoMapper _mapper;


        public CoursesController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public override ActionResult Index(RenderModel model)
        {
            var coursesContentCollection = model.Content.Descendants(nameof(DocumentTypes.Course));
            var coursesContentViewModels = new List<CourseContentViewModel>();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .MapCollection(coursesContentCollection, coursesContentViewModels, new Dictionary<string, PropertyMapping>
                    {
                      {
                          "CourseUrl", new PropertyMapping
                              {
                                  SourceProperty = "Url",
                              }
                      },
                    });

            var courses = _courses.GetActiveCourses().ToList();

            var coursesViewModels = courses.Join(
                coursesContentViewModels,
                course => course.Id,
                coursesContent => coursesContent.CourseId,
                (course, coursesContent) => new CoursesListViewModel() { Title = course.Title, CourseUrl = coursesContent.CourseUrl,ImageUrl = coursesContent.CoursePicture.Url,ShortDescription = coursesContent.ShortDescription , Category = "2"}).ToList();

            ViewBag.Categories = _categories.GetAll();
            return CurrentTemplate(coursesViewModels);
        }


    }


}