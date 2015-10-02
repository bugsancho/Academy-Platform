namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [Display(Name = "Запомни ме за 2 седмици")]
        [DisplayName("asdads")]
        public bool RememberMe { get; set; }
    }
}
