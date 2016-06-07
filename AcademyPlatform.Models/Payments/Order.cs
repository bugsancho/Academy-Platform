using AcademyPlatform.Models.Base;

namespace AcademyPlatform.Models.Payments
{
    using System.Collections.Generic;

    public class Order : SoftDeletableLoggedEntity
    {
        private ICollection<LineItem> _lineItems;

        public Order()
        {
            _lineItems = new HashSet<LineItem>();
        }

        public int Id { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual Payment Payment { get; set; }

        public int? PaymentId { get; set; }

        public decimal Total { get; set; }

        public OrderStatusType Status { get; set; }

        public string ClientName { get; set; }

        public string Company { get; set; }

        /// <summary>
        /// Bulstat /EIK
        /// </summary>
        public string CompanyId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public virtual ICollection<LineItem> LineItems
        {
            get { return _lineItems; }
            set { _lineItems = value; }
        }
    }
}
