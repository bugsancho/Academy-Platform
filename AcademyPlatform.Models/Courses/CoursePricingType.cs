namespace AcademyPlatform.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    public enum CoursePricingType
    {
        None,
        [Display(Name = "Безплатен")]
        Free,
        [Display(Name = "Платен достъп")]
        PaidAccess,
        [Display(Name = "Платен сертификат")]
        PaidCertificate,
    }
}