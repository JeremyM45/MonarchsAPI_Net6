using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public interface IDynastyServices
    {
        Task<List<Dynasty>> GetAll();
        Task<bool> AddDynasty(CreateDynastyDto dynastyDto);
        Task<Dynasty> EditDynasty(EditDynastyRequestDto dynastyDto);
        Task<bool> DeleteDynasty(int id);
    }
}
