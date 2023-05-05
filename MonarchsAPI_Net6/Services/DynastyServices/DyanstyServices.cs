using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MonarchsAPI_Net6.Data;
using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public class DyanstyServices : IDynastyServices
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DyanstyServices(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _dataContext = context;
        }
        public async Task<List<DynastyWithMonarchsDto>> GetAll()
        {
            List<Dynasty> dynasties = await _dataContext.Dynasties
                .Include(d => d.Monarchs).ThenInclude(m => m.Countries)
                .Include(c => c.Monarchs).ThenInclude(m => m.Ratings)
                .ToListAsync();

            return dynasties.Select(d => _mapper.Map<DynastyWithMonarchsDto>(d)).ToList();
        }
    }
}
