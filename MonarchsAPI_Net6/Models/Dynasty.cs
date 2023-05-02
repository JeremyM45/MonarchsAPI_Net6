using System.Text.Json.Serialization;

namespace MonarchsAPI_Net6.Models
{
    public class Dynasty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [JsonIgnore] public List<Monarch> Monarchs { get; set; }
    }
}
