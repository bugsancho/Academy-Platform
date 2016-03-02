namespace AcademyPlatform.Models.Account
{
    public enum AccountCreationStatus
    {
        None,
        Success,
        DuplicateEmail,
        InvalidEmail,
        InvalidPassword,
        Other
    }
}
