using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.RatingServices
{
    public interface IRatingServices
    {
        Task<List<Rating>> GetAll();
        Task<Rating?> GetById(int id);
        Task<bool> AddRating(CreateRatingDto ratingDto);
        Task<Rating?> EditRating(Rating rating);
        Task<bool> DeleteRating(int id);
    }
}
