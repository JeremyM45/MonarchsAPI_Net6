using Microsoft.AspNetCore.Mvc;
using MonarchsAPI_Net6.DTOs.RatingDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.RatingServices
{
    public interface IRatingServices
    {
        Task<List<Rating>> GetAll();
        Task<Rating?> GetById(int id);
        Task<bool> AddRating(Rating rating);
        Task<bool> EditRating(EditRatingDto ratingDto);
        Task<bool> DeleteRating(int id);
    }
}
