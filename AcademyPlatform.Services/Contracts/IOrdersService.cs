namespace AcademyPlatform.Services.Contracts
{
    using System.Collections.Generic;

    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Courses;
    using AcademyPlatform.Models.Payments;

    public interface IOrdersService
    {
        Order Generate(Course course, User user);

        IEnumerable<Order> GetByUser(string username);

        Order GetById(int orderId);
    }
}