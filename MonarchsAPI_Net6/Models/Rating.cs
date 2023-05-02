namespace MonarchsAPI_Net6.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public float RatingValue { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
