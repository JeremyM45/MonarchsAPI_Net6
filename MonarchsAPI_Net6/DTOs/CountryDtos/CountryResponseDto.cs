using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.DTOs.CountryDtos
{
    public class CountryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<MonarchMinDto> Monarchs { get; set; } = new List<MonarchMinDto>();
    }
}
