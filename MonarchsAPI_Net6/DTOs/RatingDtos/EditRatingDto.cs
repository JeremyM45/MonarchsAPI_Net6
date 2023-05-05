namespace MonarchsAPI_Net6.DTOs.RatingDtos
{
    public class EditRatingDto
    {
        public int Id { get; set; }
        public float RatingValue { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
