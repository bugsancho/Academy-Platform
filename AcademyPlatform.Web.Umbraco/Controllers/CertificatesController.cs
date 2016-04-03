namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Models.Certificates;
    using AcademyPlatform.Services.Contracts;

    using global::Umbraco.Web.Mvc;

    public class CertificateController : UmbracoController
    {
        private readonly ICertificatesService _certificates;

        public CertificateController(ICertificatesService certificates)
        {
            _certificates = certificates;
        }

        public ActionResult Certificate(string certificateCode)
        {
            Certificate certificate = _certificates.GetByUniqueCode(certificateCode);
            if (certificate != null)
            {
                return View(model: $"\\certificates\\{certificateCode}.jpeg");
            }

            //ViewBag.AssessmentSuccessful = false;
            return HttpNotFound();

        }
    }
}