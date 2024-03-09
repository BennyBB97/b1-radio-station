using MusicStorage.Dto;
using MusicStorage.Models;

namespace MusicStorage.Interfaces
{
    public interface IMusicRepository
    {
        ICollection<Track> GetTracks();

        ICollection<Genre> GetGenres();

        ICollection<Track> SearchTracks(SearchTracksDto searchTracks);      

        void createTrack(Track track);

        void updateTrack(Track track);
    }
}
