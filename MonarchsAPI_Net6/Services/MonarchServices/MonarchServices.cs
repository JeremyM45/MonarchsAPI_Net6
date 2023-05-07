using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;
using System.Diagnostics.Metrics;

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
            Monarch newMonarch = _mapper.Map<Monarch>(newMonarchDto);
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

        public async Task<Monarch> EditMonarch(EditMonarchRequestDto editedMonarchDto)
        {
            Monarch? monarchToEdit = await _dbContext.Monarchs.Where(m => m.Id == editedMonarchDto.Id).FirstOrDefaultAsync(); if(monarchToEdit == null) { throw new Exception(); }
            List<Country> currentCountries = await _dbContext.Countries.Where(c => c.Monarchs.Contains(monarchToEdit)).Include(c => c.Monarchs).ToListAsync();
            
            
            foreach (Country country in currentCountries)
            {
                country.Monarchs.Remove(monarchToEdit);
                
            }
            monarchToEdit.Countries = new List<Country>();
            await _dbContext.SaveChangesAsync();
            monarchToEdit.Name = editedMonarchDto.Name;
            monarchToEdit.Description = editedMonarchDto.Description;
            monarchToEdit.WikiLink = editedMonarchDto.WikiLink;
            monarchToEdit.Reign = editedMonarchDto.Reign;
            monarchToEdit.DynastyId = editedMonarchDto.DynastyId;

            List<Country> countries = await _dbContext.Countries.Include(c => c.Monarchs).ToListAsync();

            foreach (int countryId in editedMonarchDto.CountryIds)
            {   
                Country? countryToAdd = countries.Where(c => c.Id == countryId).FirstOrDefault();
                if(countryToAdd == null) { throw new Exception("Could Not Find Country By Id"); }
                monarchToEdit.Countries.Add(countryToAdd);
            }
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return await GetById(monarchToEdit.Id);
        }

        public async Task<bool> RemoveMonarch(int id)
        {
            Monarch? monarchToDelete = await _dbContext.Monarchs.FirstOrDefaultAsync(m => m.Id == id);
            if(monarchToDelete == null) { throw new Exception("Can't Find Monarch By Id"); }
            _dbContext.Monarchs.Remove(monarchToDelete);
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return true;
        }
    }
}
