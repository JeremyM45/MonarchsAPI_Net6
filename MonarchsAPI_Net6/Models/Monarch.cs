﻿namespace MonarchsAPI_Net6.Models
{
    public class Monarch
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WikiLink { get; set; } = string.Empty;
        public string Reign { get; set; } = string.Empty;
    }
}
