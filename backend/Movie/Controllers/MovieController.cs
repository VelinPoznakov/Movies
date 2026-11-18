using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Movies.Request;
using Movie.Dtos.Movies.Response;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController: ControllerBase
    {
        private readonly IMovieService _service;

        public MovieController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieResponseDto>>> GetAllMoviesAsync()
            => Ok(await _service.GetAllMovies());

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetMovieByIdAsync(long id)
            => Ok(await _service.GetMovieById(id));

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMovieAsync([FromBody] CreateMovieRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User is not authenticated");

            var result = await _service.CreateMovie(request, userId)
                ?? throw new Exception("Error creating movie");
            
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id:long}")]
        [Authorize]
        public async Task<IActionResult> UpdateMovieAsync(long id, [FromBody] UpdateMovieRequestDto request)
        {
            var result = await _service.UpdateMovie(id, request)
                ?? throw new Exception("Movie not found");

            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:long}")]
        [Authorize]
        public async Task<IActionResult> DeleteMovieAsync(long id)
        {
            await _service.DeleteMovie(id);
            return NoContent();
        }

        [HttpPut("{id:long}/rating")]
        [Authorize]
        public async Task<IActionResult> UpdateMovieRatingAsync(long id, [FromBody] MovieUpdateRatingDto request)
        {
            var result = await _service.UpdateMovieRating(id, request);

            return result == null ? NotFound() : Ok(result);
        }
    }
}