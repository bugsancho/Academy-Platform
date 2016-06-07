namespace AcademyPlatform.Web.Models.Payments
{
    using System;

    public class PaymentViewModel
    {
        public int Id { get; set; }

        public DateTime ApprovedDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        
    }
}
