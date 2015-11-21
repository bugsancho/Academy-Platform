namespace AcademyPlatform.Common.Providers
{
    public interface IRandomProvider
    {
        string GenerateRandomCode(int length, string allowedCharacters = "abcdefghijklmnopqrstuvwxyz1234567890");
    }
}