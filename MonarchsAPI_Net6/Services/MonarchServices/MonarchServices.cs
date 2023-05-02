using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.MonarchServices
{
    public class MonarchServices : IMonarchServices
    {
        private readonly DataContext _dbContext;
        public MonarchServices(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Monarch>> GetAll()
        {
            return await _dbContext.Monarchs
                .Include(m => m.Ratings)
                .Include(m => m.Dynasty)
                .Include(m => m.Countries)
                .ToListAsync();
        }
    }
}
