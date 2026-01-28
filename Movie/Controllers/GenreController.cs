using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Genre.Request;
using Movie.Dtos.Genre.Response;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController: ControllerBase
    {
        private readonly IGenreService _service;

        public GenreController(IGenreService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GenreResponse>>> GetAllGenresAsync()
            => Ok(await _service.GetAllGenreAsync());

        [HttpGet("{id:long}")]
        public async Task<IActionResult?> GetGenreById(long id)
            => Ok(await _service.GetGenreByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> CreateGenreAsync([FromBody] CreateGenreRequestDto request)
        {
            var result = await _service.CreateGenreAsync(request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateGenreAsync(long id, [FromBody] UpdateGenreRequestDto request)
        {
            var result = await _service.UpdateGenreAsync(id, request)
                ?? throw new Exception("Genre not found");

            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteGenreAsync(long id)
        {
            await _service.DeleteGenreAsync(id);
            return NoContent();
        }
        
    }
}