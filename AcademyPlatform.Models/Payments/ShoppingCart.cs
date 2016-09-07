namespace AcademyPlatform.Models.Payments
{
    using System.Collections.Generic;
    using AcademyPlatform.Models.Base;

    public class ShoppingCart : SoftDeletableLoggedEntity
    {
        public int Id { get; set; }


        private ICollection<LineItem> _lineItems;

        public ShoppingCart()
        {
            _lineItems = new HashSet<LineItem>();
        }

        public virtual ICollection<LineItem> LineItems
        {
            get { return _lineItems; }
            set { _lineItems = value; }
        }
    }
}
