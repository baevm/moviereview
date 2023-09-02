using System.Net;
using Microsoft.AspNetCore.Mvc;
using moviereview.Dto;
using moviereview.Interfaces;
using moviereview.Models;

namespace moviereview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : Controller
    {
        private readonly IMovieRepository movieRepository;
        public MovieController(IMovieRepository movieRepository)
        {
            this.movieRepository = movieRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Movie>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await movieRepository.GetMovies();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(movies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Movie))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMovie(int id)
        {
            var movie = await movieRepository.GetMovie(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
        {
            if (movieDto == null)
            {
                return BadRequest(ModelState);
            }

            if (await movieRepository.MovieExists(movieDto.Title))
            {
                ModelState.AddModelError("", "Movie Exists");
                return StatusCode(StatusCodes.Status400BadRequest, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = new Movie()
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                ReleaseDate = movieDto.ReleaseDate,
                Budget = movieDto.Budget,
            };

            var isMovieAdded = await movieRepository.AddMovie(movie);

            if (!isMovieAdded)
            {
                ModelState.AddModelError("", $"Something went wrong adding the movie {movie.Title}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully added movie");
        }


        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieDto movieDto, int id)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await movieRepository.GetMovie(id);

            if (movie == null)
            {
                ModelState.AddModelError("", "Movie does not exist");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var isMovieDeleted = await movieRepository.DeleteMovie(id);

            if (!isMovieDeleted)
            {
                ModelState.AddModelError("", $"Something went wrong deleting the movie {movie.Title}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return Ok("Successfully deleted movie");
        }
    }
}