using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using global::Umbraco.Web.Mvc;

    public class StudentSectionController : UmbracoController
    {
        // GET: StudentSection
        public ActionResult RenderNavigation()
        {
            return View("_Header");
        }
    }
}