using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.DTOs.DyanstyDtos
{
    public class DynastyResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Monarch> Monarchs { get; set; } = new List<Monarch>();
    }
}
