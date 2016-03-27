namespace AcademyPlatform.Web.Umbraco.Controllers.SurfaceControllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Models.Emails;
    using AcademyPlatform.Services.Contracts;
    using AcademyPlatform.Web.Models.Other;

    using AutoMapper;

    using global::Umbraco.Web.Mvc;

    using Recaptcha.Web;
    using Recaptcha.Web.Mvc;

    public class ContactUsController : SurfaceController
    {
        private readonly IInquiryService _inquiryService;

        public ContactUsController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        [HttpPost]
        public ActionResult ContactUs(InquiryViewModel inquiryViewModel)
        {
            var validator = this.GetRecaptchaVerificationHelper().VerifyRecaptchaResponse();
            if (validator != RecaptchaVerificationResult.Success)
            {
                ModelState.AddModelError("ReCaptcha", "Моля, попълнете предизвикателството за да сме сигурни че не сте робот");
            }

            if (ModelState.IsValid)
            {
                Inquiry inquiry = Mapper.Map<Inquiry>(inquiryViewModel);
                _inquiryService.Create(inquiry);
                TempData["SuccessMessage"] =
                    "Успешно изпратихте Вашето запитване. Ще се свържем с вас на посочения e-mail адрес възможно най-скоро.";
                return RedirectToCurrentUmbracoPage();
            }

            return CurrentUmbracoPage();
        }

    }
}