namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ResendValidationEmailViewModel
    {
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }
}
