using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    public class CoursesController : RenderMvcController
    {
        public override ActionResult Index(RenderModel model)
        {

            return base.Index(model);
        }
    }
}