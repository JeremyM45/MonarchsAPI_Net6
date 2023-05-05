namespace MonarchsAPI_Net6.DTOs.RatingDtos
{
    public class CreateRatingDto
    {
        public float ratingValue { get; set; }
        public string comment { get; set; } = string.Empty;
        public int UserId { get; set; }
    }
}
