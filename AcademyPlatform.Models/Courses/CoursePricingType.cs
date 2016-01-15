namespace AcademyPlatform.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    public enum CoursePricingType
    {
        None,
        [Display(Name = "Безплатен")]
        Free,
        PaidAccess,
        PaidCertificate,
        PaidAccessAndCertificate
    }
}