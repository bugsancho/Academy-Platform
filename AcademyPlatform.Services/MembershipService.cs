﻿namespace AcademyPlatform.Services
{
    using System;
    using System.Linq;
    using System.Web.Security;

    using AcademyPlatform.Common.Providers;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Services.Contracts;

    public class MembershipService : IMembershipService
    {
        private const int ValidationCodeLength = 6;

        private readonly IRepository<User> _users;
        private readonly IRandomProvider _random;

        public MembershipService(IRepository<User> users, IRandomProvider random)
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
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user != null && user.IsApproved == false)
            {
                throw new UserNotApprovedException(username);
            }

            return Membership.ValidateUser(username, password);
        }

        public bool IsApproved(string username)
        {
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            return user.IsApproved;
        }

        public bool ApproveUser(string username, string validationCode)
        {
            if (string.IsNullOrWhiteSpace(validationCode))
            {
                throw new ArgumentNullException(nameof(validationCode));
            }

            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            if (user.ValidationCode == validationCode)
            {
                user.ValidationCode = string.Empty;
                user.IsApproved = true;
                _users.SaveChanges();
            }

            return user.IsApproved;
        }

        public MembershipUser CreateUser(string email, string password, string firstName, string lastName, out MembershipCreateStatus status)
        {
            var membershipUser = Membership.CreateUser(email, password, email, "n/q", "n/q", true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                var user = new User
                {
                    Username = email,
                    FirstName = firstName,
                    LastName = lastName,
                    RegistrationDate = DateTime.Now
                };
                _users.Add(user);
                _users.SaveChanges();
            }

            return membershipUser;
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (ValidateCredentials(username, oldPassword))
            {
                var membershipUser = GetUser(username);
                return membershipUser.ChangePassword(oldPassword, newPassword);
            }

            return false;

        }

        public string ResetPassword(string username)
        {
            var membershipUser = GetUser(username);
            return membershipUser.ResetPassword();
        }

        public string GenerateValidationCode(string username)
        {
            var code = _random.GenerateRandomCode(ValidationCodeLength);
            var user = _users.All().FirstOrDefault(x => x.Username == username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
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
        public void Login(string username)
        {

            FormsAuthentication.SetAuthCookie(username, true);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
        }

    }
}