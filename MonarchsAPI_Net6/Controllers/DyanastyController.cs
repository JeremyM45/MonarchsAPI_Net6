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
        public DyanastyController(IDynastyServices dynastyServices)
        {
            _dynastyServices = dynastyServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<DynastyWithMonarchsDto>>> GetAllDynasties()
        {
            List<DynastyWithMonarchsDto> dynasties = await _dynastyServices.GetAll();
            return Ok(dynasties);
        }
    }
}
