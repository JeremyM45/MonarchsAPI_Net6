using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public class DyanstyServices : IDynastyServices
    {
        private readonly DataContext _dataContext;

        public DyanstyServices(DataContext context)
        {
            _dataContext = context;
        }
        public async Task<List<DynastyWithMonarchsDto>> GetAll()
        {
            List<Dynasty> dynasties = await _dataContext.Dynasties
                .Include(d => d.Monarchs).ThenInclude(m => m.Countries)
                .Include(c => c.Monarchs).ThenInclude(m => m.Ratings)
                .ToListAsync();
            List<DynastyWithMonarchsDto> dynastyDtos = new List<DynastyWithMonarchsDto>();
            foreach(Dynasty dynasty in dynasties) 
            {
                DynastyWithMonarchsDto newDyanstyDto = new DynastyWithMonarchsDto
                {
                    Id = dynasty.Id,
                    Name = dynasty.Name,
                    Monarchs = dynasty.Monarchs,
                };
                dynastyDtos.Add(newDyanstyDto);
            }
            return dynastyDtos;
        }
    }
}
