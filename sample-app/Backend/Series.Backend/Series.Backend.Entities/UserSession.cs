using System;

namespace Series.Backend.Entities
{
    public class UserSession
    {
        // PK
        public int Id { get; set; }

        // FK
        public int UserProfileId { get; set; }
        
        // Navigation properties
        public UserProfile UserProfile { get; set; }

        // Properties
        public string Token { get; set; }
        public DateTime Creation { get; set; }
    }
}
