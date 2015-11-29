namespace AcademyPlatform.Services.Contracts
{
    using AcademyPlatform.Models;

    public interface IUserService
    {
        User GetByUsername(string username);
    }
}