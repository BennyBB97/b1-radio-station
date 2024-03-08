using MusicStorage.Models;

namespace MusicStorage.Repository
{
    public interface IMusicRepository
    {
        ICollection<Track> GetTracks();

        ICollection<Track> SearchByTitel(string titel);

        ICollection<Track> SearchByGenre(Genre genre);

        ICollection<Track> SearchByArtist(string artist);

        ICollection<Genre> GetGenres();
    }
}
