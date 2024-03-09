﻿using MusicStorage.Models;

namespace MusicStorage.Dto
{
    public class TrackDto
    {
        public int TrackId { get; set; }
        public string Titel { get; set; }
        public GenreDto Genre { get; set; }
        public ICollection<ArtistDto> Artists { get; set; }
    }
}
