using moviereview.Models;

namespace moviereview.Interfaces
{
    public interface IActorRepository
    {
        Task<Actor> GetActor(int id);
        Task<bool> CreateActor(Actor actor);
        Task<bool> UpdateActor(Actor actor);
        Task<bool> DeleteActor(int id);
        Task<bool> Save();
    }
}