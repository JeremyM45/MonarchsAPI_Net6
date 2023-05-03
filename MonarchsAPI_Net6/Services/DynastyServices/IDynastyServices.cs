using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.DynastyServices
{
    public interface IDynastyServices
    {
        Task<List<DynastyWithMonarchsDto>> GetAll();
    }
}
