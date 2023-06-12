using MonarchsAPI_Net6.DTOs.RatingDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.DTOs.UserDtos
{
    public class UserGetResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<RatingGetResponseDto> Ratings { get; set; } = new List<RatingGetResponseDto>();
    }
}
