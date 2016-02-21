namespace AcademyPlatform.Common.Providers
{
    using System;

    public class RandomProvider : IRandomProvider
    {
        private static readonly Random Rnd = new Random();

        public string GenerateRandomCode(int length, string allowedCharacters = "abcdefghijklmnopqrstuvwxyz1234567890")
		{
			string allowedCharactersUpperCase = allowedCharacters.ToUpperInvariant();
			char[] code = new char[length];
			for (int i = 0; i < code.Length; i++)
			{
				code[i] = allowedCharactersUpperCase[Rnd.Next(allowedCharactersUpperCase.Length)];
			}

			return new string(code);
		} 
    }
}
