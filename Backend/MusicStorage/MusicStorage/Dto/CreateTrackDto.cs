namespace MusicStorage.Dto
{
    public class CreateTrackDto
    {
        public string Titel { get; set; }
        public int GenreId { get; set; }
        public List<string> ArtistNames{ get; set; }
    }
}
