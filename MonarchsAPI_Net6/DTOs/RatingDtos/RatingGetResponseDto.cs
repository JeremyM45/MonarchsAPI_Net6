namespace MonarchsAPI_Net6.DTOs.RatingDtos
{
    public class RatingGetResponseDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
