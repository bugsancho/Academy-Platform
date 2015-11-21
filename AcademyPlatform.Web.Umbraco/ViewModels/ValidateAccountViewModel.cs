namespace AcademyPlatform.Web.Umbraco.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class ValidateAccountViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string ValidationCode { get; set; }
    }
}