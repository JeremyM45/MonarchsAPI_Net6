using MonarchsAPI_Net6.Models;

namespace MonarchsAPI_Net6.Services.MonarchServices
{
    public interface IMonarchServices
    {
        Task<List<Monarch>> GetAll();
    }
}
