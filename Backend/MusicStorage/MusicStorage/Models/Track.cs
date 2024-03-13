namespace MusicStorage.Models
{
    public class Track
    {
        public int TrackId { get; set; }
        public string Titel { get; set; }
        public Genre Genre { get; set; }
        public List<TrackArtist> TrackArtists{ get; set; }
        
    }
}
