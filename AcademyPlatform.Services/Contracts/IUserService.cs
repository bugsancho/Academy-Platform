namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Payments;

    public interface IUserService
    {
        User GetByUsername(string username);

        BillingInfo GetBillingInfo(string username);

        void UpdateUser(User user);
    }
}