namespace AcademyPlatform.Services.Contracts
{
    using System.Web.Security;

    public interface IMembersService
    {
        MembershipUser GetUser();

        MembershipUser GetUser(string username);

        bool ValidateCredentials(string username, string password);

        void ApproveUser(string username, string validationCode);

        MembershipUser CreateUser(string email, string password, string firstName, string lastName, bool requireEmailValidation, out MembershipCreateStatus status);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        string ResetPassword(string username);

        string GenerateValidationCode(string username);

        bool Login(string username, string password, bool isPersistent);

        void LogOut();
    }
}
