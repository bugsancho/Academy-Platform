namespace AcademyPlatform.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class BillingInfoViewModel
    {
        [Display(Name = "Малко име")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилно име")]
        public string LastName { get; set; }

        [Display(Name = "Име на организацията")]
        public string Company { get; set; }

        /// <summary>
        /// Bulstat /EIK
        /// </summary>
        [Display(Name = "БУЛСТАТ (ЕИК)")]
        public string CompanyId { get; set; }

        [Display(Name = "Личен адрес")]
        public string Address { get; set; }

        [Display(Name = "Град")]
        public string City { get; set; }

    }
}
