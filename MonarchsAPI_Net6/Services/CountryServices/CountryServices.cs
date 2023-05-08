using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.CountryDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.CountryServices
{
    public class CountryServices : ICountryServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public CountryServices(DataContext dataContext, IMapper mapper)
        {
            _mapper= mapper;
            _dataContext = dataContext;
        }

        public async Task<bool> AddCountry(CreateCountryRequestDto countryDto)
        {
            Country newCountry = _mapper.Map<Country>(countryDto);
            try
            {
                await _dataContext.Countries.AddAsync(newCountry);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }

        public async Task<List<Country>> GetAll()
        {
            List<Country> countries = await _dataContext.Countries
                .Include(c => c.Monarchs).ThenInclude(m => m.Dynasty)
                .Include(c => c.Monarchs).ThenInclude(m => m.Ratings)
                .ToListAsync();
            
            return countries;
        }
        public async Task<List<Country>> GetAllMin()
        {
            List<Country> countries = await _dataContext.Countries.ToListAsync();

            return countries;
        }

        public async Task<List<int>> GetCountryIdsByMonarch(int monarchId)
        {
            Monarch? monarch = await _dataContext.Monarchs.Where(m => m.Id == monarchId).FirstOrDefaultAsync(); if(monarch == null) { throw new Exception(); }
            List<int> countryIds = new List<int>();
            foreach(Country country in monarch.Countries)
            {
                countryIds.Add(country.Id);
            }
            return countryIds;
        }
    }
}
