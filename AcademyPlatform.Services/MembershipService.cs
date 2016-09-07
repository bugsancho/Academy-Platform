namespace AcademyPlatform.Services
{
    using System;
    using System.Web.Security;

    using AcademyPlatform.Common.Providers;
    using AcademyPlatform.Data.Repositories;
    using AcademyPlatform.Models;
    using AcademyPlatform.Models.Account;
    using AcademyPlatform.Models.Exceptions;
    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    public class MembershipService : IMembershipService
    {
        private const int ValidationCodeLength = 6;

        private readonly IRepository<User> _users;
        private readonly IRandomProvider _randomProvider;

        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRouteProvider _routeProvider;

        public MembershipService(IRepository<User> users, IRandomProvider randomProvider, IMessageService messageService, IRouteProvider routeProvider, IUserService userService)
        {
            _users = users;
            _randomProvider = randomProvider;
            _messageService = messageService;
            _routeProvider = routeProvider;
            _userService = userService;
        }

        public MembershipUser GetUser(string username)
        {
            return Membership.GetUser(username);
        }

        public bool ValidateCredentials(string username, string password)
        {
            User user = _userService.GetByUsername(username);
            if (user != null && user.IsApproved == false)
            {
                throw new UserNotApprovedException(username);
            }

            return Membership.ValidateUser(username, password);
        }

        public bool IsApproved(string username)
        {
            User user = _userService.GetByUsername(username);
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

            User user = _userService.GetByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            if (user.ValidationCode == validationCode)
            {
                user.ValidationCode = string.Empty;
                user.IsApproved = true;
                _users.SaveChanges();
                Login(user.Username);
            }

            return user.IsApproved;
        }

        public AccountCreationStatus CreateUser(string email, string password, string firstName, string lastName)
        {
            //TODO add extra properties
            MembershipCreateStatus membershipCreateStatus;
            Membership.CreateUser(email, password, email, "n/q", "n/q", true, out membershipCreateStatus);
            if (membershipCreateStatus == MembershipCreateStatus.Success)
            {
                User user = new User
                {
                    Username = email,
                    FirstName = firstName,
                    LastName = lastName,
                    BillingInfo = new BillingInfo { FirstName = firstName, LastName = lastName }
                };

                _users.Add(user);
                _users.SaveChanges();
                string validationLink = _routeProvider.GetValidateAccountRoute(email, GenerateValidationCode(email));
                _messageService.SendAccountValidationMessage(user, validationLink);

            }

            AccountCreationStatus status = MapMembershipCreateStatus(membershipCreateStatus);
            return status;
        }

        public void ResendValidationEmail(string username)
        {
            User user = _userService.GetByUsername(username);
            string validationLink = _routeProvider.GetValidateAccountRoute(user.Username, GenerateValidationCode(user.Username));
            _messageService.SendAccountValidationMessage(user, validationLink);
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (ValidateCredentials(username, oldPassword))
            {
                MembershipUser membershipUser = GetUser(username);
                return membershipUser.ChangePassword(oldPassword, newPassword);
            }

            return false;
        }

        public void ResetPassword(string username)
        {
            MembershipUser membershipUser = GetUser(username);
            User user = _userService.GetByUsername(username);
            //TODO Don't send passwords in emails!!!
            string newPassword = membershipUser.ResetPassword();
            _messageService.SendForgotPasswordMessage(user, newPassword);
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

        private void Login(string username)
        {
            FormsAuthentication.SetAuthCookie(username, true);
        }

        private string GenerateValidationCode(string username)
        {
            string code = _randomProvider.GenerateRandomCode(ValidationCodeLength);
            User user = _userService.GetByUsername(username);
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            user.ValidationCode = code;
            _users.SaveChanges();
            return code;
        }

        private AccountCreationStatus MapMembershipCreateStatus(MembershipCreateStatus membershipCreateStatus)
        {
            AccountCreationStatus status;
            switch (membershipCreateStatus)
            {
                case MembershipCreateStatus.Success:
                    status = AccountCreationStatus.Success;
                    break;
                case MembershipCreateStatus.DuplicateUserName:
                case MembershipCreateStatus.DuplicateEmail:
                    status = AccountCreationStatus.DuplicateEmail;
                    break;
                case MembershipCreateStatus.InvalidEmail:
                case MembershipCreateStatus.InvalidUserName:
                    status = AccountCreationStatus.InvalidEmail;
                    break;
                case MembershipCreateStatus.InvalidPassword:
                    status = AccountCreationStatus.InvalidPassword;
                    break;
                case MembershipCreateStatus.InvalidQuestion:
                case MembershipCreateStatus.InvalidAnswer:
                case MembershipCreateStatus.UserRejected:
                case MembershipCreateStatus.InvalidProviderUserKey:
                case MembershipCreateStatus.DuplicateProviderUserKey:
                case MembershipCreateStatus.ProviderError:
                    status = AccountCreationStatus.Other;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(membershipCreateStatus));
            }

            return status;
        }
    }
}
