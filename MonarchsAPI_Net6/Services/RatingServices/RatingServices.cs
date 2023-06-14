using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.RatingDtos;
using MonarchsAPI_Net6.Models;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace MonarchsAPI_Net6.Services.RatingServices
{
    public class RatingServices : IRatingServices
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RatingServices(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = context;
            _httpContextAccessor = httpContextAccessor;
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
            User? user = await _dbContext.Users.FindAsync(rating.UserId);
            Monarch? monarch = await _dbContext.Monarchs.Include(m => m.Ratings).Where(m => m.Id == rating.MonarchId).FirstOrDefaultAsync();
            if(user != null && monarch != null && VerifyJwT(user.UserName))
            {
                rating.Monarch = monarch;
                rating.User = user;
                try
                {
                    await _dbContext.AddAsync(rating);
                    await _dbContext.SaveChangesAsync();
                    if (await UpdateAverageRatingValue(monarch))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            
            return false;
        }

        public async Task<bool> EditRating(EditRatingDto ratingDto)
        {
            Rating? ratingToEdit = await _dbContext.Ratings.Where(r => r.Id == ratingDto.Id).FirstOrDefaultAsync();
            if(ratingToEdit != null) 
            {
                User? ratingsUser = await _dbContext.Users.FindAsync(ratingToEdit.UserId);
                if (ratingsUser != null && VerifyJwT(ratingsUser.UserName))
                {
                    ratingToEdit.RatingValue = ratingDto.RatingValue;
                    ratingToEdit.Comment = ratingDto.Comment;
                    Monarch? monarch = await _dbContext.Monarchs.Include(m => m.Ratings).Where(m => m.Id == ratingToEdit.MonarchId).FirstOrDefaultAsync();
                    if (monarch != null)
                    {
                        try
                        {
                            await _dbContext.SaveChangesAsync();
                            if (await UpdateAverageRatingValue(monarch))
                            {
                                return true;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> DeleteRating(int id)
        {
            Rating? rating = await _dbContext.Ratings.Where(r => r.Id == id).FirstOrDefaultAsync();
            try 
            {
                if (rating != null) { 
                    User? user = await _dbContext.Users.FindAsync(rating.UserId);
                    if (user != null && VerifyJwT(user.UserName))
                    {
                        _dbContext.Remove(rating);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }
                }
                return false;
            }
            catch(Exception ex)
            {

                throw ex;
            }
        }

        public bool VerifyJwT(string username)
        {
            return username == _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
        private async Task<bool> UpdateAverageRatingValue(Monarch monarch)
        {
            int numOfAllMRatings = monarch.Ratings.Count;
            float totalRatingsValue = 0f;
            foreach(Rating rating in monarch.Ratings)
            {
                totalRatingsValue += rating.RatingValue;
            }
            try
            {
                Monarch? monarchToUpdate = await _dbContext.Monarchs.FindAsync(monarch.Id);
                if(monarchToUpdate != null)
                {
                    monarchToUpdate.AverageRating = totalRatingsValue / numOfAllMRatings;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return false;
        }
    }
}
