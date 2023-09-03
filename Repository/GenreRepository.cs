using Microsoft.EntityFrameworkCore;
using moviereview.Data;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext context;
        public GenreRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateGenre(Genre genre)
        {
            await context.AddAsync(genre);
            return await Save();
        }

        public async Task<bool> DeleteGenre(int id)
        {
            var genre = await GetGenre(id);

            if (genre == null)
            {
                return false;
            }

            context.Remove(genre);
            return await Save();
        }

        public async Task<Genre> GetGenre(int id)
        {
            return await context.Genres.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateGenre(Genre genre)
        {
            context.Update(genre);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }
    }
}