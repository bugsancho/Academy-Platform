namespace AcademyPlatform.Web.Models.Payments
{
    using System;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class PaymentEditViewModel : IMapFrom<Payment>
    {
        [HiddenInput]
        public int SubscriptionId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Total { get; set; }

        public string BankAccount { get; set; }

        public string Details { get; set; }
    }
}
