using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs;
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

        public async Task<Monarch> GetByName(string name)
        {
            Monarch? monarch = await _dbContext.Monarchs
                .Where(m => m.Name == name)
                .Include(m => m.Ratings)
                .Include(m => m.Dynasty)
                .Include(m => m.Countries)
                .FirstOrDefaultAsync();
            if (monarch == null) { return null; }
            return monarch;
        }

        public async Task<bool> AddMonarch(CreateMonarchDto newMonarchDto)
        {
            Monarch newMonarch = new Monarch
            {
                Name = newMonarchDto.Name,
                Description = newMonarchDto.Description,
                WikiLink = newMonarchDto.WikiLink,
                Reign = newMonarchDto.Reign,
                DynastyId = newMonarchDto.DyanstyId,
            };

            Dynasty? dynasty = await _dbContext.Dynasties.Where(d => d.Id == newMonarch.DynastyId).FirstOrDefaultAsync();
            if(dynasty == null) { return false; }
            newMonarch.Dynasty = dynasty;

            foreach(int countryId in newMonarchDto.CountryIds)
            {
                Country? country = await _dbContext.Countries.Where(c => c.Id == countryId).FirstOrDefaultAsync();
                if(country == null) { break; }
                Console.WriteLine(country.Name);
                newMonarch.Countries.Add(country);
            }
            try
            {
                _dbContext.Monarchs.Add(newMonarch);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
    }
}
