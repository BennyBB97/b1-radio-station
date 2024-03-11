using MusicStorage.Dto;
using MusicStorage.Models;

namespace MusicStorage.Interfaces
{
    public interface IMusicRepository
    {
        List<TrackDto> GetAllTracks();

        ICollection<Genre> GetGenres();

        ICollection<TrackDto> SearchTracks(SearchTracksDto searchTracks);      

        void CreateTrack(CreateTrackDto createTrack);

        void UpdateTrack(UpdateTrackDto track);

        void CreateArtistToTrack(ModifyArtistsDto artists);
        void DeleteArtistFromTrack(ModifyArtistsDto artists);

        bool CheckTrackHasUniqueArtists(List<string> artistNamens, string trackTitel);
        bool CheckTrackExist(string trackTitel);
        bool CheckTrackExist(int trackId);
        bool CheckGenreExist(int genreId);

        void DeleteTrack(int trackId);
    }
}
