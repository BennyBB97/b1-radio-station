namespace MusicStorage.Dto
{
    public class ModifyArtistsDto
    {
        public int TrackId { get; set; }
        public List<ArtistDto> Artists { get; set; }
    }
}
