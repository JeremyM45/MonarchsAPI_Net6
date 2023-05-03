using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.MonarchServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonarchController : ControllerBase
    {
        private readonly IMonarchServices _moarchServices;
        public MonarchController(IMonarchServices moarchServices)
        {
            _moarchServices = moarchServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Monarch>>> GetAllMonarchs()
        {
            List<Monarch> monarchs = await _moarchServices.GetAll();
            return Ok(monarchs);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Monarch>> GetMonarchById(int id)
        {
            Monarch monarch = await _moarchServices.GetById(id);
            if(monarch == null) { return NotFound();  }
            return Ok(monarch);
        }

        [HttpPost]
        public async Task<ActionResult> AddMonarch(CreateMonarchDto newMonarchDto)
        {
            if(await _moarchServices.AddMonarch(newMonarchDto))
            {
                return CreatedAtAction(nameof(AddMonarch), await _moarchServices.GetAll());
            }
            return BadRequest();

        }
    }
}
