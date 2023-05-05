namespace MonarchsAPI_Net6.DTOs.RatingDtos
{
    public class CreateRatingDto
    {
        public float RatingValue { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int MonachId { get; set; }
    }
}
