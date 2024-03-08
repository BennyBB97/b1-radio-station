﻿namespace MusicStorage.Models
{
    public class Track
    {
        public int TrackId { get; set; }
        public string? Titel { get; set; }
        public Genre? Genre { get; set; }
        public ICollection<Artist>? Artists { get; set; }
    }
}
