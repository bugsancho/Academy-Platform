namespace AcademyPlatform.Models.Payments
{
    using System;

    using AcademyPlatform.Models.Courses;

    public class Payment
    {
        public int Id { get; set; }

        public virtual Course Course { get; set; }

        public int CourseId { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Total { get; set; }

        public string Details { get; set; }

        public PaymentType Type { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
