namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;
    using DocumentTypes = DocumentTypeModels;
    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class CourseController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ICategoryService _categories;
        private readonly IUmbracoMapper _mapper;


        public CourseController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
        }

        [HttpGet]
        public override ActionResult Index(RenderModel model)
        {
            var coursePublishedContentViewModel = new DocumentTypes.Course();
            _mapper.AddCustomMapping(typeof(ImageViewModel).FullName, UmbracoMapperMappings.MapMediaFile)
                   .Map(model.Content.AncestorOrSelf(), coursePublishedContentViewModel, new Dictionary<string, PropertyMapping>
                    {//TODO extract and reuse
                      {
                          "CourseUrl", new PropertyMapping
                              {
                                  SourceProperty = "Url",
                              }
                      }
                    });

            var course = _courses.GetById(coursePublishedContentViewModel.CourseId);
            var courseDetailsViewModel = new CourseDetailsViewModel
            {
                Category = course.Category,
                Title = course.Title,
                LecturerName = "The great Lecturer",
                ImageUrl = coursePublishedContentViewModel.CoursePicture.Url,
                CoursesPageUrl = model.Content.Parent.Url,
                DetailedDescription = coursePublishedContentViewModel.DetailedDescription
            };


            return CurrentTemplate(courseDetailsViewModel);
        }


    }
}