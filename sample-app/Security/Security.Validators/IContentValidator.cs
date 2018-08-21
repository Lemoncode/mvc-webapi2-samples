namespace Security.Validators
{
    public interface IContentValidator
    {
        bool IsValidHashedContent(string content, string salt, string hashedContent);
    }
}
