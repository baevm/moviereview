using moviereview.Models;

namespace moviereview.Interfaces
{
    public interface IMovieRepository
    {
        Task<ICollection<Movie>> GetMovies();
        Task<Movie> GetMovie(int id);
        Task<bool> AddMovie(Movie movie);
        Task<bool> UpdateMovie(Movie movie);
        Task<bool> DeleteMovie(int id);
        Task<bool> MovieExists(string title);
        Task<bool> Save();
    }
}