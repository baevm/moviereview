using Microsoft.AspNetCore.Mvc;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreRepository genreRepository;
        public GenreController(IGenreRepository genreRepository)
        {
            this.genreRepository = genreRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Genre>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = await genreRepository.GetGenre(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (genre == null)
            {
                return NotFound();
            }

            return Ok(genre);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGenre([FromBody] Genre genre)
        {
            if (genre == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isGenreCreated = await genreRepository.CreateGenre(genre);

            if (!isGenreCreated)
            {
                ModelState.AddModelError("", $"Something went wrong creating the genre {genre.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully created genre");
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGenre()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var genre = await genreRepository.GetGenre(id);

            if (genre == null)
            {
                ModelState.AddModelError("", "Genre does not exist");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var isGenreDeleted = await genreRepository.DeleteGenre(id);

            if (!isGenreDeleted)
            {
                ModelState.AddModelError("", $"Something went wrong deleting the genre {genre.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully deleted genre");
        }
    }
}
