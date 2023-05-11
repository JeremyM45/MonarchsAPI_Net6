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

        public async Task<Dynasty> EditDynasty(EditDynastyRequestDto dynastyDto)
        {
            Dynasty? dynastyToEdit = await _dataContext.Dynasties.Where(d => d.Id == dynastyDto.Id).FirstOrDefaultAsync(); if(dynastyToEdit == null){ throw new Exception(); }
            try
            {
                dynastyToEdit.Name = dynastyDto.Name;
                await _dataContext.SaveChangesAsync();
                return dynastyToEdit;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<bool> DeleteDynasty(int id)
        {
            Dynasty? dynastyToRemove = await _dataContext.Dynasties.Where(d => d.Id == id).FirstOrDefaultAsync(); if (dynastyToRemove == null) { throw new Exception(); }

            try
            {
                _dataContext.Dynasties.Remove(dynastyToRemove);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
