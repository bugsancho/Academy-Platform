namespace AcademyPlatform.Web.Models.Payments
{
    public class AwaitingPaymentViewModel
    {
        public int SubscriptionId { get; set; }

        public string CourseName { get; set; }

        public decimal? CoursePrice { get; set; }
        
    }
}
