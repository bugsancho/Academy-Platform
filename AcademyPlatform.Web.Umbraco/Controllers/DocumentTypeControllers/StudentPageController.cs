namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Common;
    using AcademyPlatform.Web.Models.Courses;
    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;
    using AcademyPlatform.Web.Umbraco.UmbracoConfiguration;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    using Zone.UmbracoMapper;

    public class StudentPageController : RenderMvcController
    {
        private readonly ICoursesService _courses;
        private readonly ICategoryService _categories;
        private readonly IUmbracoMapper _mapper;

         public StudentPageController(ICoursesService courses, ICategoryService categories, IUmbracoMapper mapper)
        {
            _courses = courses;
            _categories = categories;
            _mapper = mapper;
        }
        
        [HttpGet]
        public override ActionResult Index(RenderModel model)
        {
            throw new ArgumentException("This should never be called - student page!!!" + Request.Url.AbsolutePath);
     
        }
    }
}