using Series.Backend.Contracts;
using Series.Backend.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace Series.Backend.Models.Repositories
{
    public class SecurityRepository : ISecurityRepository
    {
        private Contexts.SecurityContext _context;

        public SecurityRepository()
        {
            _context = new Contexts.SecurityContext();
        }

        public SecurityRepository(string connString)
        {
            _context = new Contexts.SecurityContext(connString);
        }
        
        public UserProfile GetUserProfileByUserEmail(string email)
        {
            // TODO: Alternative queries
            return _context.UserProfiles
                .Where(s => s.Email == email)
                .Select(p => new { p.Salt, p.PasswordHash, p.Id })
                .AsNoTracking()
                .ToList()
                .Select(d => new UserProfile { Salt = d.Salt, PasswordHash = d.PasswordHash, Id = d.Id })
                .FirstOrDefault();
        }

        public void InsertPasswordHashByUserId(string passwordHash, int userId)
        {
            var userProfile = _context.UserProfiles.SingleOrDefault(u => u.Id == userId);
            if (userProfile != null)
            {
                userProfile.PasswordHash = passwordHash;
                _context.SaveChanges();
            }
        }

        public void CreateUserSession(string token, int userId)
        {
            var userSession = new UserSession
            {
                Creation = DateTime.Now, // TODO: Create an index for sorting??
                Token = token,
                UserProfileId = userId,
            };
            _context.UserSessions.Add(userSession);
            _context.SaveChanges();
        }

        public string GetLastUserToken(int userId)
        {
            // TODO: Study different options
            return _context.UserSessions
                .Where(u => u.UserProfileId == userId)
                .Select(u => new { u.Token, u.Creation })
                .AsNoTracking()
                .ToList()
                .Select(u => new UserSession { Creation = u.Creation, Token = u.Token })
                .OrderByDescending(d => d.Creation)
                .First()
                .Token;
        }

        public int GetUserRole(int userId)
        {
            return _context.UserProfiles
                .SingleOrDefault(u => u.Id == userId)
                .RoleId;
        }
    }
}
