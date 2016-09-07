namespace AcademyPlatform.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment> _payments;

        public PaymentService(IRepository<Payment> payments)
        {
            _payments = payments;
        }

        public IEnumerable<Payment> GetByUser(string username)
        {
            List<Payment> payments =
                _payments.All()
                         .Where(x => x.Subscription.User.Username == username && x.Status == PaymentStatus.Completed)
                         .ToList();

            return payments;
        }
    }
}
