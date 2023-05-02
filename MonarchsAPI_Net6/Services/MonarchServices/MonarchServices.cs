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

        public async Task<Monarch> GetById(int id)
        {
            Monarch? monarch = await _dbContext.Monarchs
                .Where(m => m.Id == id)
                .Include(m => m.Ratings)
                .Include(m => m.Dynasty)
                .Include(m => m.Countries)
                .FirstOrDefaultAsync();
            if(monarch == null) { return null; }
            return monarch;
        }
    }
}
