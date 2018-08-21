using System;

namespace Security.Utils
{
    public class SaltGenerator
    {
        public string GenerateRandomSalt()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
