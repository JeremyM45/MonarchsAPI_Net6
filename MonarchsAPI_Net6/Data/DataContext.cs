using Microsoft.EntityFrameworkCore;

namespace MonarchsAPI_Net6.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
              
        }

    }
}
