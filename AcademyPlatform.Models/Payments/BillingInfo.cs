namespace AcademyPlatform.Models.Payments
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class BillingInfo
    {
        [Key, ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        /// <summary>
        /// Bulstat /EIK
        /// </summary>
        public string CompanyId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}
