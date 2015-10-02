namespace AcademyPlatform.Services.Contracts
{
    using System.Web.Security;

    public interface IMembersService
    {
        MembershipUser GetUser();

        MembershipUser GetUser(string username);

        bool ValidateUser(string username, string password);

        MembershipUser CreateUser(string email, string password, bool requireEmailValidation, out MembershipCreateStatus status);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        string ResetPassword(string username);

        bool Login(string username, string password, bool isPersistent);

        void LogOut();
    }
}
