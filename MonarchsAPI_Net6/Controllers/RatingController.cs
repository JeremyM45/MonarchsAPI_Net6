using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.RatingServices;

namespace MonarchsAPI_Net6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingController : ControllerBase
    {

        IRatingServices _ratingServices;
        
        public RatingController(IRatingServices ratingServices)
        {

            _ratingServices = ratingServices;
        }

        [HttpGet]
        public async Task<ActionResult<List<Rating>>> GetAllRatings()
        {
            List<Rating> ratings = await _ratingServices.GetAll();
            if (ratings == null) { return NotFound(); }
            return Ok(ratings);
        }
    }
}
