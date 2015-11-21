namespace AcademyPlatform.Services
{
    using System;
    using System.Linq;
    using System.Web.Security;

    using AcademyPlatform.Common.Providers;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Services.Contracts;

    public class MembersService : IMembersService
    {
        private const int ValidationCodeLength = 6;

        private readonly IRepository<User> _users;
        private readonly IRandomProvider _random;

        public MembersService(IRepository<User> users, IRandomProvider random)
        {
            _users = users;
            _random = random;
        }


        public MembershipUser GetUser()
        {
            return Membership.GetUser();
        }

        public MembershipUser GetUser(string username)
        {
            return Membership.GetUser(username);
        }

        public bool ValidateCredentials(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public void ApproveUser(string username, string validationCode)
        {
            var userInDb = _users.All().FirstOrDefault(x => x.Username == username);
            if (userInDb == null)
            {
                throw new ArgumentException("Невалидно потребителско име", nameof(username));
            }

            if (userInDb.ValidationCode == validationCode)
            {
                var user = GetUser(username);
                user.IsApproved = true;
                Membership.UpdateUser(user);
            }
            else
            {
                throw new ArgumentException("Невалиден валидационен код", nameof(validationCode));
            }

        }

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, bool requireEmailValidation, out MembershipCreateStatus status)
        {
            var user = Membership.CreateUser(email, password, email, "n/q", "n/q", !requireEmailValidation, null, out status);
            if (status == MembershipCreateStatus.Success)
            {
                var dbUser = new User
                {
                    Username = email,
                    FirstName = firstName,
                    LastName = lastName,
                    RegistrationDate = DateTime.Now
                };
                _users.Add(dbUser);
                _users.SaveChanges();
            }

            return user;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = GetUser(username);
            return user.ChangePassword(oldPassword, newPassword);
        }

        public string ResetPassword(string username)
        {
            var user = GetUser(username);
            return user.ResetPassword();
        }

        public string GenerateValidationCode(string username)
        {
            var code = _random.GenerateRandomCode(ValidationCodeLength);
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new ArgumentException("Невалидно потребителско име", nameof(username));
            }

            user.ValidationCode = code;
            _users.SaveChanges();
            return code;
        }

        public bool Login(string username, string password, bool isPersistent)
        {
            if (ValidateCredentials(username, password))
            {
                FormsAuthentication.SetAuthCookie(username, isPersistent);
                return true;
            }

            return false;
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

    }
}
