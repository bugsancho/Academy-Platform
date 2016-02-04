namespace AcademyPlatform.Web.Models.Courses
{
    using System.ComponentModel.DataAnnotations;

    using AcademyPlatform.Web.Models.Account;

    public class JoinCourseViewModel
    {
        public string CourseNiceUrl { get; set; }

        public bool RequiresBillingInfo { get; set; }

        public BillingInfoViewModel BillingInfo { get; set; }

        public string LicenseTermsUrl { get; set; }

        [Display(Name = "Общи условия.")]
        public bool AcceptLicenseTerms { get; set; }

        public string CourseName { get; set; }
    }
}
