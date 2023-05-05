using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.MonarchServices
{
    public class MonarchServices : IMonarchServices
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        public MonarchServices(DataContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
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

        public async Task<bool> AddMonarch(CreateMonarchRequestDto newMonarchDto)
        {
            Console.WriteLine("Dto " + newMonarchDto.Name);
            Console.WriteLine("Dto " + newMonarchDto.Description);
            Console.WriteLine("Dto " + newMonarchDto.WikiLink);
            Console.WriteLine("Dto " + newMonarchDto.Reign);
            Console.WriteLine("Dto " + newMonarchDto.DynastyId);
            Monarch newMonarch = _mapper.Map<Monarch>(newMonarchDto);
            Console.WriteLine("New " + newMonarch.Name);
            Console.WriteLine("New " + newMonarch.Description);
            Console.WriteLine("New " + newMonarch.WikiLink);
            Console.WriteLine("New " + newMonarch.Reign);
            Console.WriteLine("New " + newMonarch.DynastyId);

            Dynasty? dynasty = await _dbContext.Dynasties.Where(d => d.Id == newMonarch.DynastyId).FirstOrDefaultAsync();
            if(dynasty == null) { return false; }
            newMonarch.Dynasty = dynasty;
            Country[] countries = await _dbContext.Countries.ToArrayAsync();
            foreach (int countryId in newMonarchDto.CountryIds)
            {
                Country? country = countries.Where(c => c.Id == countryId).FirstOrDefault();
                if(country == null) { break; }
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
