using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;
using System.Runtime.InteropServices;

namespace MonarchsAPI_Net6.Services.RatingServices
{
    public class RatingService : IRatingService
    {
        private readonly DataContext _dbContext;
        public RatingService(DataContext context)
        {
            _dbContext = context;               
        }

        public async Task<List<Rating>> GetAll()
        {
            return await _dbContext.Ratings.ToListAsync();
        }

        public async Task<Rating?> GetById(int id)
        {
            Rating? rating = await _dbContext.Ratings.Where(r => r.Id == id).FirstOrDefaultAsync();
            return rating ?? null;
        }

        public async Task<bool> AddRating(CreateRatingDto ratingDto)
        {
            Rating newRating = new Rating
            {
                RatingValue = ratingDto.ratingValue,
                Comment = ratingDto.comment
            };
            try
            {
                await _dbContext.AddAsync(newRating);
                await _dbContext.SaveChangesAsync();
                return true;
            } 
            catch
            {
                return false;
            }
            
        }

        public async Task<Rating?> EditRating(Rating EditedRating)
        {
            Rating? ratingToEdit = await _dbContext.Ratings.Where(r => r.Id == EditedRating.Id).FirstOrDefaultAsync();
            if(ratingToEdit != null) 
            {
                ratingToEdit.RatingValue = EditedRating.RatingValue;
                ratingToEdit.Comment = EditedRating.Comment;
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return ratingToEdit;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public async Task<bool> DeleteRating(int id)
        {
            Rating? rating = await _dbContext.Ratings.Where(r => r.Id == id).FirstOrDefaultAsync();
            try 
            {
                if(rating != null) _dbContext.Remove(rating);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {

                return false;
            }
        }

        

        

        
    }
}
