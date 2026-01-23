using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Dtos.Director.Request;
using Movie.Dtos.Director.Response;
using Movie.Services.Interfaces;

namespace Movie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DirectorController: ControllerBase
    {
        private readonly IDirectorService _service;

        public DirectorController(IDirectorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorReasponse>>> GetAllDirectorsAsync()
            => Ok(await _service.GetAllDirectorsAsync());
        
        [HttpGet("{id:long}")]
        public async Task<IActionResult?> GetDirectorByIdAsync(long id)
            => Ok(await _service.GetDirectorByIdAsync(id));

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateDirectorAsync([FromBody] CreateDirectorRequestDto request)
        {
            
            var result = await _service.CreateDirectorAsync(request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPut("{id:long}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateDirectorAsync(long id, [FromBody] UpdateDirectorRequestDto request)
            => Ok(await _service.UpdateDirectorAsync(id, request));

        [HttpDelete("{id:long}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteDirectorAsync(long id)
        {
            await _service.DeleteDirectorAsync(id);
            return NoContent();
        }
        
    }
}