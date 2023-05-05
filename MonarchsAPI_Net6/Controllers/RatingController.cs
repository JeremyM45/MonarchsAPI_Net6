﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.RatingDtos;
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

        [HttpPost]
        public async Task<ActionResult<Rating>> AddRating(CreateRatingDto ratingDto)
        {
            Rating newRating = new Rating
            {
                RatingValue = ratingDto.ratingValue,
                Comment = ratingDto.comment,
                UserId = ratingDto.UserId
            };
            if(await _ratingServices.AddRating(newRating))
            {
                return CreatedAtAction(nameof(AddRating), newRating);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRating(int id)
        {
            if (await _ratingServices.DeleteRating(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult> EditUser(EditRatingDto ratingDto)
        {
            if (await _ratingServices.EditRating(ratingDto))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
