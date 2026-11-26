using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Movies;
using Movie.Exceptions;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController: ControllerBase
    {
        private readonly IMovieService _service;
        private readonly ILogger<MovieController> _logger;

        public MovieController(IMovieService service, ILogger<MovieController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("/{id:long}")]
        public async Task<ActionResult<MovieDetailsResponseDto>> GetMovieDetailsAsync([FromRoute]long id)
        {
            try
            {
                MovieDetailsResponseDto movieDetails = await _service.GetMovieDetailsAsync(id);
                return Ok(movieDetails);
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogInformation(ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving movie details.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}