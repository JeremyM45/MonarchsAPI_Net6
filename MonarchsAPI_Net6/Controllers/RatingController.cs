using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.RatingDtos;
using MonarchsAPI_Net6.Models;
using MonarchsAPI_Net6.Services.RatingServices;
using System.Data;

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

        [HttpPost, Authorize(Roles = "User")]
        public async Task<ActionResult<Rating>> AddRating(CreateRatingDto ratingDto)
        {
            Rating newRating = new Rating
            {
                RatingValue = ratingDto.RatingValue,
                Comment = ratingDto.Comment,
                UserId = ratingDto.UserId,
                MonarchId = ratingDto.MonarchId
            };
            if(await _ratingServices.AddRating(newRating))
            {
                return CreatedAtAction(nameof(AddRating), newRating);
            }
            return BadRequest();
        }

        [HttpDelete, Authorize(Roles = "User")]
        public async Task<ActionResult> DeleteRating(int id)
        {
            if (await _ratingServices.DeleteRating(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut, Authorize(Roles = "User")]
        public async Task<ActionResult> EditRating(EditRatingDto ratingDto)
        {
            if (await _ratingServices.EditRating(ratingDto))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
