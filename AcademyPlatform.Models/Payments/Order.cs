namespace AcademyPlatform.Models.Payments
{
    using System;
    using System.Collections.Generic;

    public class Order
    {
        private ICollection<LineItem> _lineItems;

        public Order()
        {
            _lineItems = new HashSet<LineItem>();
        }
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual Payment Payment { get; set; }

        public int PaymentId { get; set; }

        public decimal Total { get; set; }

        public OrderStatusType Status { get; set; }

         public virtual  ICollection<LineItem> LineItems
        {
            get { return _lineItems; }
            set { _lineItems = value; }
        }
    }
}
