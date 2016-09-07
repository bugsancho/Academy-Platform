namespace AcademyPlatform.Models.Courses
{
    using System;

    using AcademyPlatform.Models.Base;
    using AcademyPlatform.Models.Payments;

    public class CourseSubscription : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }

        //Unique constraint on(CourseId & UserId)
        public int CourseId { get; set; }

        public int UserId { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        public SubscriptionType SubscriptionType { get; set; }

        public int? OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }

    }
}
