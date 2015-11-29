namespace AcademyPlatform.Services
{
    using System.Linq;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
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
    }
}
