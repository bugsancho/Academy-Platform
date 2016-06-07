namespace AcademyPlatform.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class OrdersService : IOrdersService
    {

        private readonly IRepository<Order> _orders;

        public OrdersService(IRepository<Order> orders)
        {
            _orders = orders;
        }

        public Order GetById(int orderId)
        {
            Order order = _orders.GetById(orderId);
            return order;
        }

        public Order Generate(Course course, User user)
        {
            Product product = course.Products.FirstOrDefault(x => x.IsActive && x.Type == ProductType.CourseAccess);
            if (product == null)
            {
                throw new ArgumentException($"No active product found for course with id: {course.Id}");
            }

            List<LineItem> lineItems = new List<LineItem>
                                           {
                                               new LineItem { Product = product, Quantity = 1 }
                                           };
            var billingInfo = user.BillingInfo;
            Order order = new Order
            {
                LineItems = lineItems,
                Status = OrderStatusType.AwaitingPayment,
                User = user,
                Total = product.Price,
                ClientName = $"{billingInfo.FirstName} {billingInfo.LastName}",
                Address = billingInfo.Address,
                City = billingInfo.City,
                Company = billingInfo.Company,
                CompanyId = billingInfo.CompanyId
            };

            _orders.Add(order);
            _orders.SaveChanges();

            return order;
        }

        public IEnumerable<Order> GetByUser(string username)
        {
            List<Order> orders = _orders.All().Where(x => x.User.Username == username).ToList();
            return orders;
        }
    }
}
