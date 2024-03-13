namespace MusicStorage.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public List<TrackArtist> TrackArtists { get; set; }
       
    }
}
