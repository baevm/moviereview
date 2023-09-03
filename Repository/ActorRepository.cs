using Microsoft.EntityFrameworkCore;
using moviereview.Data;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Repository
{
    public class ActorRepository : IActorRepository
    {
        private readonly DataContext context;
        public ActorRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<bool> CreateActor(Actor actor)
        {
            await context.AddAsync(actor);
            return await Save();
        }

        public async Task<bool> DeleteActor(int id)
        {
            var actor = await GetActor(id);

            if (actor == null)
            {
                return false;
            }

            context.Remove(actor);
            return await Save();
        }

        public async Task<Actor> GetActor(int id)
        {
            return await context.Actors.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateActor(Actor actor)
        {
            context.Update(actor);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var saved = await context.SaveChangesAsync();
            return saved > 0;
        }
    }
}