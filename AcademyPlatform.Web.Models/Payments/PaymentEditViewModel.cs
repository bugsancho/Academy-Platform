namespace AcademyPlatform.Web.Models.Payments
{
    using System.ComponentModel.DataAnnotations;

    using System;
    using System.Web.Mvc;

    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Web.Infrastructure.Mappings;

    public class PaymentEditViewModel : IMapFrom<Payment>
    {
        [HiddenInput]
        [Display(Name = "Номер на абонамента")]
        public int SubscriptionId { get; set; }

        [Display(Name = "Дата на транзакцията")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Обща сума на транзакцията")]
        public decimal Total { get; set; }

        [Display(Name = "Банкова сметка")]
        public string BankAccount { get; set; }

        [Display(Name = "Детайли за транзакцията")]
        public string Details { get; set; }
    }
}
