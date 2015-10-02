namespace AcademyPlatform.Services
{
    using System.Web.Security;

    using AcademyPlatform.Services.Contracts;

    public class MembersService : IMembersService
    {
        public MembersService()
        {
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
            user.Comment = "asd";
            Membership.UpdateUser(user);
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
