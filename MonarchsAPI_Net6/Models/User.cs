﻿namespace MonarchsAPI_Net6.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<Rating> Ratings { get; set; } = new List<Rating>();
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
