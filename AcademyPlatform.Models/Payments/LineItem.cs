﻿namespace AcademyPlatform.Models.Payments
{
    public class LineItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public virtual Order Order { get; set; }

        public int OrderId { get; set; }

        public decimal Discount { get; set; }
    }
}
