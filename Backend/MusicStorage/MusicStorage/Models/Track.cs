namespace MusicStorage.Models
{
    public class Track
    {
        public int TrackId { get; set; }
        public string Titel { get; set; }
        public Genre Genre { get; set; }
        public List<Artist> Artists { get; set; }
        public ICollection<TrackArtist> TrackArtists{ get; set; }
        
    }
}
