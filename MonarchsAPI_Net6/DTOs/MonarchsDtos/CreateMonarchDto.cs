namespace MonarchsAPI_Net6.DTOs.MonarchsDtos
{
    public class CreateMonarchDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WikiLink { get; set; } = string.Empty;
        public string Reign { get; set; } = string.Empty;
        public int DyanstyId { get; set; }
        public List<int> CountryIds { get; set; }

    }
}
