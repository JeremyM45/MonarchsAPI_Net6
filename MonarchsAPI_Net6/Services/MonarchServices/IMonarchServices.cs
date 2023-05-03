using MonarchsAPI_Net6.DTOs;
using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.MonarchServices
{
    public interface IMonarchServices
    {
        Task<List<Monarch>> GetAll();
        Task<Monarch> GetById(int id);
        Task<Monarch> GetByName(string name);
        Task<bool> AddMonarch(CreateMonarchDto newMonarchDto);
    }
}
