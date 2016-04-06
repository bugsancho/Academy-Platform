namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class BillingInfoViewModel : IMapFrom<BillingInfo>
    {
        [Display(Name = "Малко име")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилно име")]
        public string LastName { get; set; }

        [Display(Name = "Организация (ако е приложимо)*:")]
        public string Company { get; set; }

        /// <summary>
        /// Bulstat /EIK
        /// </summary>
        [Display(Name = "ЕИК/Булстат (ако е приложимо)*:")]
        public string CompanyId { get; set; }

        [Display(Name = "Адрес (ул., номер):")]
        public string Address { get; set; }

        [Display(Name = "Град/село:")]
        public string City { get; set; }

    }
}
