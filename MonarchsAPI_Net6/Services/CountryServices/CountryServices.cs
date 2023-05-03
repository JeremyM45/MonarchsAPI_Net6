using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public class CountryServices : ICountryServices
    {
        private readonly DataContext _dataContext;
        public CountryServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<CountryWithMonarchsDto>> GetAll()
        {
            List<Country> countries = await _dataContext.Countries.Include(c => c.Monarchs).ThenInclude(m => m.Dynasty).ToListAsync();
            List<CountryWithMonarchsDto> countryWithMonarchsDtos = new List<CountryWithMonarchsDto>();
            foreach (Country country in countries) 
            {
                CountryWithMonarchsDto newCountryWithMonarchsDtos = new CountryWithMonarchsDto
                {
                    Id = country.Id,
                    Name = country.Name,
                    Monarchs = country.Monarchs
                };
                countryWithMonarchsDtos.Add(newCountryWithMonarchsDtos);
            }
            return countryWithMonarchsDtos;
        }
    }
}
