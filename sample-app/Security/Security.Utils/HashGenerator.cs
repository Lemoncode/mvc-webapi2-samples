using System;
using System.Security.Cryptography;
using System.Text;

namespace Security.Utils
{
    public class HashGenerator : IHashGenerator
    {
        public string SaltedContentHash(string content, string salt)
        {
            string saltedContent = $"{content}{salt}";
            return ToHash(saltedContent);
        }

        protected string ToHash(string input)
        {
            using (SHA512CryptoServiceProvider sha = new SHA512CryptoServiceProvider())
            {
                byte[] dataToHash = Encoding.UTF8.GetBytes(input);
                byte[] hashed = sha.ComputeHash(dataToHash);
                return Convert.ToBase64String(hashed);
            }
        }
    }
}
