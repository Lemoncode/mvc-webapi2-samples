namespace Series.Backend.Entities
{
    public class TVSerieUser
    {
        // PK
        public int Id { get; set; }
        
        // FK
        public int UserId { get; set; }
        public int TVSerieId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public TVSerie TVSerie { get; set; }
    }
}
