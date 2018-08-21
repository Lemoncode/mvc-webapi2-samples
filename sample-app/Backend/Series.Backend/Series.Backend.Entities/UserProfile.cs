using System.Collections.Generic;

namespace Series.Backend.Entities
{
    public class UserProfile
    {
        // PK
        public int Id { get; set; }

        // FK
        public int RoleId { get; set; }

        // Navaigation properties
        public Role Role { get; set; }
        public ICollection<UserSession> UserSessions { get; set; }
        
        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email  { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
