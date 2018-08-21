using Series.Backend.Entities;

namespace Series.Backend.Contracts
{
    public interface ISecurityRepository
    {
        void InsertPasswordHashByUserId(string passwordHash, int userId);
        UserProfile GetUserProfileByUserEmail(string email);
        void CreateUserSession(string token, int userId);
        string GetLastUserToken(int userId);
        int GetUserRole(int userId);
    }
}
