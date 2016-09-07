namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models.Account;

    public interface IMembershipService
    {
        bool ValidateCredentials(string username, string password);

        bool ApproveUser(string username, string validationCode);

        AccountCreationStatus CreateUser(string email, string password, string firstName, string lastName);

        bool ChangePassword(string username, string oldPassword, string newPassword);

        void ResetPassword(string username);

        bool Login(string username, string password, bool isPersistent);

        void LogOut();

        bool IsApproved(string username);

        void ResendValidationEmail(string username);
    }
}
