using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.MonarchServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonarchController : ControllerBase
    {
        private readonly IMonarchServices _monarchServices;
        public MonarchController(IMonarchServices moarchServices)
        {
            _monarchServices = moarchServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Monarch>>> GetAllMonarchs()
        {
            List<Monarch> monarchs = await _monarchServices.GetAll();
            return Ok(monarchs);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Monarch>> GetMonarchById(int id)
        {
            Monarch monarch = await _monarchServices.GetById(id);
            if(monarch == null) { return NotFound();  }
            return Ok(monarch);
        }

        [HttpPost]
        public async Task<ActionResult> AddMonarch(CreateMonarchRequestDto newMonarchDto)
        {
            if (await _monarchServices.AddMonarch(newMonarchDto))
            {
                return CreatedAtAction(nameof(AddMonarch), await _monarchServices.GetByName(newMonarchDto.Name));
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> EditMonarch(EditMonarchRequestDto editedMonarchDto)
        {
            return Ok(await _monarchServices.EditMonarch(editedMonarchDto));
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteMonarch(int id)
        {
            if(await _monarchServices.RemoveMonarch(id))
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
