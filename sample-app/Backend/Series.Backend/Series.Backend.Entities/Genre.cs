using System.Collections.Generic;

namespace Series.Backend.Entities
{
    public class Genre
    {
        // PK
        public int Id { get; set; }

        // Navigation properties
        public ICollection<TVSerie> TVSerieCollection { get; set; }

        // Properties
        public string Description { get; set; }
    }
}
