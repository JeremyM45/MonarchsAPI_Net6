using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.DTOs.UserDtos
{
    public class UserLoginResponseDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
