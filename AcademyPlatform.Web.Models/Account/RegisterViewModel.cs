namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterViewModel
    {
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        
        [Display(Name = "Име")]
        public string FirstName { get; set; }

        //[Required]
        //[Display(Name = "Презиме")]
        //public string MiddleName { get; set; }
        
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        //[Display(Name = "Организация")]
        //public string Company { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Парола")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Потвърждение на парола")]
        public string ConfirmPassword { get; set; }
        }
}
