using MusicStorage.Dto;
using MusicStorage.Models;

namespace MusicStorage.Interfaces
{
    public interface IMusicRepository
    {
        List<TrackDto> GetAllTracks();
        List<GenreDto> GetGenres();
        Track getTrack(int trackId);
        Artist getArtist(string artistName);
        List<ArtistDto> GetAllArtistsFromTrack(int trackId);
        List<TrackDto> SearchTracks(string searchTerm, int type);
        TrackDto getCompleteTrack(int trackId);
        int getArtistCount(int trackId);
        void CreateTrack(CreateTrackDto createTrack);
        void UpdateTrack(UpdateTrackDto track);
        void AddArtistToTrack(int trackId, ArtistDto artist);    
        bool CheckTrackHasUniqueArtists(List<string> artistNamens, string trackTitel);
        bool CheckTrackHasUniquieArtistAfterUpdate(int trackId, string newTrackTitel);
        bool CheckTrackExist(string trackTitel);
        bool CheckTrackExist(int trackId);
        bool CheckGenreExist(int genreId);
        bool CheckArtistExist(int artistId);
        bool CheckArtistExist(string artistName);
        bool CheckArtistBelongsToTrack(int trackId, int artistId);
        void DeleteArtistFromTrack(int trackId, int artistId);        
        void DeleteTrack(int trackId);
    }
}
