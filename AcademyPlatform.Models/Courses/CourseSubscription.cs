namespace AcademyPlatform.Models.Courses
{
    using System;

    public class CourseSubscription
    {
        public int CourseId { get; set; }

        public int UserId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public SubscriptionStatus Status { get; set; }

        public virtual Course Course { get; set; }

        public virtual User User { get; set; }

    }
}
