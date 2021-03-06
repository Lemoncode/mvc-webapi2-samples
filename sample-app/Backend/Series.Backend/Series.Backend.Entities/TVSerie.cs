﻿using System;

namespace Series.Backend.Entities
{
    public class TVSerie
    {
        // PK
        public int Id { get; set; }

        // FK
        public int GenreId { get; set; }

        // Navigation properties
        public Genre Genre { get; set; }

        // Properties
        public string Title { get; set; }
        public DateTime OriginalRelease { get; set; }
        public string Creator { get; set; }
        public bool Complete { get; set; }
    }
}
