using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Controllers
{
    [Route("api/actor")]
    [ApiController]
    public class ActorController : ControllerBase
    {
        private readonly IActorRepository actorRepository;
        public ActorController(IActorRepository actorRepository)
        {
            this.actorRepository = actorRepository;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Actor>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActor(int id)
        {
            var actor = await actorRepository.GetActor(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (actor == null)
            {
                return NotFound();
            }

            return Ok(actor);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Actor>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateActor(Actor actor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCreated = await actorRepository.CreateActor(actor);


            if (!isCreated)
            {
                ModelState.AddModelError("", $"Something went wrong creating actor {actor.FirstName} {actor.LastName}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created actor");
        }

        [Authorize]
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateActor()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            throw new NotImplementedException();
        }

        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteActor(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actor = await actorRepository.GetActor(id);

            if (actor == null)
            {
                ModelState.AddModelError("", "Actor does not exist");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var isActorDeleted = await actorRepository.DeleteActor(id);

            if (!isActorDeleted)
            {
                ModelState.AddModelError("", $"Something went wrong deleting actor {actor.FirstName} {actor.LastName}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully deleted actor");
        }
    }
}