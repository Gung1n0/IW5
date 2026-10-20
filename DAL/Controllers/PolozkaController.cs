using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DAL.Mappers;
using DAL.Models;
namespace DAL
{

    [ApiController]
    [Route("api/[controller]")]
    public class PolozkaController : ControllerBase
    {
        private readonly PolozkaService _polozkaService;
        private readonly TransferObjects _transferObjects;

        public PolozkaController(PolozkaService  polozkaService)
        {
            _polozkaService = polozkaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _polozkaService.GetAllPolzka();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _polozkaService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PolozkaEntity updatedEntity)
        {
            var result = await _polozkaService.UpdatePolozka(id, updatedEntity);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _polozkaService.RemovePolozka(id);
            if(result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePolozkaDTO InputDTO)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            if (!int.TryParse(userIdString, out int currentUserId))
                return Unauthorized();

            var NewId = await _polozkaService.CreatePolozkaAsync(InputDTO, currentUserId);

            var CreatedDto = await _polozkaService.GetByIdAsync(NewId);
            return CreatedAtAction(nameof(GetById), new { id = NewId }, CreatedDto);
        }
    }
}