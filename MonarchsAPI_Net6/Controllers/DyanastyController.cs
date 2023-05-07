using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.DynastyServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DyanastyController : ControllerBase
    {
        private readonly IDynastyServices _dynastyServices;
        private readonly IMapper _mapper;

        public DyanastyController(IDynastyServices dynastyServices, IMapper mapper)
        {
            _mapper = mapper;
            _dynastyServices = dynastyServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<DynastyResponseDto>>> GetAllDynasties()
        {
            List<Dynasty> dynasties = await _dynastyServices.GetAll();

            return Ok(dynasties.Select(d => _mapper.Map<DynastyResponseDto>(d)));
        }
        [HttpGet("min")]
        public async Task<ActionResult<List<DynastyResponseDto>>> GetAllDynastiesMin()
        {
            List<Dynasty> dynasties = await _dynastyServices.GetAll();

            return Ok(dynasties.Select(d => _mapper.Map<DynastyResponseMinDto>(d)));
        }
    }
}
