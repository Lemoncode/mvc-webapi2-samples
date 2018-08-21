using System;

namespace Security.Utils
{
    public class TokenGenerator : ITokenGenerator
    {
        private SaltGenerator _saltGenerator = new SaltGenerator();
        private HashGenerator _hashGenerator = new HashGenerator();

        public string GenerateToken()
        {
            string token = Guid.NewGuid().ToString("B");
            string salt = _saltGenerator.GenerateRandomSalt();
            return _hashGenerator.SaltedContentHash(token, salt);
        }
    }
}
