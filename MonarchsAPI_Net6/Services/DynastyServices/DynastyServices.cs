using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public class DynastyServices : IDynastyServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DynastyServices(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = context;
        }

        public async Task<bool> AddDynasty(CreateDynastyDto dynastyDto)
        {
            Dynasty newDynasty = _mapper.Map<Dynasty>(dynastyDto);

            try
            {
                await _dataContext.Dynasties.AddAsync(newDynasty);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<List<Dynasty>> GetAll()
        {
            List<Dynasty> dynasties = await _dataContext.Dynasties
                .Include(d => d.Monarchs).ThenInclude(m => m.Countries)
                .Include(c => c.Monarchs).ThenInclude(m => m.Ratings)
                .ToListAsync();

            return dynasties;
        }
    }
}
