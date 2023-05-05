using MonarchsAPI_Net6.DTOs.MonarchsDtos;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.MonarchServices
{
    public interface IMonarchServices
    {
        Task<List<Monarch>> GetAll();
        Task<Monarch> GetById(int id);
        Task<Monarch> GetByName(string name);
        Task<bool> AddMonarch(CreateMonarchRequestDto newMonarchDto);
        Task<Monarch> EditMonarch(EditMonarchRequestDto editedMonarchDto);
        Task<bool> RemoveMonarch(int id);
    }
}
