namespace MonarchsAPI_Net6.DTOs.UserDtos
{
    public class UserDeleteRequestDto
    {
        public int Id { get; set; }
        public string Password { get; set; } = string.Empty;

    }
}
