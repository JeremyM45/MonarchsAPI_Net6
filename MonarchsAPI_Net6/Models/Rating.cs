using System.Text.Json.Serialization;

namespace MonarchsAPI_Net6.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public float RatingValue { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int UserId { get; set; }
        [JsonIgnore] public User User { get; set; }
    }
}
