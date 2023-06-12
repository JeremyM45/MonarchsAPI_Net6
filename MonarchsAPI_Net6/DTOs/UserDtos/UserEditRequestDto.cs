namespace MonarchsAPI_Net6.DTOs.UserDtos
{
    public class UserEditRequestDto
    {
        public string Username { get; set; } = string.Empty;
        public string NewUsername { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NewEmail { get; set; } = string.Empty;
    }
}
