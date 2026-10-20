using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

using IW5_project.API.Common.Models;
using IW5_project.Api.DAL.Common.Entities;
using IW5_project.Api.DAL.Common.Repositories;
using IW5_project.API.BL.Mappers;

namespace IW5_project.Api.App.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly ItemService _ItemService;
        private readonly TransferObjects _transferObjects;

        public ItemController(ItemService  ItemService)
        {
            _ItemService = ItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ItemService.GetAllItems();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _ItemService.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemEntity updatedEntity)
        {
            var result = await _ItemService.UpdateItem(id, updatedEntity);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _ItemService.RemoveItem(id);
            if(result == null)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemDTO InputDTO)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
                return Unauthorized();

            if (!int.TryParse(userIdString, out int currentUserId))
                return Unauthorized();

            var NewId = await _ItemService.CreateItemAsync(InputDTO, currentUserId);

            var CreatedDto = await _ItemService.GetByIdAsync(NewId);
            return CreatedAtAction(nameof(GetById), new { id = NewId }, CreatedDto);
        }
    }
}