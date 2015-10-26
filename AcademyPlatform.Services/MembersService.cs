namespace AcademyPlatform.Services
{
    using System;
    using System.Web.Security;

    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Services.Contracts;

    public class MembersService : IMembersService
    {
        private readonly IRepository<User> _users;

        public MembersService(IRepository<User> users)
        {
            _users = users;
        }

        public MembershipUser GetUser()
        {
            return Membership.GetUser();
        }

        public MembershipUser GetUser(string username)
        {
            return Membership.GetUser(username);
        }

        public bool ValidateUser(string username, string password)
        {
            return Membership.ValidateUser(username, password);
        }

        public MembershipUser CreateUser(string email, string password, bool requireEmailValidation, out MembershipCreateStatus status)
        {
            var user = Membership.CreateUser(email, password, email, "n/q", "n/q", !requireEmailValidation, null, out status);
            if (status == MembershipCreateStatus.Success)
            {
                var dbUser = new User
                {
                    Username = email,
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

        public bool Login(string username, string password, bool isPersistent)
        {
            if (ValidateUser(username, password))
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
