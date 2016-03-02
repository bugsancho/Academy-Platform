namespace AcademyPlatform.Services.Contracts
{
    public interface IRouteProvider
    {
        string GetRouteByName(string routeName, object routeValues);

        string Host { get; }

        string GetValidateAccountRoute(string email, string validationCode);

        string GetForgotPasswordRoute(string email, string validationCode);
    }
}
