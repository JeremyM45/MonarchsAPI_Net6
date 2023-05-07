using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.DTOs.MonarchsDtos
{
    public class MonarchMinDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WikiLink { get; set; } = string.Empty;
        public string Reign { get; set; } = string.Empty;
        public int DynastyId { get; set; } = new int();
        public List<int> CountryIds { get; set; } = new List<int>();
    }
}
