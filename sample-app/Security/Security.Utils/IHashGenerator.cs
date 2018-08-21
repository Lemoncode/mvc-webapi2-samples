namespace Security.Utils
{
    public interface IHashGenerator
    {
        string SaltedContentHash(string content, string salt);
    }
}
