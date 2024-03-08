using MusicStorage.Data;
using MusicStorage.Models;

namespace MusicStorage.Repository
{
    public class MusicRepository : IMusicRepository
    {
        private readonly DataContext context;

        public MusicRepository(DataContext context) 
        {
            this.context = context;
        }

        public ICollection<Genre> GetGenres()
        {
            throw new NotImplementedException();
        }

        public ICollection<Track> GetTracks()
        {
            throw new NotImplementedException();
        }

        public ICollection<Track> SearchByArtist(string artist)
        {
            throw new NotImplementedException();
        }

        public ICollection<Track> SearchByGenre(Genre genre)
        {
            throw new NotImplementedException();
        }

        public ICollection<Track> SearchByTitel(string titel)
        {
            throw new NotImplementedException();
        }
    }
}
