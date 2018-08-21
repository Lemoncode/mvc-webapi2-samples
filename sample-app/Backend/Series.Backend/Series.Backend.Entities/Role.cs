using System.Collections.Generic;

namespace Series.Backend.Entities
{
    public class Role
    {
        // PK
        public int Id { get; set; }

        // Navigation properties
        public ICollection<UserProfile> UserProfiles { get; set; }

        // Properties
        public string Name { get; set; }
    }
}
