namespace AcademyPlatform.Services
{
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class UserService : IUserService
    {
        private readonly IRepository<User> _users;

        public UserService(IRepository<User> users)
        {
            _users = users;
        }

        public User GetByUsername(string username)
        {
            return _users.All().FirstOrDefault(x => x.Username == username);
        }

        public BillingInfo GetBillingInfo(string username)
        {
            User user = GetByUsername(username);
            return user.BillingInfo;
        }

        public void UpdateUser(User user)
        {
           _users.Update(user);
            //TODO implement UoW properly
            _users.SaveChanges();
        }
    }
}
