using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.RatingDtos;
using MonarchsAPI_Net6.Models;
using System.Runtime.InteropServices;

namespace MonarchsAPI_Net6.Services.RatingServices
{
    public class RatingServices : IRatingServices
    {
        private readonly DataContext _dbContext;
        public RatingServices(DataContext context)
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

        public async Task<bool> AddRating(Rating rating)
        {
            User user = await _dbContext.Users.FindAsync(rating.UserId);
            rating.User = user;
            try
            {
                await _dbContext.AddAsync(rating);
                await _dbContext.SaveChangesAsync();
                return true;
            } 
            catch
            {
                return false;
            }
            
        }

        public async Task<bool> EditRating(EditRatingDto ratingDto)
        {
            Rating? ratingToEdit = await _dbContext.Ratings.Where(r => r.Id == ratingDto.Id).FirstOrDefaultAsync();
            if(ratingToEdit != null) 
            {
                ratingToEdit.RatingValue = ratingDto.RatingValue;
                ratingToEdit.Comment = ratingDto.Comment;
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
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
