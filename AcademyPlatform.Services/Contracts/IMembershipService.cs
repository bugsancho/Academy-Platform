namespace AcademyPlatform.Services.Contracts
{
    using System.Web.Security;

    public interface IMembershipService
    {
        MembershipUser GetUser();

        MembershipUser GetUser(string username);

        bool ValidateCredentials(string username, string password);

        bool ApproveUser(string username, string validationCode);

        MembershipUser CreateUser(string email, string password, string firstName, string lastName, out MembershipCreateStatus status);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        string ResetPassword(string username);

        string GenerateValidationCode(string username);

        bool Login(string username, string password, bool isPersistent);

        void LogOut();

        bool IsApproved(string username);

        void Login(string username);
    }
}
