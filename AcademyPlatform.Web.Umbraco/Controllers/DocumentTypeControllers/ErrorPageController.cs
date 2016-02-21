namespace AcademyPlatform.Web.Umbraco.Controllers.DocumentTypeControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Web.Models.Umbraco.DocumentTypes;

    using global::Umbraco.Web;
    using global::Umbraco.Web.Models;
    using global::Umbraco.Web.Mvc;

    public class ErrorPageController : RenderMvcController
    {
        // GET: ErrorPage
        public override ActionResult Index(RenderModel model)
        {
            Response.StatusCode = model.Content.GetPropertyValue<int>(nameof(ErrorPage.ErrorCode));
            //Response.TrySkipIisCustomErrors = true;
            return base.Index(model);
        }
    }
}