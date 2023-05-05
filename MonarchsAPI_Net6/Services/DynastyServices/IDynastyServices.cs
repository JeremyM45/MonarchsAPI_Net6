using MonarchsAPI_Net6.DTOs.DyanstyDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public interface IDynastyServices
    {
        Task<List<Dynasty>> GetAll();
    }
}
