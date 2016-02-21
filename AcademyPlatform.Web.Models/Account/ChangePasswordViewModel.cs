namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordViewModel
    {
        [Display(Name = "Стара парола")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Нова парола")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Потвърждение на парола")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
