namespace AcademyPlatform.Common.Providers
{
    using System;

    public class RandomProvider : IRandomProvider
    {
        private static readonly Random _rnd = new Random();

        public string GenerateRandomCode(int length, string allowedCharacters = "abcdefghijklmnopqrstuvwxyz1234567890")
		{
			var allowedCharactersUpperCase = allowedCharacters.ToUpperInvariant();
			var code = new char[length];
			for (int i = 0; i < code.Length; i++)
			{
				code[i] = allowedCharactersUpperCase[_rnd.Next(allowedCharactersUpperCase.Length)];
			}

			return new string(code);
		} 
    }
}
