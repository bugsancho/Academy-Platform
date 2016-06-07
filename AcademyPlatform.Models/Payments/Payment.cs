namespace AcademyPlatform.Models.Payments
{
    using System;

    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Courses;

    public class Payment : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        public int SubscriptionId { get; set; }

        public virtual CourseSubscription Subscription { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Total { get; set; }

        public string BankAccount { get; set; }

        public string Details { get; set; }

        //public int? OnlineTransactionId { get; set; }

        //public virtual OnlineTransaction OnlineTransaction { get; set; }

        //public PaymentType Type { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
