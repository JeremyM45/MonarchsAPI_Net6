namespace MonarchsAPI_Net6.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Monarch> Monarchs { get; set; }
    }
}
