using System.Collections.Generic;

namespace Series.Backend.Entities
{
    public class User
    {
        // PK
        public int Id { get; set; }

        // Navigation properties
        public ICollection<TVSerieUser> TVSerieUserCollection { get; set; }

        // Properties
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
