namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;

    using AcademyPlatform.Models.Payments;

    public interface IPaymentService
    {
        IEnumerable<Payment> GetByUser(string username);
    }
}