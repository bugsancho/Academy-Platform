namespace AcademyPlatform.Services.Contracts
{
    using System.Web.Security;

    public interface IMembersService
    {
        MembershipUser GetUser();

        MembershipUser GetUser(string username);

        bool ValidateUser(string username, string password);
        
        bool ChangePassword(string username, string oldPassword, string newPassword);

        string ResetPassword(string username);

        bool Login(string username, string password, bool isPersistent);

        void LogOut();

        MembershipUser CreateUser(string email, string password, string firstName, string lastName, bool requireEmailValidation, out MembershipCreateStatus status);

        void ApproveUser(string username);
    }
}
