namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using AcademyPlatform.Models;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class UpdateProfileViewModel : IMapFrom<User>
    {
        public string SuccessMessage { get; set; }

        public bool IsSuccessful { get; set; }

        [Display(Name = "Име")]
        public string FirstName { get; set; }

        [Display(Name = "Презиме")]
        public string MiddleName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Организация")]
        public string Company { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }
    }
}
