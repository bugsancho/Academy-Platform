namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Services.Contracts;

    using global::Umbraco.Web.Mvc;

    public class CertificateController : UmbracoController
    {
        private readonly ICertificatesService _certificates;

        public CertificateController(ICertificatesService certificates)
        {
            _certificates = certificates;
        }

        public ActionResult Certificate(string certificateUniqueCode)
        {
            var certificate = _certificates.GetByUniqueCode(certificateUniqueCode);
            if (certificate != null)
            {
                return View(model: $"\\certificates\\{certificateUniqueCode}.jpeg");
            }

            //ViewBag.AssessmentSuccessful = false;
            return HttpNotFound();

        }
    }
}