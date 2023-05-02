using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
              
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Monarch> Monarchs { get; set; }
        public DbSet<Dynasty> Dynasties { get; set; }
        public DbSet<Country> Countries { get; set; }

    }
}
