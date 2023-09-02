using Microsoft.EntityFrameworkCore;
using moviereview.Data;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext context;
        public MovieRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> AddMovie(Movie movie)
        {
            await context.AddAsync(movie);
            return await Save();
        }

        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await context.Movies.Where(p => p.Id == id).FirstOrDefaultAsync();

            if (movie == null)
            {
                return false;
            }

            context.Remove(movie);
            return await Save();
        }

        public async Task<Movie> GetMovie(int id)
        {
            return await context.Movies.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<Movie>> GetMovies()
        {
            return await context.Movies.OrderBy(m => m.Id).ToListAsync();
        }

        public async Task<bool> UpdateMovie(Movie movie)
        {
            context.Update(movie);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> MovieExists(string title)
        {
            return await context.Movies.AnyAsync(m => m.Title.ToLower().Trim() == title.ToLower().Trim());
        }
    }
}