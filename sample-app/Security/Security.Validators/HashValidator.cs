using Security.Utils;

namespace Security.Validators
{
    public class HashValidator : IContentValidator
    {
        private HashGenerator _hashGenerator = new HashGenerator();

        public bool IsValidHashedContent(string content, string salt, string hashedContent)
        {
            return _hashGenerator.SaltedContentHash(content, salt) == hashedContent;
        }
    }
}
