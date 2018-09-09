using System;

namespace Backend.Entities
{
    public class Serie
    {
        // PK
        public int Id { get; set; }
        
        // Properties
        public string Title { get; set; }
        public DateTime OriginalRelease { get; set; }
        public string Creator { get; set; }
        public bool Complete { get; set; }
    }
}
