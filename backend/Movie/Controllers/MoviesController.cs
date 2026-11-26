using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Movies;
using Movie.Exceptions;
using Movie.Services.Interfaces;

using static Movie.Common.ApplicationConstans;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController: ControllerBase
    {
        private readonly IMovieService _service;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService service, ILogger<MoviesController> logger)
        {
            _service = service;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("{id:long}")]
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
                _logger.LogError(ex, DefaultServerErrorMessage);
                return StatusCode(500, DefaultServerErrorMessage);
            }
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<MovieCollectionResponseDto>> GetAllMoviesAsync([FromQuery] int page = 1)
        {
            try
            {
                MovieCollectionResponseDto moviesDto = await _service
                    .GetMovieCollectionsAsync(page);
                
                return Ok(moviesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, DefaultServerErrorMessage);
                return StatusCode(500, DefaultServerErrorMessage);
            }
        }
    }
}