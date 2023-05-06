using AutoMapper;
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
        private readonly IMapper _mapper;
        public MonarchController(IMonarchServices moarchServices, IMapper mapper)
        {
            _monarchServices = moarchServices;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Monarch>>> GetAllMonarchs()
        {
            List<Monarch> monarchs = await _monarchServices.GetAll();
            return Ok(monarchs);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<List<MonarchResponseDashboardDto>>> GetAllMonarchsDashboard()
        {
            List<Monarch> monarchs = await _monarchServices.GetAll();
            List<MonarchResponseDashboardDto> monarchDtos = monarchs.Select(m => _mapper.Map<MonarchResponseDashboardDto>(m)).ToList();
            foreach(MonarchResponseDashboardDto monarchDto in monarchDtos)
            {
                foreach(Country country in monarchDto.Countries)
                {
                    monarchDto.CountryIds.Add(country.Id);
                }
            }

            return Ok(monarchDtos);
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
