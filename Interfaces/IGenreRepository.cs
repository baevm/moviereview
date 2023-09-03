using moviereview.Models;

namespace moviereview.Interfaces
{
    public interface IGenreRepository
    {
        Task<Genre> GetGenre(int id);
        Task<bool> CreateGenre(Genre genre);
        Task<bool> UpdateGenre(Genre genre);
        Task<bool> DeleteGenre(int id);
        Task<bool> Save();
    }
}